using System.Threading.Tasks;
using AutoMapper;
using Kadena.WebAPI.Contracts;
using Kadena.WebAPI.Services;
using Kadena2.MicroserviceClients.Contracts;
using Moq;
using Xunit;
using System;
using Kadena.Models;
using Kadena.Dto.General;
using Kadena.Dto.ViewOrder.MicroserviceResponses;
using System.Security;
using Kadena.WebAPI.KenticoProviders.Contracts;
using Kadena.WebAPI;

namespace Kadena.Tests.WebApi
{
    
    public class OrderServiceTests
    {
        private BaseResponseDto<GetOrderByOrderIdResponseDTO> CreateOrderDetailDtoOK()
        {
            return new BaseResponseDto<GetOrderByOrderIdResponseDTO>()
            {
                Success = true,
                Payload = new GetOrderByOrderIdResponseDTO()
                {
                    Id = "1",
                    Items = new System.Collections.Generic.List<OrderItemDTO>(),
                    PaymentInfo = new PaymentInfoDTO(),
                    ShippingInfo = new ShippingInfoDTO(),
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

        // TODO Refactor to use different setups
        private OrderService CreateOrderService(Mock<IKenticoLogger> kenticoLogger = null,
                                                Mock<IOrderViewClient> orderViewClient = null)
        {
            MapperBuilder.InitializeAll();
            var mapper = Mapper.Instance;

            var kenticoProvider = new Mock<IKenticoProviderService>();
            kenticoProvider.Setup(p => p.UserCanSeeAllOrders())
                .Returns(false);
            kenticoProvider.Setup(p => p.GetCurrentCustomer())
               .Returns(new Customer() { Id = 10, UserID = 16});

            var kenticoResource = new Mock<IKenticoResourceService>();
            kenticoResource.Setup(p => p.GetKenticoSite())
                .Returns(new KenticoSite());

            var orderSubmitClient = new Mock<IOrderSubmitClient>();
            var taxCalculator = new Mock<ITaxEstimationService>();
            var mailingListClient = new Mock<IMailingListClient>();
            var templateProductService = new Mock<ITemplatedProductService>();

            return new OrderService(mapper,
                orderSubmitClient.Object,
                orderViewClient?.Object ?? new Mock<IOrderViewClient>().Object,
                mailingListClient.Object,
                kenticoProvider.Object,
                kenticoResource.Object,
                kenticoLogger?.Object ?? new Mock<IKenticoLogger>().Object,
                taxCalculator.Object);
        }

        [Fact]
        public async Task OrderServiceTest_UserCanSee()
        {
            // Arrange
            var orderViewClient = new Mock<IOrderViewClient>();
            orderViewClient.Setup(o => o.GetOrderByOrderId(null, "0010-0016-17-00006"))
                .Returns(Task.FromResult(CreateOrderDetailDtoOK()));
            var sut = CreateOrderService(orderViewClient: orderViewClient);

            // Act
            var result = await sut.GetOrderDetail("0010-0016-17-00006");

            //Assert
            Assert.Equal("Shipped", result.CommonInfo.Status);
        }


        [Fact]
        public async Task OrderServiceTest_UserCannotSee()
        {
            // Arrange
            var orderViewClient = new Mock<IOrderViewClient>();
            orderViewClient.Setup(o => o.GetOrderByOrderId(null, "0099-0099-17-00006"))
                .Returns(Task.FromResult(CreateOrderDetailDtoERROR()));
            var sut = CreateOrderService(orderViewClient: orderViewClient);

            // Act
            var result = sut.GetOrderDetail("0099-0099-17-00006");

            // Assert
            await Assert.ThrowsAsync<SecurityException>( async () => await result );
        }


        [Fact]
        public async Task OrderServiceTest_MicroserviceErrorLogged()
        {
            // Arrange
            var logger = new Mock<IKenticoLogger>();
            var orderViewClient = new Mock<IOrderViewClient>();
            orderViewClient.Setup(o => o.GetOrderByOrderId(null, "0010-0016-66-00006"))
                .Returns(Task.FromResult(CreateOrderDetailDtoERROR()));
            var sut = CreateOrderService(logger, orderViewClient);

            // Act
            var result = sut.GetOrderDetail("0010-0016-66-00006");

            // Assert
            var exception = await Assert.ThrowsAsync<Exception>(async () => await result);
            Assert.Contains("Failed to obtain order detail", exception.Message);
            logger.Verify(l => l.LogError("GetOrderDetail", ""), Times.Exactly(1));
        }


        [Theory]
        [InlineData("123")]
        [InlineData("asdgfdsrfgsdfg")]
        public async Task OrderServiceTest_BadFormatOrderId(string orderId)
        {
            // Arrange
            var sut = CreateOrderService();

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
            var sut = CreateOrderService();

            // Act
            var result = sut.GetOrderDetail(orderId);

            // Assert
            var exception = await Assert.ThrowsAsync <ArgumentNullException>(async () => await result);
            Assert.Equal("orderId", exception.ParamName);
        }

    }
}
