using Kadena.WebAPI.Controllers;
using Xunit;
using Moq;
using System.Web.Http.Results;
using Kadena.WebAPI.Infrastructure.Communication;
using Kadena.Dto.RecentOrders;
using System.Threading.Tasks;
using Kadena.BusinessLogic.Contracts;

namespace Kadena.Tests.WebApi
{
    public class RecentOrdersControllerTests: KadenaUnitTest<RecentOrdersController>
    {
        [Fact(DisplayName = "RecentOrdersController.GetCampaignOrdersToApprove()")]
        public async Task GetCampaignOrdersToApprove()
        {
            Setup<IOrderListServiceFactory, IOrderListService>(s => s.GetRecentOrders(), new Mock<IOrderListService>().Object);

            var actualResult = await Sut.GetCampaignOrdersToApprove(string.Empty);

            Assert.IsType<JsonResult<BaseResponse<OrderHeadBlockDto>>>(actualResult);
        }
    }
}
