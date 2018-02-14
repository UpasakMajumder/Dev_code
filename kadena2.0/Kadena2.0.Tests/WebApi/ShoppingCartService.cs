using System.Threading.Tasks;
using Kadena.BusinessLogic.Contracts;
using Kadena.WebAPI.KenticoProviders.Contracts;
using Kadena.BusinessLogic.Services;
using Moq;
using Xunit;
using Kadena.Models;
using Kadena.Models.Checkout;
using Kadena.BusinessLogic.Factories.Checkout;
using Kadena.Models.Product;
using Kadena2.WebAPI.KenticoProviders.Contracts;
using Kadena2.MicroserviceClients.Contracts;

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

        private ShoppingCartService CreateShoppingCartService(Mock<IKenticoLogger> kenticoLogger = null, Mock<IShoppingCartProvider> shoppingCart = null)
        {
            var kenticoUser = new Mock<IKenticoUserProvider>();
            kenticoUser.Setup(p => p.GetCustomerAddresses(AddressType.Shipping))
                .Returns(new[] { CreateDeliveryAddress() });
            kenticoUser.Setup(p => p.GetCurrentCustomer())
                .Returns(new Customer { DefaultShippingAddressId = 1, Id = 1});


            if(shoppingCart == null)
                shoppingCart = new Mock<IShoppingCartProvider>();
            shoppingCart.Setup(p => p.GetShippingCarriers())
                .Returns(new[] { CreateDeliveryCarrier() });
            shoppingCart.Setup(p => p.GetPaymentMethods())
                .Returns(new[] { CreatePaymentMethod() });
            shoppingCart.Setup(p => p.GetShoppingCartItems(It.IsAny<bool>()))
                .Returns(() => new[] { CreateCartitem(1)});
            shoppingCart.Setup(p => p.GetShoppingCartTotals())
                .Returns(() => GetShoppingCartTotals(100));
            shoppingCart.Setup(p => p.AddCartItem(It.IsAny<NewCartItem>(), null))
                .Returns(() => CreateCartitem(1));




            var kenticoSite = new Mock<IKenticoSiteProvider>();
            var kenticoLocalization = new Mock<IKenticoLocalizationProvider>();
            var kenticoPermissions = new Mock<IKenticoPermissionsProvider>();
            var kenticoResource = new Mock<IKenticoResourceService>();
            kenticoResource.Setup(m => m.GetResourceString("Kadena.Checkout.CountOfItems"))
                .Returns("You have {0} {1} in cart");
            kenticoResource.Setup(m => m.GetResourceString("Kadena.Checkout.ItemSingular"))
                .Returns("item");
            kenticoResource.Setup(m => m.GetSettingsKey("KDA_AddressDefaultCountry"))
                .Returns("0");
            var taxCalculator = new Mock<ITaxEstimationService>();
            var mailingService = new Mock<IKListService>();
            var documentsProvider = new Mock<IKenticoDocumentProvider>();
            var checkoutFactory = new CheckoutPageFactory(kenticoResource.Object, documentsProvider.Object, kenticoLocalization.Object);

            return new ShoppingCartService(
                kenticoSite.Object,
                kenticoLocalization.Object,
                kenticoPermissions.Object,
                kenticoUser.Object,
                kenticoResource.Object,
                taxCalculator.Object,
                mailingService.Object,
                new Mock<IUserDataServiceClient>().Object,
                shoppingCart.Object,
                checkoutFactory,
                new Mock<IKenticoLogger>().Object
                );
        }

        [Fact]
        public async Task GetCheckoutPageTest() 
        {
            // Arrange
            var sut = CreateShoppingCartService();

            // Act
            var result = await sut.GetCheckoutPage();

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.DeliveryAddresses);
            Assert.NotNull(result.EmailConfirmation);
            Assert.Null(result.EmptyCart);
            Assert.NotNull(result.PaymentMethods);
            Assert.NotNull(result.Submit);
            Assert.Matches("You have 1 item in cart", result.Products.Number);
            Assert.Single(result.Products.Items);
            Assert.Equal(10, result.Products.Items[0].TotalPrice);
        }

        [Fact]
        public void ChangeItemQuantityTest()
        {
            // Arrange
            var cartItems = new []{ CreateCartitem(1), CreateCartitem(2) };
            var mockShoppingCart = new Mock<IShoppingCartProvider>();
            mockShoppingCart.Setup(m => m.GetShoppingCartItems(true))
                .Returns(cartItems);
            mockShoppingCart.Setup(m => m.GetShoppingCartTotals())
                .Returns(new ShoppingCartTotals() { TotalItemsPrice = 1, TotalShipping = 2, TotalTax = 3});
            //mockShoppingCart.Setup(m => m.SetCartItemQuantity(1, 10))
              //  .Callback(() => cartItems.First(i => i.Id == 1).Quantity = 10);
            var sut = CreateShoppingCartService(null, mockShoppingCart);

            // Act
            var result = sut.ChangeItemQuantity(1, 100);

            // Assert
            Assert.NotNull(result);
            mockShoppingCart.Verify(m => m.SetCartItemQuantity(1,100), Times.Once);
        }

        [Fact]
        public void ItemPreview()
        {
            // Arrange
            var service = CreateShoppingCartService();

            // Act
            var result = service.ItemsPreview();

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Items);
            Assert.Single(result.Items);
        }

        [Fact]
        public async Task AddToCart()
        {
            // Arrange 
            var service = CreateShoppingCartService();

            // Act
            var result = await service.AddToCart(new NewCartItem());

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.CartPreview.Items);
            Assert.Single(result.CartPreview.Items);
        }
    }
}
