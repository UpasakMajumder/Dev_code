using Kadena.BusinessLogic.Contracts;
using Kadena.WebAPI.KenticoProviders.Contracts;
using Kadena2.BusinessLogic.Services.OrderPayment;
using Kadena.Models.CreditCard;
using Moq;
using Moq.AutoMock;
using System.Threading.Tasks;
using Xunit;
using System;
using Kadena2.MicroserviceClients.Contracts;
using Kadena.Dto.CreditCard.MicroserviceRequests;

namespace Kadena.Tests._3dsiCreditCard
{
    public class CreditCard3dsiTests
    {
        [Theory]
        [InlineData("https://someurl/", "https://someurl/?submissionId=7c9e6679-7425-40de-944b-e07fc1f90ae7")]
        [InlineData("https://someurl", "https://someurl/?submissionId=7c9e6679-7425-40de-944b-e07fc1f90ae7")]
        [InlineData("https://someurl?lang=en", "https://someurl/?lang=en&submissionId=7c9e6679-7425-40de-944b-e07fc1f90ae7")]
        public async Task Card3dsi_SubmisisonInQuerystring(string serviceUrl, string finalUrl)
        {
            const string guid = "7c9e6679-7425-40de-944b-e07fc1f90ae7";

            var autoMocker = new AutoMocker();
            var sut = autoMocker.CreateInstance<CreditCard3dsi>();
            var resources = autoMocker.GetMock<IKenticoResourceService>();
            resources.Setup(r => r.GetSettingsKey("KDA_CreditCard_InsertCardDetailsURL"))
                .Returns(serviceUrl);

            var documents = autoMocker.GetMock<IKenticoDocumentProvider>()
                .Setup(d => d.GetDocumentUrl(serviceUrl, true))
                .Returns(serviceUrl);

            var submissions = autoMocker.GetMock<ISubmissionService>();
            submissions.Setup(s => s.GenerateNewSubmission(It.IsAny<string>()))
                .Returns(new Submission() { SubmissionId = Guid.Parse(guid) });

            // Act
            var result = await sut.PayByCard3dsi(null);

            // Assert
            Assert.Equal(finalUrl, result.RedirectURL);

        }

        [Fact]
        public async Task SaveToken_NullRequest()
        {
            // Arrange
            var autoMocker = new AutoMocker();
            var sut = autoMocker.CreateInstance<CreditCard3dsi>();
            var loggerMock = autoMocker.GetMock<IKenticoLogger>();

            // Act
            var result = await sut.SaveToken(null);

            // Assert
            Assert.False(result);
            loggerMock.Verify(l => l.LogError("3DSi SaveToken", "Empty or unknown format SaveTokenData"), Times.Once);
        }


        [Fact]
        public async Task SaveToken_NullSubmission()
        {
            // Arrange
            var autoMocker = new AutoMocker();
            var sut = autoMocker.CreateInstance<CreditCard3dsi>();
            var loggerMock = autoMocker.GetMock<IKenticoLogger>();

            var saveData = new SaveTokenData()
            {
                SubmissionID = "sbmid"
            };

            // Act
            var result = await sut.SaveToken(saveData);

            // Assert
            Assert.False(result);
            loggerMock.Verify(l => l.LogError("3DSi SaveToken", "Unknown or already used submissionId : sbmid"), Times.Once);
            
        }

        [Fact]
        public async Task SaveToken_NotVerifiedSubmission()
        {
            // Arrange
            var autoMocker = new AutoMocker();
            var sut = autoMocker.CreateInstance<CreditCard3dsi>();
            var loggerMock = autoMocker.GetMock<IKenticoLogger>();
            var submissionMock = autoMocker.GetMock<ISubmissionService>();
            submissionMock.Setup(s => s.GetSubmission("sbmid"))
                .Returns(new Submission() {AlreadyVerified = false });
            var saveData = new SaveTokenData()
            {
                SubmissionID = "sbmid"
            };

            // Act
            var result = await sut.SaveToken(saveData);

            // Assert
            Assert.False(result);
            loggerMock.Verify(l => l.LogError("3DSi SaveToken", "Unknown or already used submissionId : sbmid"), Times.Once);
        }


        [Theory]
        [InlineData("2012", 2020, 12)]
        [InlineData("3412", 2034, 12)]
        public async Task SaveTokenToUserData_ParseExpirationDateOk(string date, int year, int month)
        {
            // Arrange
            var autoMocker = new AutoMocker();
            var sut = autoMocker.CreateInstance<CreditCard3dsi>();
            var userClientMock = autoMocker.GetMock<IUserDataServiceClient>();
            var submission = new Submission();
            var saveData = new SaveTokenData()
            {
                ExpirationDate = date
            };

            // Act
            var result = await sut.SaveTokenToUserData(submission, saveData);

            // Assert
            userClientMock.Verify(u => u.SaveCardToken(It.Is<SaveCardTokenRequestDto>(s => s.CardExpirationDate.Year == year && 
                                                                                           s.CardExpirationDate.Month == month )), Times.Once);
        }

        [Fact]
        public async Task SaveTokenToUserData_ParseExpirationDateNull()
        {
            // Arrange
            var autoMocker = new AutoMocker();
            var sut = autoMocker.CreateInstance<CreditCard3dsi>();
            var submission = new Submission();
            var saveData = new SaveTokenData()
            {
                ExpirationDate = null
            };

            // Act
            var result = sut.SaveTokenToUserData(submission, saveData);

            // Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await result);
        }

        [Theory]
        [InlineData("")]
        [InlineData("1818")]
        [InlineData("201810")]
        [InlineData("1234")]
        public async Task SaveTokenToUserData_ParseExpirationDateBad(string date)
        {
            // Arrange
            var autoMocker = new AutoMocker();
            var sut = autoMocker.CreateInstance<CreditCard3dsi>();
            var submission = new Submission();
            var saveData = new SaveTokenData()
            {
                ExpirationDate = date
            };

            // Act
            var result = sut.SaveTokenToUserData(submission, saveData);

            // Assert
            await Assert.ThrowsAsync<FormatException>(async () => await result);
        }
    }
}
