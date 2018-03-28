using Kadena.BusinessLogic.Contracts;
using Kadena.Dto.Logon.Requests;
using Kadena.WebAPI.Controllers;
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
    }
}
