using Kadena.WebAPI.Controllers;
using Xunit;
using System.Web.Http.Results;
using Kadena.WebAPI.Infrastructure.Communication;
using Kadena.Dto.RecentOrders;

namespace Kadena.Tests.WebApi
{
    public class CampaignsContollerTests : KadenaUnitTest<CampaignsController>
    {
        [Fact(DisplayName = "CampaignsController.DeleteCampaign()")]
        public void DeleteCampaign()
        {
            var actualResult = Sut.DeleteCampaign(0);

            Assert.IsType<JsonResult<BaseResponse<string>>>(actualResult);
        }

        [Fact(DisplayName = "CampaignsController.GetCampaigns()")]
        public void GetCampaigns()
        {
            var actualResult = Sut.GetCampaigns(string.Empty);

            Assert.IsType<JsonResult<BaseResponse<OrderCampaginHeadDto>>>(actualResult);
        }
    }
}
