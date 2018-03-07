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
using Kadena2.Container.Default;
using Kadena.BusinessLogic.Services.Orders;
using Moq.AutoMock;
using AutoMapper;

namespace Kadena.Tests.WebApi
{
    public class OrderDetailServiceTests
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

        private AutoMocker CreateOrderDetailServiceAutomocker()
        {
            var autoMocker = new AutoMocker();
            autoMocker.Use<IMapper>(MapperBuilder.MapperInstance);
            
            autoMocker.GetMock<IKenticoPermissionsProvider>().Setup(p => p.UserCanSeeAllOrders())
                .Returns(false);
            autoMocker.GetMock<IKenticoUserProvider>().Setup(p => p.GetCurrentCustomer())
               .Returns(new Customer() { Id = 10, UserID = 16 });
            autoMocker.GetMock<IKenticoSiteProvider>().Setup(p => p.GetKenticoSite())
                .Returns(new KenticoSite());

            return autoMocker;
        }

        [Fact]
        public async Task OrderServiceTest_UserCanSee()
        {
            // Arrange
            var autoMocker = CreateOrderDetailServiceAutomocker();
            var orderViewClient = autoMocker.GetMock<IOrderViewClient>();
            orderViewClient.Setup(o => o.GetOrderByOrderId("0010-0016-17-00006"))
                .Returns(Task.FromResult(CreateOrderDetailDtoOK()));
            var sut = autoMocker.CreateInstance<OrderDetailService>();

            // Act
            var result = await sut.GetOrderDetail("0010-0016-17-00006");

            //Assert
            Assert.NotNull(result);
        }
        
        [Fact]
        public async Task OrderServiceTest_UserCannotSee()
        {
            // Arrange
            var autoMocker = CreateOrderDetailServiceAutomocker();
            var orderViewClient = autoMocker.GetMock<IOrderViewClient>();
            orderViewClient.Setup(o => o.GetOrderByOrderId("0099-0099-17-00006"))
                .Returns(Task.FromResult(CreateOrderDetailDtoERROR()));
            var sut = autoMocker.CreateInstance<OrderDetailService>();

            // Act
            var result = sut.GetOrderDetail("0099-0099-17-00006");

            // Assert
            await Assert.ThrowsAsync<SecurityException>(async () => await result);
        }

        [Fact]
        public async Task OrderServiceTest_MicroserviceErrorLogged()
        {
            // Arrange
            var autoMocker = CreateOrderDetailServiceAutomocker();
            var orderViewClient = autoMocker.GetMock<IOrderViewClient>();
            orderViewClient.Setup(o => o.GetOrderByOrderId("0010-0016-66-00006"))
                .Returns(Task.FromResult(CreateOrderDetailDtoERROR()));
            var sut = autoMocker.CreateInstance<OrderDetailService>();

            // Act
            var result = await sut.GetOrderDetail("0010-0016-66-00006");

            // Assert
            Assert.Null(result);
            autoMocker.GetMock<IKenticoLogger>().Verify(l => l.LogError("GetOrderDetail", ""), Times.Exactly(1));
        }

        [Theory]
        [InlineData("123")]
        [InlineData("asdgfdsrfgsdfg")]
        public async Task OrderServiceTest_BadFormatOrderId(string orderId)
        {
            // Arrange
            var sut = new AutoMocker().CreateInstance<OrderDetailService>();

            // Act
            var result = sut.GetOrderDetail(orderId);

            // Assert
            var exception = await Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () => await result);
            Assert.Contains("Bad format of customer ID", exception.Message);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public async Task OrderServiceTest_EmptyOrderId(string orderId)
        {
            // Arrange
            var sut = new AutoMocker().CreateInstance<OrderDetailService>();

            // Act
            var result = sut.GetOrderDetail(orderId);

            // Assert
            var exception = await Assert.ThrowsAsync<ArgumentNullException>(async () => await result);
            Assert.Equal("orderId", exception.ParamName);
        }

        [Fact]
        public async Task OrderServiceTest_EmptyShippingAddressWhenMailingOnly()
        {
            // Arrange
            var autoMocker = CreateOrderDetailServiceAutomocker();
            var orderId = "0010-0016-17-00006";
            var orderViewClient = autoMocker.GetMock<IOrderViewClient>();
            var orderResponse = CreateOrderDetailDtoOK(new[] 
            {
                new OrderItemDTO { Type = Dto.SubmitOrder.MicroserviceRequests.OrderItemTypeDTO.Mailing.ToString() }
            });
            orderViewClient.Setup(o => o.GetOrderByOrderId(orderId))
                .Returns(Task.FromResult(orderResponse));
            var sut = autoMocker.CreateInstance<OrderDetailService>();

            // Act
            var result = await sut.GetOrderDetail(orderId);

            // Assert
            Assert.Null(result.ShippingInfo.Address);
        }


        [Fact]
        public async Task NullDatetimeTests()
        {
            // Arrange
            var autoMocker = CreateOrderDetailServiceAutomocker();
            var orderId = "0010-0016-17-00006";
            var orderViewClient = autoMocker.GetMock<IOrderViewClient>();
            var orderResponse = CreateOrderDetailDtoOK(new[]
            {
                new OrderItemDTO { Type = Dto.SubmitOrder.MicroserviceRequests.OrderItemTypeDTO.Mailing.ToString() }
            });
            orderViewClient.Setup(o => o.GetOrderByOrderId(orderId))
                .Returns(Task.FromResult(orderResponse));
            var sut = autoMocker.CreateInstance<OrderDetailService>();

            // Act
            var result = await sut.GetOrderDetail(orderId).ConfigureAwait(false);

            // Assert
            Assert.NotEqual(result.CommonInfo.OrderDate.Value, DateTime.MinValue);
            Assert.Null(result.CommonInfo.ShippingDate.Value);
        }
 
    }
}