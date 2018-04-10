using Kadena.BusinessLogic.Contracts;
using Kadena.Dto.Logon.Requests;
using Kadena.WebAPI.Controllers;
using Kadena.WebAPI.Infrastructure.Communication;
using Moq;
using System;
using System.Web.Http.Results;
using Xunit;

namespace Kadena.Tests.WebApi
{
    public class LoginControllerTest : KadenaUnitTest<LoginController>
    {
        [Fact(DisplayName = "LoginController.LoginSaml2() | NotFound")]
        public void LoginSaml2NotFound()
        {
            var actualResult = Sut.LoginSaml2(new SamlAuthenticationDto());

            Assert.IsType<NotFoundResult>(actualResult);
            Assert.NotNull(actualResult);
        }

        [Fact(DisplayName = "LoginController.LoginSaml2() | Redirect")]
        public void LoginSaml2Redirect()
        {
            var expectedResult = new Uri("http://example.com");
            Setup<IIdentityService, Uri>(i => i.TryAuthenticate(It.IsAny<string>()), expectedResult);

            var actualResult = Sut.LoginSaml2(new SamlAuthenticationDto());

            Assert.IsType<RedirectResult>(actualResult);
            Assert.NotNull(actualResult);
            Assert.Equal(expectedResult, (actualResult as RedirectResult).Location);
        }

        [Fact(DisplayName = "LoginController.Logout()")]
        public void Logout()
        {
            var expectedResult = "http://example.com";
            Setup<ILoginService, string>(i => i.Logout(), expectedResult);

            var actualResult = Sut.Logout() as JsonResult<BaseResponse<string>>;

            Assert.NotNull(actualResult);
            Assert.True(actualResult.Content.Success);
            Assert.Equal(expectedResult, actualResult.Content.Payload);
        }
    }
}
