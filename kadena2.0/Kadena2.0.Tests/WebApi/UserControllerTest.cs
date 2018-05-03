using Kadena.Dto.Logon.Requests;
using Kadena.Dto.Logon.Responses;
using Kadena.WebAPI.Controllers;
using Kadena.WebAPI.Infrastructure.Communication;
using System.Web.Http.Results;
using Xunit;

namespace Kadena.Tests.WebApi
{
    public class UserControllerTest : KadenaUnitTest<UserController>
    {
        [Fact(DisplayName = "UserController.AcceptTaC()")]
        public void AcceptTac()
        {
            var actualResult = Sut.AcceptTaC();

            Assert.IsType<JsonResult<BaseResponse<object>>>(actualResult);
            Assert.NotNull(actualResult);
        }

        [Fact(DisplayName = "UserController.CheckTaC()")]
        public void CheckTaC()
        {
            var actualResult = Sut.CheckTaC();

            Assert.IsType<JsonResult<BaseResponse<CheckTaCResultDTO>>>(actualResult);
            Assert.NotNull(actualResult);
        }

        [Fact(DisplayName = "UserController.Register()")]
        public void Register()
        {
            var actualResult = Sut.Register(new RegistrationDto());

            Assert.IsType<JsonResult<BaseResponse<object>>>(actualResult);
            Assert.NotNull(actualResult);
        }
    }
}
