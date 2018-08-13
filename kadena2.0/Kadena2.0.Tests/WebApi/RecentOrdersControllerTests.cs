using Kadena.WebAPI.Controllers;
using Xunit;
using Moq;
using System.Web.Http.Results;
using Kadena.WebAPI.Infrastructure.Communication;
using Kadena.Dto.RecentOrders;
using System.Threading.Tasks;
using Kadena.BusinessLogic.Contracts;
using AutoMapper;
using System;
using Kadena.Dto.ViewOrder.Responses;
using Kadena.BusinessLogic.Factories;

namespace Kadena.Tests.WebApi
{
    public class RecentOrdersControllerTests : KadenaUnitTest<RecentOrdersController>
    {
        [Theory(DisplayName = "RecentOrdersController()")]
        [ClassData(typeof(RecentOrdersControllerTests))]
        public void RecentOrdersController(
            IOrderDetailService orderDetailService,
            IOrderListServiceFactory orderListServiceFactory,
            IOrderHistoryService orderHistoryService,
            IMapper mapper)
        {
            Assert.Throws<ArgumentNullException>(() => new RecentOrdersController(
                orderDetailService, 
                orderListServiceFactory, 
                orderHistoryService, 
                mapper));
        }


        [Fact(DisplayName = "RecentOrdersController.GetCampaignOrdersToApprove()")]
        public async Task GetCampaignOrdersToApprove()
        {
            Setup<IOrderListServiceFactory, IOrderListService>(s => s.GetRecentOrders(), new Mock<IOrderListService>().Object);

            var actualResult = await Sut.GetCampaignOrdersToApprove(string.Empty);

            Assert.IsType<JsonResult<BaseResponse<OrderHeadBlockDto>>>(actualResult);
        }

        [Fact(DisplayName = "RecentOrdersController.Get()")]
        public async Task Get()
        {
            Setup<IOrderListServiceFactory, IOrderListService>(s => s.GetRecentOrders(), new Mock<IOrderListService>().Object);

            var actualResult = await Sut.Get(string.Empty);

            Assert.IsType<JsonResult<BaseResponse<OrderDetailDTO>>>(actualResult);
        }
    }
}
