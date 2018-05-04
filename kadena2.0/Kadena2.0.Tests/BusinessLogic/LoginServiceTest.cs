using Xunit;
using Kadena.BusinessLogic.Services;
using Kadena.WebAPI.KenticoProviders.Contracts;
using Kadena.Models.Membership;

namespace Kadena.Tests.BusinessLogic
{
    public class LoginServiceTest : KadenaUnitTest<LoginService>
    {
        [Fact(DisplayName = "LoginService.Logout() | Empty callback url")]
        public void LogoutEmpty()
        {
            var expectedResult = "/";
            Setup<IKenticoUserProvider, User>(s => s.GetCurrentUser(), new User());

            var actualResult = Sut.Logout();

            Assert.Equal(expectedResult, actualResult);
        }

        [Fact(DisplayName = "LoginService.Logout() | Non empty callback url")]
        public void LogoutNonEmpty()
        {
            var expectedResult = "http://example.com";
            Setup<IKenticoUserProvider, User>(s => s.GetCurrentUser(), new User { CallBackUrl = expectedResult });

            var actualResult = Sut.Logout();

            Assert.Equal(expectedResult, actualResult);
        }
    }
}
