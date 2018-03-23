using Kadena.WebAPI.Controllers;
using Kadena.WebAPI.Infrastructure.Communication;
using Moq.AutoMock;
using System.Web.Http.Results;
using Xunit;

namespace Kadena.Tests.WebApi
{
    public class UserControllerTest
    {
        [Fact(DisplayName = "UserController.AcceptTaC()")]
        public void AcceptTac()
        {
            var autoMock = new AutoMocker();
            var sut = autoMock.CreateInstance<UserController>();

            var actualResult = sut.AcceptTaC();

            Assert.IsType<JsonResult<BaseResponse<object>>>(actualResult);
            Assert.NotNull(actualResult);
        }
    }
}
