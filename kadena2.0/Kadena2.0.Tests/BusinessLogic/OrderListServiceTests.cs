using AutoMapper;
using Kadena.BusinessLogic.Services.Orders;
using Kadena.Dto.General;
using Kadena.Dto.Order;
using Kadena.Models;
using Kadena.Models.Checkout;
using Kadena.Models.Site;
using Kadena.Models.SiteSettings.Permissions;
using Kadena.WebAPI.KenticoProviders.Contracts;
using Kadena2.MicroserviceClients.Contracts;
using Kadena2.WebAPI.KenticoProviders.Contracts;
using Moq;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Kadena.Tests.BusinessLogic
{
    public class OrderListServiceTests : KadenaUnitTest<OrderListService>
    {
        [Fact(DisplayName = "OrderListService.GetCampaignOrdersToApprove() | Success")]
        public async Task GetCampaignOrdersToApprove_Success()
        {
            Setup<IKenticoCustomerProvider, Customer>(s => s.GetCurrentCustomer(), new Customer());
            Setup<IOrderViewClient, Task<BaseResponseDto<OrderListDto>>>(
                ovc => ovc.GetOrders(It.IsAny<OrderListFilter>()), 
                Task.FromResult(new BaseResponseDto<OrderListDto> { Success = true, Payload = new OrderListDto { Orders = new RecentOrderDto[0] } }));
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

        [Fact]
        public async Task GetHeaders_ShouldExcludeRemovedLineItems()
        {
            Setup<IOrderViewClient, Task<BaseResponseDto<OrderListDto>>>(
                ovc => ovc.GetOrders(It.IsAny<OrderListFilter>()),
                Task.FromResult(new BaseResponseDto<OrderListDto>
                {
                    Success = true,
                    Payload = new OrderListDto { Orders = new RecentOrderDto[0] }
                }));

            Setup<IKenticoSiteProvider, KenticoSite>(s => s.GetKenticoSite(), new KenticoSite());

            Setup<IMapper, OrderList>(
                s => s.Map<OrderList>(It.IsNotNull<OrderListDto>()),
                new OrderList { Orders = new[] 
                {
                    new Order
                    {
                        Items = new []
                        {
                            new CheckoutCartItem { Quantity = 1 },
                            new CheckoutCartItem { Quantity = 0 },
                            new CheckoutCartItem { Quantity = 2 },
                        }
                    }
                } });

            var result = await Sut.GetHeaders();
            
            Assert.Equal(2, result.Rows.First().Items.Count());
        }
    }
}
