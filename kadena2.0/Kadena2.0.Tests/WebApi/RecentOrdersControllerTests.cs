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
using System.Collections.Generic;
using Kadena.Dto.ViewOrder.Responses;
using System.Linq;

namespace Kadena.Tests.WebApi
{
    public class RecentOrdersControllerTests : KadenaUnitTest<RecentOrdersController>
    {
        public static IEnumerable<object[]> GetDependencies()
        {
            var dependencies = new object[] {
                new Mock<IOrderDetailService>().Object,
                new Mock<IOrderListServiceFactory>().Object,
                new Mock<IMapper>().Object
            };

            foreach (var dep in dependencies)
            {
                yield return dependencies
                    .Select(d => d.Equals(dep) ? null : d)
                    .ToArray();
            }
        }

        [Theory(DisplayName = "RecentOrdersController()")]
        [MemberData(nameof(GetDependencies))]
        public void RecentOrdersController(IOrderDetailService orderDetailService,
            IOrderListServiceFactory orderListServiceFactory,
            IMapper mapper)
        {
            Assert.Throws<ArgumentNullException>(() => new RecentOrdersController(orderDetailService, orderListServiceFactory, mapper));
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
