using Kadena.Dto.General;
using Kadena.Dto.ViewOrder.MicroserviceResponses;
using Kadena.Models;
using Kadena.Models.Site;
using Kadena.WebAPI.KenticoProviders.Contracts;
using Kadena2.MicroserviceClients.Contracts;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Threading.Tasks;
using Xunit;
using Kadena2.WebAPI.KenticoProviders.Contracts;
using Kadena.Container.Default;
using Kadena.BusinessLogic.Services.Orders;

namespace Kadena.Tests.BusinessLogic
{
    public class OrderDetailServiceTests : KadenaUnitTest<OrderDetailService>
    {
        private BaseResponseDto<GetOrderByOrderIdResponseDTO> CreateOrderDetailDtoOK(OrderItemDTO[] items = null)
        {
            return new BaseResponseDto<GetOrderByOrderIdResponseDTO>()
            {
                Success = true,
                Payload = new GetOrderByOrderIdResponseDTO()
                {
                    Id = "1",
                    Items = new List<OrderItemDTO>(items ?? Enumerable.Empty<OrderItemDTO>()),
                    OrderDate = DateTime.Now,
                    PaymentInfo = new PaymentInfoDTO(),
                    ShippingInfo = new ShippingInfoDTO() { ShippingDate = null },
                    Status = "Shipped"
                }
            };
        }

        private BaseResponseDto<GetOrderByOrderIdResponseDTO> CreateOrderDetailDtoERROR()
        {
            return new BaseResponseDto<GetOrderByOrderIdResponseDTO>()
            {
                Success = false,
                Payload = null
            };
        }

        private void SetupBase()
        {
            Use(MapperBuilder.MapperInstance);

            Setup<IKenticoPermissionsProvider, bool>(p => p.UserCanSeeAllOrders(), false);
            Setup<IKenticoCustomerProvider, Customer>(p => p.GetCurrentCustomer(), new Customer() { Id = 10, UserID = 16 });
            Setup<IKenticoSiteProvider, KenticoSite>(p => p.GetKenticoSite(), new KenticoSite());
        }

        [Fact(DisplayName = "OrderDetailService.GetOrderDetail() | User has permission to view order")]
        public async Task OrderServiceTest_UserCanSee()
        {
            // Arrange
            SetupBase();
            Setup<IOrderViewClient, Task<BaseResponseDto<GetOrderByOrderIdResponseDTO>>>(o => o.GetOrderByOrderId("0010-0016-17-00006")
                , Task.FromResult(CreateOrderDetailDtoOK()));

            // Act
            var actualResult = await Sut.GetOrderDetail("0010-0016-17-00006");

            //Assert
            Assert.NotNull(actualResult);
        }

        [Fact(DisplayName = "OrderDetailService.GetOrderDetail() | User hasn't permission to view order")]
        public async Task OrderServiceTest_UserCannotSee()
        {
            // Arrange
            SetupBase();
            Setup<IOrderViewClient, Task<BaseResponseDto<GetOrderByOrderIdResponseDTO>>>(o => o.GetOrderByOrderId("0099-0099-17-00006")
                , Task.FromResult(CreateOrderDetailDtoERROR()));

            // Act
            Task action() => Sut.GetOrderDetail("0099-0099-17-00006");

            // Assert
            await Assert.ThrowsAsync<SecurityException>(action);
        }

        [Fact(DisplayName = "OrderDetailService.GetOrderDetail() | Microservice request error")]
        public async Task OrderServiceTest_MicroserviceErrorLogged()
        {
            // Arrange
            SetupBase();
            Setup<IOrderViewClient, Task<BaseResponseDto<GetOrderByOrderIdResponseDTO>>>(o => o.GetOrderByOrderId("0010-0016-66-00006")
                , Task.FromResult(CreateOrderDetailDtoERROR()));

            // Act
            var actualResult = await Sut.GetOrderDetail("0010-0016-66-00006");

            // Assert
            Assert.Null(actualResult);
            Verify<IKenticoLogger>(l => l.LogError("GetOrderDetail", ""), Times.Exactly(1));
        }

        [Theory(DisplayName = "OrderDetailService.GetOrderDetail() | Bad format of order id")]
        [InlineData("123")]
        [InlineData("asdgfdsrfgsdfg")]
        public async Task OrderServiceTest_BadFormatOrderId(string orderId)
        {
            // Act
            Task action() => Sut.GetOrderDetail(orderId);

            // Assert
            var exception = await Assert.ThrowsAsync<ArgumentOutOfRangeException>(action);
            Assert.Contains("Bad format of customer ID", exception.Message);
        }

        [Theory(DisplayName = "OrderDetailService.GetOrderDetail() | Empty order id")]
        [InlineData(null)]
        [InlineData("")]
        public async Task OrderServiceTest_EmptyOrderId(string orderId)
        {
            // Act
            Task action() => Sut.GetOrderDetail(orderId);

            // Assert
            await Assert.ThrowsAsync<ArgumentNullException>("orderId", action);
        }

        [Fact(DisplayName = "OrderDetailService.GetOrderDetail() | Empty shipping address with only mailing products")]
        public async Task OrderServiceTest_EmptyShippingAddressWhenMailingOnly()
        {
            // Arrange
            SetupBase();
            var orderId = "0010-0016-17-00006";
            var orderResponse = CreateOrderDetailDtoOK(new[]
            {
                new OrderItemDTO { Type = Dto.SubmitOrder.MicroserviceRequests.OrderItemTypeDTO.Mailing.ToString() }
            });
            Setup<IOrderViewClient, Task<BaseResponseDto<GetOrderByOrderIdResponseDTO>>>(o => o.GetOrderByOrderId(orderId), Task.FromResult(orderResponse));

            // Act
            var actualResult = await Sut.GetOrderDetail(orderId);

            // Assert
            Assert.Null(actualResult.ShippingInfo.Address);
        }

        [Fact(DisplayName = "OrderDetailService.GetOrderDetail() | Null dates")]
        public async Task NullDatetimeTests()
        {
            // Arrange
            SetupBase();
            var orderId = "0010-0016-17-00006";
            var orderResponse = CreateOrderDetailDtoOK(new[]
            {
                new OrderItemDTO { Type = Dto.SubmitOrder.MicroserviceRequests.OrderItemTypeDTO.Mailing.ToString() }
            });
            Setup<IOrderViewClient, Task<BaseResponseDto<GetOrderByOrderIdResponseDTO>>>(o => o.GetOrderByOrderId(orderId), Task.FromResult(orderResponse));

            // Act
            var actualResult = await Sut.GetOrderDetail(orderId).ConfigureAwait(false);

            // Assert
            Assert.NotEqual(actualResult.CommonInfo.OrderDate.Value, DateTime.MinValue);
            Assert.Null(actualResult.CommonInfo.ShippingDate.Value);
        }
    }
}