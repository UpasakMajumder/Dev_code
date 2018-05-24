using Kadena.BusinessLogic.Services.Orders;
using Xunit;
using Moq;
using System.Threading.Tasks;
using System;
using Kadena2.WebAPI.KenticoProviders.Contracts;
using Kadena.Models.SiteSettings.Permissions;
using Kadena.WebAPI.KenticoProviders.Contracts;
using Kadena.Models;
using AutoMapper;
using Kadena.Dto.Order;

namespace Kadena.Tests.BusinessLogic
{
    public class OrderListServiceTests : KadenaUnitTest<OrderListService>
    {
        [Fact(DisplayName = "OrderListService.GetCampaignOrdersToApprove() | Success")]
        public async Task GetCampaignOrdersToApprove_Success()
        {
            Setup<IKenticoCustomerProvider, Customer>(s => s.GetCurrentCustomer(), new Customer());
            Setup<IMapper, OrderList>(s => s.Map<OrderList>(It.IsNotNull<OrderListDto>()), new OrderList { Orders = new Order[0] });

            Setup<IKenticoPermissionsProvider, bool>(
                s => s.CurrentUserHasPermission(ModulePermissions.KadenaOrdersModule, ModulePermissions.KadenaOrdersModule.ApproveOrders,
                    It.IsAny<string>()),
                true);

            var actualResult = await  Sut.GetCampaignOrdersToApprove(string.Empty, 0);

            Assert.NotNull(actualResult);
        }

        [Fact(DisplayName = "OrderListService.GetCampaignOrdersToApprove() | Not enough permissions")]
        public async Task GetCampaignOrdersToApprove_Denied()
        {
            Task action() => Sut.GetCampaignOrdersToApprove(string.Empty, 0);

            await Assert.ThrowsAsync<UnauthorizedAccessException>(action);
        }
    }
}
