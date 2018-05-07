using Xunit;
using Kadena.WebAPI.Controllers;
using System.Web.Http.Results;
using Kadena.Dto.BusinessUnits;
using Kadena.WebAPI.Infrastructure.Communication;

namespace Kadena.Tests.WebApi
{
    public class BusinessUnitsControllerTests : KadenaUnitTest<BusinessUnitsController>
    {
        [Fact(DisplayName = "BusinessUnitsController.GetSiteActiveBusinessUnits()")]
        public void GetSiteActiveBusinessUnits()
        {
            var actualResult = Sut.GetSiteActiveBusinessUnits();

            Assert.IsType<JsonResult<BaseResponse<BusinessUnitDto[]>>>(actualResult);
            Assert.NotNull(actualResult);
        }

        [Fact(DisplayName = "BusinessUnitsController.GetUserBusinessUnitData()")]
        public void GetUserBusinessUnitData()
        {
            var actualResult = Sut.GetUserBusinessUnitData(0);

            Assert.IsType<JsonResult<BaseResponse<BusinessUnitDto[]>>>(actualResult);
            Assert.NotNull(actualResult);
        }
    }
}
