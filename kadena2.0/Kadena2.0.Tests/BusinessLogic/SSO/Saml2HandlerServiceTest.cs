using Xunit;
using Moq;
using Moq.AutoMock;
using Kadena.BusinessLogic.Services.SSO;
using Kadena.WebAPI.KenticoProviders.Contracts;

namespace Kadena.Tests.BusinessLogic.SSO
{
    public class Saml2HandlerServiceTest
    {
        [Fact(DisplayName = "Saml2HandlerService.GetTokenHandler()")]
        public void GetHandler()
        {
            var autoMock = new AutoMocker();
            var sut = autoMock.CreateInstance<Saml2HandlerService>();
            autoMock
                .GetMock<IKenticoResourceService>()
                .Setup(s => s.GetSettingsKey(It.IsAny<string>()))
                .Returns("SettingKeyValue");

            var actualResult = sut.GetTokenHandler();

            Assert.NotNull(actualResult);
        }
    }
}
