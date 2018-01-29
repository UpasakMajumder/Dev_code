using Kadena.BusinessLogic.Contracts;
using Kadena.Dto.SubmitOrder.MicroserviceRequests;
using Kadena.WebAPI.KenticoProviders.Contracts;
using Kadena2._0.BusinessLogic.Contracts.Orders;
using Kadena2.BusinessLogic.Services.OrderPayment;
using Kadena.Models.CreditCard;
using Moq;
using Moq.AutoMock;
using System.Threading.Tasks;
using Xunit;
using System;

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
                .Setup(d => d.GetDocumentUrl(serviceUrl))
                .Returns(serviceUrl);

            var submissions = autoMocker.GetMock<ISubmissionService>();
            submissions.Setup(s => s.GenerateNewSubmission(It.IsAny<string>()))
                .Returns(new Submission() { SubmissionId = Guid.Parse(guid) });

            // Act
            var result = await sut.PayByCard3dsi(null);

            // Assert
            Assert.Equal(finalUrl, result.RedirectURL);

        }
    }
}
