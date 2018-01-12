using AutoMapper;
using Kadena.Dto.General;
using Kadena.Dto.ViewOrder.MicroserviceResponses;
using Kadena.Models;
using Kadena.Models.Site;
using Kadena.WebAPI;
using Kadena.BusinessLogic.Contracts;
using Kadena.WebAPI.KenticoProviders.Contracts;
using Kadena.BusinessLogic.Services;
using Kadena2.MicroserviceClients.Contracts;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Threading.Tasks;
using Xunit;
using Kadena2.WebAPI.KenticoProviders.Contracts;
using Kadena2.WebAPI.KenticoProviders.Contracts.KadenaSettings;

namespace Kadena.Tests.WebApi
{
    public class OrderServiceTests
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

        // TODO Refactor to use different setups
        private OrderService CreateOrderService(Mock<IKenticoLogger> kenticoLogger = null,
                                                Mock<IOrderViewClient> orderViewClient = null)
        {
            MapperBuilder.InitializeAll();
            var mapper = Mapper.Instance;
            var kenticoUsers = new Mock<IKenticoUserProvider>();
            var kenticoPermissions = new Mock<IKenticoPermissionsProvider>();
            kenticoPermissions.Setup(p => p.UserCanSeeAllOrders())
                .Returns(false);
            kenticoUsers.Setup(p => p.GetCurrentCustomer())
               .Returns(new Customer() { Id = 10, UserID = 16 });

            var kenticoResource = new Mock<IKenticoResourceService>();
            var kenticoSite = new Mock<IKenticoSiteProvider>();
            kenticoSite.Setup(p => p.GetKenticoSite())
                .Returns(new KenticoSite());

            var kenticoOrder = new Mock<IKenticoOrderProvider>();
            var shoppingCart = new Mock<IShoppingCartProvider>();
            var productsProvider = new Mock<IKenticoProductsProvider>();
            var orderSubmitClient = new Mock<IOrderSubmitClient>();
            var taxCalculator = new Mock<ITaxEstimationService>();
            var mailingListClient = new Mock<IMailingListClient>();
            var templateProductService = new Mock<ITemplatedClient>();
            var documents = new Mock<IKenticoDocumentProvider>();
            var localization = new Mock<IKenticoLocalizationProvider>();
            var permissions = new Mock<IKenticoPermissionsProvider>();
            var settings = new Mock<IKadenaSettings>();
            var businessUnits = new Mock<IKenticoBusinessUnitsProvider>();

            return new OrderService(mapper,
                orderSubmitClient.Object,
                orderViewClient?.Object ?? new Mock<IOrderViewClient>().Object,
                mailingListClient.Object,
                kenticoOrder.Object,
                shoppingCart.Object,
                productsProvider.Object,
                kenticoUsers.Object,
                kenticoResource.Object,
                kenticoLogger?.Object ?? new Mock<IKenticoLogger>().Object,
                taxCalculator.Object,
                templateProductService.Object,
                documents.Object,
                localization.Object,
                permissions.Object,
                kenticoSite.Object,
                settings.Object,
                businessUnits.Object
            );
        }

        [Fact]
        public async Task OrderServiceTest_UserCanSee()
        {
            // Arrange
            var orderViewClient = new Mock<IOrderViewClient>();
            orderViewClient.Setup(o => o.GetOrderByOrderId("0010-0016-17-00006"))
                .Returns(Task.FromResult(CreateOrderDetailDtoOK()));
            var sut = CreateOrderService(orderViewClient: orderViewClient);

            // Act
            var result = await sut.GetOrderDetail("0010-0016-17-00006");

            //Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task OrderServiceTest_UserCannotSee()
        {
            // Arrange
            var orderViewClient = new Mock<IOrderViewClient>();
            orderViewClient.Setup(o => o.GetOrderByOrderId("0099-0099-17-00006"))
                .Returns(Task.FromResult(CreateOrderDetailDtoERROR()));
            var sut = CreateOrderService(orderViewClient: orderViewClient);

            // Act
            var result = sut.GetOrderDetail("0099-0099-17-00006");

            // Assert
            await Assert.ThrowsAsync<SecurityException>(async () => await result);
        }

        [Fact]
        public async Task OrderServiceTest_MicroserviceErrorLogged()
        {
            // Arrange
            var logger = new Mock<IKenticoLogger>();
            var orderViewClient = new Mock<IOrderViewClient>();
            orderViewClient.Setup(o => o.GetOrderByOrderId("0010-0016-66-00006"))
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
            var exception = await Assert.ThrowsAsync<ArgumentNullException>(async () => await result);
            Assert.Equal("orderId", exception.ParamName);
        }

        [Fact]
        public async Task OrderServiceTest_EmptyShippingAddressWhenMailingOnly()
        {
            // Arrange
            var orderId = "0010-0016-17-00006";
            var orderViewClient = new Mock<IOrderViewClient>();
            var orderResponse = CreateOrderDetailDtoOK(new[] 
            {
                new OrderItemDTO { Type = Dto.SubmitOrder.MicroserviceRequests.OrderItemTypeDTO.Mailing.ToString() }
            });
            orderViewClient.Setup(o => o.GetOrderByOrderId(orderId))
                .Returns(Task.FromResult(orderResponse));
            var sut = CreateOrderService(orderViewClient: orderViewClient);

            // Act
            var result = await sut.GetOrderDetail(orderId);

            // Assert
            Assert.Null(result.ShippingInfo.Address);
        }


        [Fact]
        public async Task NullDatetimeTests()
        {
            // Arrange
            var orderId = "0010-0016-17-00006";
            var orderViewClient = new Mock<IOrderViewClient>();
            var orderResponse = CreateOrderDetailDtoOK(new[]
            {
                new OrderItemDTO { Type = Dto.SubmitOrder.MicroserviceRequests.OrderItemTypeDTO.Mailing.ToString() }
            });
            orderViewClient.Setup(o => o.GetOrderByOrderId(orderId))
                .Returns(Task.FromResult(orderResponse));
            var sut = CreateOrderService(orderViewClient: orderViewClient);

            // Act
            var result = await sut.GetOrderDetail(orderId).ConfigureAwait(false);

            // Assert
            Assert.NotEqual(result.CommonInfo.OrderDate.Value, DateTime.MinValue);
            Assert.Null(result.CommonInfo.ShippingDate.Value);
        }
    }
}