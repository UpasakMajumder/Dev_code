using Xunit;
using Moq;
using Kadena.BusinessLogic.Services.SSO;
using Kadena.WebAPI.KenticoProviders.Contracts;

namespace Kadena.Tests.BusinessLogic.SSO
{
    public class Saml2HandlerServiceTest : KadenaUnitTest<Saml2HandlerService>
    {
        [Fact(DisplayName = "Saml2HandlerService.GetTokenHandler()")]
        public void GetHandler()
        {
            Setup<IKenticoResourceService, string>(s => s.GetSiteSettingsKey(It.IsAny<string>()), "SettingKeyValue");

            var actualResult = Sut.GetTokenHandler();

            Assert.NotNull(actualResult);
        }
    }
}
