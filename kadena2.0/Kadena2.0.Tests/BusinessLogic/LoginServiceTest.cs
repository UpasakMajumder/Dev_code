using Xunit;
using Moq.AutoMock;
using Moq;
using Kadena.BusinessLogic.Services;
using Kadena.WebAPI.KenticoProviders.Contracts;
using Kadena.Models.Membership;

namespace Kadena.Tests.BusinessLogic
{
    public class LoginServiceTest
    {
        [Fact(DisplayName = "LoginService.Logout() | Empty callback url")]
        public void LogoutEmpty()
        {
            var expectedResult = "/";
            var autoMock = new AutoMocker();
            var userProvider = autoMock.GetMock<IKenticoUserProvider>();
            userProvider.Setup(s => s.GetCurrentUser())
                .Returns(new User());
            var sut = autoMock.CreateInstance<LoginService>();

            var actualResult = sut.Logout();

            Assert.Equal(expectedResult, actualResult);
        }

        [Fact(DisplayName = "LoginService.Logout() | Non empty callback url")]
        public void LogoutNonEmpty()
        {
            var expectedResult = "http://example.com";
            var autoMock = new AutoMocker();
            var userProvider = autoMock.GetMock<IKenticoUserProvider>();
            userProvider.Setup(s => s.GetCurrentUser())
                .Returns(new User());
            userProvider.Setup(s => s.GetUserSettings(It.IsAny<int>()))
                .Returns(new UserSettings { CallBackUrl = expectedResult });
            var sut = autoMock.CreateInstance<LoginService>();

            var actualResult = sut.Logout();

            Assert.Equal(expectedResult, actualResult);
        }
    }
}
