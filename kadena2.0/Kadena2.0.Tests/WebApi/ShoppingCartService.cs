using System.Threading.Tasks;
using AutoMapper;
using Kadena.WebAPI;
using Kadena.WebAPI.Contracts;
using Kadena.WebAPI.KenticoProviders.Contracts;
using Kadena.WebAPI.Services;
using Kadena2.MicroserviceClients.Contracts;
using Moq;
using Xunit;
using Kadena.Models;
using Kadena.Models.Checkout;

namespace Kadena.Tests.WebApi
{
    
    public class ShoppingCartServiceTests
    {
        private DeliveryAddress CreateDeliveryAddress()
        {
            return new DeliveryAddress()
            {
                City = "Prague"
            };
        }

        private DeliveryCarrier CreateDeliveryCarrier()
        {
            return new DeliveryCarrier()
            {
                Title = "CeskaPosta"
            };
        }

        private PaymentMethod CreatePaymentMethod()
        {
            return new PaymentMethod()
            {
                Title = "Card"
            };
        }

        private CartItem CreateCartitem()
        {
            return new CartItem()
            {
                Id = 1,
                CartItemText = "Item1"
            };
        }

        // TODO Refactor to use different setups
        private ShoppingCartService CreateShoppingCartService(Mock<IKenticoLogger> kenticoLogger = null)
        {
            MapperBuilder.InitializeAll();
            var mapper = Mapper.Instance;

            var kenticoProvider = new Mock<IKenticoProviderService>();
            kenticoProvider.Setup(p => p.GetCustomerAddresses("Shipping"))
                .Returns(new[] { CreateDeliveryAddress() });
            kenticoProvider.Setup(p => p.GetShippingCarriers())
                .Returns(new[] { CreateDeliveryCarrier() });
            kenticoProvider.Setup(p => p.GetPaymentMethods())
                .Returns(new[] { CreatePaymentMethod() });
            kenticoProvider.Setup(p => p.GetShoppingCartItems())
                .Returns(new[] { CreateCartitem() });


            var kenticoResource = new Mock<IKenticoResourceService>();
            var orderSubmitClient = new Mock<IOrderSubmitClient>();
            var taxCalculator = new Mock<ITaxEstimationService>();
            var templateClient = new Mock<ITemplatedProductService>();
            var templateProductService = new Mock<ITemplatedProductService>();

            return new ShoppingCartService(mapper,
                kenticoProvider.Object,
                kenticoResource.Object,
                taxCalculator.Object,
                templateClient.Object,
                kenticoLogger?.Object ?? new Mock<IKenticoLogger>().Object);
        }

        [Fact]
        public async Task GetCheckoutPageTest() //TODO not finished
        {
            // Arrange
            var sut = CreateShoppingCartService();

            // Act
            var result = await sut.GetCheckoutPage(); 

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task ChangeItemQuantityTest() //TODO not finished
        {
            // Arrange
            var sut = CreateShoppingCartService();

            // Act
            await sut.ChangeItemQuantity(1, 10);

            // Assert
        }

    }
}
