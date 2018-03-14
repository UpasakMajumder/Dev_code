using System.Threading.Tasks;
using Kadena.WebAPI.KenticoProviders.Contracts;
using Kadena.BusinessLogic.Services;
using Moq;
using Xunit;
using Kadena.Models;
using Kadena.Models.Checkout;
using Kadena.BusinessLogic.Factories.Checkout;
using Kadena.Models.Product;
using Kadena2.WebAPI.KenticoProviders.Contracts;
using Moq.AutoMock;
using System;
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
                Quantity = 5,
                PriceText = "10"
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

        private ShoppingCartService CreateShoppingCartService(AutoMocker autoMocker)
        {            
            var kenticoUser = autoMocker.GetMock<IKenticoUserProvider>();
            kenticoUser.Setup(p => p.GetCurrentCustomer())
                .Returns(new Customer { DefaultShippingAddressId = 1, Id = 1});
            kenticoUser.Setup(p => p.GetCurrentUser())
                .Returns(new User { UserId = 1 });

            var kenticoAddresses = autoMocker.GetMock<IKenticoAddressBookProvider>();
            kenticoAddresses.Setup(p => p.GetCustomerAddresses(AddressType.Shipping))
                .Returns(new[] { CreateDeliveryAddress() });

            var shoppingCart = autoMocker.GetMock<IShoppingCartProvider>();
            shoppingCart.Setup(p => p.GetShippingCarriers())
                .Returns(new[] { CreateDeliveryCarrier() });
            shoppingCart.Setup(p => p.GetPaymentMethods())
                .Returns(new[] { CreatePaymentMethod() });
            shoppingCart.Setup(p => p.GetShoppingCartTotals())
                .Returns(() => GetShoppingCartTotals(100));

            var shoppingCartItems = autoMocker.GetMock<IShoppingCartItemsProvider>();
            shoppingCartItems.Setup(p => p.GetShoppingCartItems(It.IsAny<bool>()))
                .Returns(() => new[] { CreateCartitem(1) });

            var kenticoResource = autoMocker.GetMock<IKenticoResourceService>();
            kenticoResource.Setup(m => m.GetResourceString("Kadena.Checkout.CountOfItems"))
                .Returns("You have {0} {1} in cart");
            kenticoResource.Setup(m => m.GetResourceString("Kadena.Checkout.ItemSingular"))
                .Returns("item");
            kenticoResource.Setup(m => m.GetSettingsKey("KDA_AddressDefaultCountry"))
                .Returns("0");

            var checkoutFactory = autoMocker.CreateInstance<CheckoutPageFactory>();

            autoMocker.Use<ICheckoutPageFactory>(checkoutFactory);

            return autoMocker.CreateInstance<ShoppingCartService>();
        }

        [Fact]
        public async Task GetCheckoutPageTest() 
        {
            // Arrange
            var autoMocker = new AutoMocker();

            var sut = CreateShoppingCartService(autoMocker);

            // Act
            var result = await sut.GetCheckoutPage();

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.DeliveryAddresses);
            Assert.NotNull(result.EmailConfirmation);
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
            var autoMocker = new AutoMocker();
            var sut = CreateShoppingCartService(autoMocker);
            var cartItems = new []{ CreateCartitem(1), CreateCartitem(2) };
            var mockShoppingCart = autoMocker.GetMock<IShoppingCartProvider>();
            mockShoppingCart.Setup(m => m.GetShoppingCartTotals())
                .Returns(new ShoppingCartTotals() { TotalItemsPrice = 1, TotalShipping = 2, TotalTax = 3});
            var mockShoppingCartItems = autoMocker.GetMock<IShoppingCartItemsProvider>();

            // Act
            var result = sut.ChangeItemQuantity(1, 100);

            // Assert
            Assert.NotNull(result);
            mockShoppingCartItems.Verify(m => m.SetCartItemQuantity(1,100), Times.Once);
        }


        [Theory]
        [InlineData(true, false)]
        [InlineData(false, true)]
        public void SeePricesPermissions_Items(bool userHasPermissions, bool priceShouldBeEmpty)
        {
            // Arrange
            var autoMocker = new AutoMocker();
            var sut = CreateShoppingCartService(autoMocker);
            
            var permissionsMock = autoMocker.GetMock<IKenticoPermissionsProvider>();
            permissionsMock.Setup(p => p.UserCanSeePrices())
                .Returns(userHasPermissions);

            // Act
            var result = sut.GetCartItems();

            // Assert
            Assert.NotNull(result);
            Assert.Single(result.Items);
            Assert.Equal(string.IsNullOrEmpty(result.Items[0].PriceText), priceShouldBeEmpty);
            Assert.Equal(string.IsNullOrEmpty(result.SummaryPrice.Price), priceShouldBeEmpty);
        }

        [Fact]
        public async Task CanSeePrices_DeliveryAndTotals()
        {
            // Arrange
            var autoMocker = new AutoMocker();
            var sut = CreateShoppingCartService(autoMocker);
            var permissionsMock = autoMocker.GetMock<IKenticoPermissionsProvider>();
            permissionsMock.Setup(p => p.UserCanSeePrices())
                .Returns(true);

            // Act
            var result = await sut.GetDeliveryAndTotals();

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.DeliveryMethods);
            Assert.NotNull(result.Totals);
            Assert.True(result.Totals.Items.TrueForAll( i => !string.IsNullOrEmpty(i.Value)));
        }

        [Fact]
        public async Task CannotSeePrices_DeliveryAndTotals()
        {
            // Arrange
            var autoMocker = new AutoMocker();
            var sut = CreateShoppingCartService(autoMocker);
            var permissionsMock = autoMocker.GetMock<IKenticoPermissionsProvider>();
            permissionsMock.Setup(p => p.UserCanSeePrices())
                .Returns(false);

            // Act
            var result = await sut.GetDeliveryAndTotals();

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.DeliveryMethods);
            Assert.NotNull(result.Totals);
            Assert.Null(result.Totals.Items);
        }


        [Fact]
        public void ItemPreview()
        {
            // Arrange
            var autoMocker = new AutoMocker();
            var sut = CreateShoppingCartService(autoMocker);

            // Act
            var result = sut.ItemsPreview();

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Items);
            Assert.Single(result.Items);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        public async Task AddToCart_CannotAddZero(int quantity)
        {
            // Arrange 
            var autoMocker = new AutoMocker();
            var sut = CreateShoppingCartService(autoMocker);

            // Act
            var result = sut.AddToCart(new NewCartItem { Quantity = quantity });

            // Assert
            var exception = await Assert.ThrowsAsync<ArgumentException>(async () => await result);
        }

        [Fact]
        public async Task AddToCart_DynamicPrice()
        {
            // Arrange 
            var newCartItem = new NewCartItem
            {
                Quantity = 2,
                DocumentId = 1123
            };

            var originalCartItemEntity = new CartItemEntity
            {
                ProductType = "KDA.StaticProduct",
                SKUUnits = 3
            };

            var autoMocker = new AutoMocker();
            var itemsProvider = autoMocker.GetMock<IShoppingCartItemsProvider>();
            itemsProvider.Setup(ip => ip.GetOrCreateCartItem(newCartItem))
                .Returns(originalCartItemEntity);
            autoMocker.GetMock<IDynamicPriceRangeProvider>()
                .Setup(dp => dp.GetDynamicPrice(It.IsAny<int>(), It.IsAny<int>()))
                .Returns(12.34m);
            var sut = CreateShoppingCartService(autoMocker);

            // Act
            var result = await sut.AddToCart(newCartItem);

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.CartPreview.Items);
            Assert.Single(result.CartPreview.Items);
            itemsProvider.Verify(i => i.SaveCartItem(It.Is<CartItemEntity>(
                    e => e.CartItemPrice == 12.34m)
                ), Times.Once);
        }

        [Fact]
        public async Task AddToCart_StaticProduct()
        {
            // Arrange 
            const string name = "Some static product";

            var newCartItem = new NewCartItem
            {
                Quantity = 2,
                CustomProductName = name,
                DocumentId = 1123
            };

            var originalCartItemEntity = new CartItemEntity
            {
                CartItemText = name,
                ProductType = "KDA.StaticProduct",
                SKUUnits = 3
            };

            var autoMocker = new AutoMocker();
            var itemsProvider = autoMocker.GetMock<IShoppingCartItemsProvider>();
            itemsProvider.Setup(ip => ip.GetOrCreateCartItem(newCartItem))
                .Returns(originalCartItemEntity);
            var sut = CreateShoppingCartService(autoMocker);

            // Act
            var result = await sut.AddToCart(newCartItem);

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.CartPreview.Items);
            Assert.Single(result.CartPreview.Items);
            autoMocker.GetMock<IMailingListClient>().VerifyNoOtherCalls();
            itemsProvider.Verify(i => i.SaveCartItem( It.Is<CartItemEntity>(
                    e => e.CartItemText == name)
                ), Times.Once);
        }

        [Fact]
        public async Task AddToCart_TemplatedProduct()
        {
            // Arrange 
            const string name = "Templated default name";
            const string customName = "Templated custom name";

            var newCartItem = new NewCartItem
            {
                Quantity = 2,
                CustomProductName = customName,
                DocumentId = 1123
            };

            var originalCartItemEntity = new CartItemEntity
            {
                CartItemText = name,
                ProductType = "KDA.TemplatedProduct",
                SKUUnits = 3
            };

            var autoMocker = new AutoMocker();
            var itemsProvider = autoMocker.GetMock<IShoppingCartItemsProvider>();
            itemsProvider.Setup(ip => ip.GetOrCreateCartItem(newCartItem))
                .Returns(originalCartItemEntity);
            var sut = CreateShoppingCartService(autoMocker);

            // Act
            var result = await sut.AddToCart(newCartItem);

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.CartPreview.Items);
            Assert.Single(result.CartPreview.Items);
            autoMocker.GetMock<IMailingListClient>().VerifyNoOtherCalls();
            itemsProvider.Verify(i => i.SaveCartItem(It.Is<CartItemEntity>(
                    e => e.CartItemText == customName)
                ), Times.Once);
        }
    }
}
