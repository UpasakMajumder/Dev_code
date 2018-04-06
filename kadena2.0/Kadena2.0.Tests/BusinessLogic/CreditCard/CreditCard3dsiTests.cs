using Kadena.BusinessLogic.Contracts;
using Kadena.WebAPI.KenticoProviders.Contracts;
using Kadena2.BusinessLogic.Services.OrderPayment;
using Kadena.Models.CreditCard;
using Moq;
using System.Threading.Tasks;
using Xunit;
using System;
using Kadena2.MicroserviceClients.Contracts;
using Kadena.Dto.CreditCard.MicroserviceRequests;

namespace Kadena.Tests.CreditCard
{
    public class CreditCard3dsiTests : KadenaUnitTest<CreditCard3dsi>
    {
        [Theory(DisplayName = "CreditCard3dsi.PayByCard3dsi()")]
        [InlineData("https://someurl/", "https://someurl/?submissionId=7c9e6679-7425-40de-944b-e07fc1f90ae7")]
        [InlineData("https://someurl", "https://someurl/?submissionId=7c9e6679-7425-40de-944b-e07fc1f90ae7")]
        [InlineData("https://someurl?lang=en", "https://someurl/?lang=en&submissionId=7c9e6679-7425-40de-944b-e07fc1f90ae7")]
        public async Task Card3dsi_SubmisisonInQuerystring(string serviceUrl, string finalUrl)
        {
            const string guid = "7c9e6679-7425-40de-944b-e07fc1f90ae7";

            Setup<IKenticoResourceService, string>(r => r.GetSiteSettingsKey("KDA_CreditCard_InsertCardDetailsURL"), serviceUrl);
            Setup<IKenticoDocumentProvider, string>(d => d.GetDocumentUrl(serviceUrl, true), serviceUrl);
            Setup<ISubmissionService, Submission>(s => s.GenerateNewSubmission(It.IsAny<string>()), new Submission() { SubmissionId = Guid.Parse(guid) });

            // Act
            var result = await Sut.PayByCard3dsi(null);

            // Assert
            Assert.Equal(finalUrl, result.RedirectURL);
        }

        [Fact(DisplayName = "CreditCard3dsi.SaveToken() | Null request")]
        public async Task SaveToken_NullRequest()
        {
            // Act
            var result = await Sut.SaveToken(null);

            // Assert
            Assert.False(result);
            Verify<IKenticoLogger>(l => l.LogError("3DSi SaveToken", "Empty or unknown format SaveTokenData"), Times.Once);
        }


        [Fact(DisplayName = "CreditCard3dsi.SaveToken() | Null submission")]
        public async Task SaveToken_NullSubmission()
        {
            // Arrange
            var saveData = new SaveTokenData()
            {
                SubmissionID = "sbmid"
            };

            // Act
            var result = await Sut.SaveToken(saveData);

            // Assert
            Assert.False(result);
            Verify<IKenticoLogger>(l => l.LogError("3DSi SaveToken", "Unknown or already used submissionId : sbmid"), Times.Once);
        }

        [Fact(DisplayName = "CreditCard3dsi.SaveToken() | Submission isn't verified")]
        public async Task SaveToken_NotVerifiedSubmission()
        {
            // Arrange
            Setup<ISubmissionService, Submission>(s => s.GetSubmission("sbmid"), new Submission() { AlreadyVerified = false });
            var saveData = new SaveTokenData()
            {
                SubmissionID = "sbmid"
            };

            // Act
            var result = await Sut.SaveToken(saveData);

            // Assert
            Assert.False(result);
            Verify<IKenticoLogger>(l => l.LogError("3DSi SaveToken", "Unknown or already used submissionId : sbmid"), Times.Once);
        }


        [Theory(DisplayName = "CreditCard3dsi.SaveTokenToUserData() | Valid expiration date")]
        [InlineData("2012", 2020, 12)]
        [InlineData("3412", 2034, 12)]
        public async Task SaveTokenToUserData_ParseExpirationDateOk(string date, int year, int month)
        {
            // Arrange
            var submission = new Submission();
            var saveData = new SaveTokenData()
            {
                ExpirationDate = date
            };

            // Act
            var result = await Sut.SaveTokenToUserData(submission, saveData);

            // Assert
            Verify<IUserDataServiceClient>(u => u.SaveCardToken(It.Is<SaveCardTokenRequestDto>(s => s.CardExpirationDate.Year == year 
                && s.CardExpirationDate.Month == month)), Times.Once);
        }

        [Fact(DisplayName = "CreditCard3dsi.SaveTokenToUserData() | Null expiration date")]
        public async Task SaveTokenToUserData_ParseExpirationDateNull()
        {
            // Arrange
            var submission = new Submission();
            var saveData = new SaveTokenData()
            {
                ExpirationDate = null
            };

            // Act
            Task action() => Sut.SaveTokenToUserData(submission, saveData);

            // Assert
            await Assert.ThrowsAsync<ArgumentNullException>(action);
        }

        [Theory(DisplayName = "CreditCard3dsi.SaveTokenToUserData() | Invalid expiration date")]
        [InlineData("")]
        [InlineData("1818")]
        [InlineData("201810")]
        [InlineData("1234")]
        public async Task SaveTokenToUserData_ParseExpirationDateBad(string date)
        {
            // Arrange
            var submission = new Submission();
            var saveData = new SaveTokenData()
            {
                ExpirationDate = date
            };

            // Act
            Task action() => Sut.SaveTokenToUserData(submission, saveData);

            // Assert
            await Assert.ThrowsAsync<FormatException>(action);
        }
    }
}
