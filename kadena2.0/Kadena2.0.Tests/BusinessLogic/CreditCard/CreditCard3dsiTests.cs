using Kadena.BusinessLogic.Contracts;
using Kadena.WebAPI.KenticoProviders.Contracts;
using Kadena2.BusinessLogic.Services.OrderPayment;
using Kadena.Models.CreditCard;
using Moq;
using System.Threading.Tasks;
using Xunit;
using System;

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

            Setup<IKenticoResourceService, string>(r => r.GetSettingsKey("KDA_CreditCard_InsertCardDetailsURL"), serviceUrl);
            Setup<IKenticoDocumentProvider, string>(d => d.GetDocumentUrl(serviceUrl, true), serviceUrl);
            Setup<ISubmissionService, Submission>(s => s.GenerateNewSubmission(It.IsAny<string>()), new Submission() { SubmissionId = Guid.Parse(guid) });

            // Act
            var result = await Sut.PayByCard3dsi(null);

            // Assert
            Assert.Equal(finalUrl, result.RedirectURL);
        }
    }
}
