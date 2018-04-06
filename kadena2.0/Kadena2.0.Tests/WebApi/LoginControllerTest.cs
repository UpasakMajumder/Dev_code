using Kadena.BusinessLogic.Contracts;
using Kadena.Dto.Logon.Requests;
using Kadena.WebAPI.Controllers;
using Kadena.WebAPI.Infrastructure.Communication;
using Moq;
using Moq.AutoMock;
using System;
using System.Web.Http.Results;
using Xunit;

namespace Kadena.Tests.WebApi
{
    public class LoginControllerTest
    {
        [Fact(DisplayName = "LoginController.LoginSaml2() | NotFound")]
        public void LoginSaml2NotFound()
        {
            var autoMock = new AutoMocker();
            var sut = autoMock.CreateInstance<LoginController>();

            var actualResult = sut.LoginSaml2(new SamlAuthenticationDto());

            Assert.IsType<NotFoundResult>(actualResult);
            Assert.NotNull(actualResult);
        }

        [Fact(DisplayName = "LoginController.LoginSaml2() | Redirect")]
        public void LoginSaml2Redirect()
        {
            var expectedResult = new Uri("http://example.com");
            var autoMock = new AutoMocker();
            var identityService = autoMock.GetMock<IIdentityService>();
            identityService
                .Setup(i => i.TryAuthenticate(It.IsAny<string>()))
                .Returns(expectedResult);
            var sut = autoMock.CreateInstance<LoginController>();

            var actualResult = sut.LoginSaml2(new SamlAuthenticationDto());

            Assert.IsType<RedirectResult>(actualResult);
            Assert.NotNull(actualResult);
            Assert.Equal(expectedResult, (actualResult as RedirectResult).Location);
        }

        [Fact(DisplayName = "LoginController.Logout()")]
        public void Logout()
        {
            var expectedResult = "http://example.com";
            var autoMock = new AutoMocker();
            autoMock
                .GetMock<ILoginService>()
                .Setup(i => i.Logout())
                .Returns(expectedResult);
            var sut = autoMock.CreateInstance<LoginController>();

            var actualResult = sut.Logout() as JsonResult<BaseResponse<string>>;

            Assert.NotNull(actualResult);
            Assert.True(actualResult.Content.Success);
            Assert.Equal(expectedResult, actualResult.Content.Payload);
        }
    }
}
