using System.Threading.Tasks;
using AutoMapper;
using Kadena.WebAPI;
using Kadena.BusinessLogic.Contracts;
using Kadena.WebAPI.KenticoProviders.Contracts;
using Kadena.BusinessLogic.Services;
using Kadena2.MicroserviceClients.Contracts;
using Moq;
using Xunit;
using Kadena.Models;
using Kadena.Models.Checkout;
using Kadena.BusinessLogic.Factories.Checkout;
using Kadena.Models.Product;
using System.Linq;

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
                Title = "Card",
                ClassName = "PurchaseOrder"
            };
        }

        private CartItem CreateCartitem(int id)
        {
            return new CartItem()
            {
                Id = id,
                CartItemText = $"Item{id}",
                ProductType = ProductTypes.StaticProduct,
                TotalPrice = 10,
                UnitPrice = 2,
                Quantity = 5
            };            
        }

        private ShoppingCartTotals GetShoppingCartTotals(decimal totalItemsPrice)
        {
            return new ShoppingCartTotals()
            {
                TotalItemsPrice = totalItemsPrice,
                TotalShipping = 19.99m,
                TotalTax = 0.2m * totalItemsPrice
            };
        }

        // TODO Refactor to use different setups
        private ShoppingCartService CreateShoppingCartService(Mock<IKenticoLogger> kenticoLogger = null, Mock<IKenticoProviderService> kenticoProvider = null )
        {
            MapperBuilder.InitializeAll();
            var mapper = Mapper.Instance;

            var kenticoUser = new Mock<IKenticoUserProvider>();
            kenticoUser.Setup(p => p.GetCustomerAddresses(AddressType.Shipping))
                .Returns(new[] { CreateDeliveryAddress() });

            if(kenticoProvider == null)
                kenticoProvider = new Mock<IKenticoProviderService>();            
            kenticoProvider.Setup(p => p.GetShippingCarriers())
                .Returns(new[] { CreateDeliveryCarrier() });
            kenticoProvider.Setup(p => p.GetPaymentMethods())
                .Returns(new[] { CreatePaymentMethod() });
            kenticoProvider.Setup(p => p.GetShoppingCartItems(true))
                .Returns(() => new[] { CreateCartitem(1), CreateCartitem(2) });
            kenticoProvider.Setup(p => p.GetShoppingCartTotals())
                .Returns(() => GetShoppingCartTotals(100));
            kenticoProvider.Setup(p => p.AddCartItem(It.IsAny<NewCartItem>(), null))
                .Returns(() => CreateCartitem(1));

            var kenticoResource = new Mock<IKenticoResourceService>();
            var orderSubmitClient = new Mock<IOrderSubmitClient>();
            var taxCalculator = new Mock<ITaxEstimationService>();
            var mailingService = new Mock<IKListService>();
            var checkoutFactory = new Mock<ICheckoutPageFactory>();
            var documents = new Mock<IKenticoDocumentProvider>();

            return new ShoppingCartService(mapper,
                kenticoProvider.Object,
                kenticoUser.Object,
                kenticoResource.Object,
                taxCalculator.Object,
                mailingService.Object,
                documents.Object,
                kenticoLogger?.Object ?? new Mock<IKenticoLogger>().Object,
                checkoutFactory.Object);
        }

        [Fact]
        public void GetCheckoutPageTest() //TODO not finished
        {
            // Arrange
            var sut = CreateShoppingCartService();
            //CreateCartitem();

            // Act
            var result = sut.GetCheckoutPage();

            // Assert
            Assert.NotNull(result);
            Assert.True(result.Products.Items.Count == 1);
            Assert.Equal(10, result.Products.Items[0].TotalPrice);
        }

        [Fact]
        public void ChangeItemQuantityTest() //TODO not finished
        {
            // Arrange
            var cartItems = new []{ CreateCartitem(1), CreateCartitem(2) };
            var mockProvider = new Mock<IKenticoProviderService>();
            mockProvider.Setup(m => m.GetShoppingCartItems(true))
                .Returns(cartItems);
            mockProvider.Setup(m => m.GetShoppingCartTotals())
                .Returns(new ShoppingCartTotals() { TotalItemsPrice = 1, TotalShipping = 2, TotalTax = 3});
            mockProvider.Setup(m => m.SetCartItemQuantity(1, 10))
                .Callback(() => cartItems.First(i => i.Id == 1).Quantity = 10);
            var sut = CreateShoppingCartService(null, mockProvider);

            // Act
            var result = sut.ChangeItemQuantity(1, 10);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void ItemPreview()
        {
            var service = CreateShoppingCartService();
            var result = service.ItemsPreview();

            Assert.NotNull(result);
            Assert.NotNull(result.Items);
            Assert.Equal(0, result.Items.Count);
        }

        [Fact]
        public async Task AddToCart()
        {
            var service = CreateShoppingCartService();
            var result = await service.AddToCart(new NewCartItem());

            Assert.NotNull(result);
            Assert.NotNull(result.CartPreview.Items);
            Assert.Equal(1, result.CartPreview.Items.Count);
        }
    }
}
