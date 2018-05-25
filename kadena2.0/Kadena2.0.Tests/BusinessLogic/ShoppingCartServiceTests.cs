using Kadena.AmazonFileSystemProvider;
using Kadena.BusinessLogic.Contracts;
using Kadena.BusinessLogic.Factories.Checkout;
using Kadena.BusinessLogic.Services;
using Kadena.Models;
using Kadena.Models.Checkout;
using Kadena.Models.Membership;
using Kadena.Models.Product;
using Kadena.Models.SiteSettings;
using Kadena.WebAPI.KenticoProviders.Contracts;
using Kadena2.WebAPI.KenticoProviders.Contracts;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Kadena.Tests.BusinessLogic
{
    public class ShoppingCartServiceTests : KadenaUnitTest<ShoppingCartService>
    {
        string Name => "default name";
        string CustomName => "custom name";

        NewCartItem CreateNewCartItem(string customName = null)
        {
            return new NewCartItem
            {
                Quantity = 2,
                DocumentId = 1123,
                CustomProductName = customName
            };
        }

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

        private CheckoutCartItem CreateCartitem(int id)
        {
            return new CheckoutCartItem()
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

        private void SetupBase()
        {
            Setup<IKenticoCustomerProvider, Customer>(p => p.GetCurrentCustomer(), new Customer { DefaultShippingAddressId = 1, Id = 1 });
            Setup<IKenticoUserProvider, User>(p => p.GetCurrentUser(), new User { UserId = 1 });

            Setup<IKenticoAddressBookProvider, DeliveryAddress[]>(p => p.GetCustomerAddresses(AddressType.Shipping), new[] { CreateDeliveryAddress() });

            Setup<IShoppingCartProvider, DeliveryCarrier[]>(p => p.GetShippingCarriers(), new[] { CreateDeliveryCarrier() });
            Setup<IShoppingCartProvider, PaymentMethod[]>(p => p.GetPaymentMethods(), new[] { CreatePaymentMethod() });
            Setup<IShoppingCartProvider, ShoppingCartTotals>(p => p.GetShoppingCartTotals(), GetShoppingCartTotals(100));

            Setup<IShoppingCartItemsProvider, CheckoutCartItem[]>(p => p.GetCheckoutCartItems(It.IsAny<bool>()), new[] { CreateCartitem(1) });

            Setup<IKenticoResourceService, string>(m => m.GetResourceString("Kadena.Checkout.CountOfItems"), "You have {0} {1} in cart");
            Setup<IKenticoResourceService, string>(m => m.GetResourceString("Kadena.Checkout.ItemSingular"), "item");
            Setup<IKenticoResourceService, string>(m => m.GetSiteSettingsKey(Settings.KDA_AddressDefaultCountry), "0");

            Use<ICheckoutPageFactory, CheckoutPageFactory>();
        }

        [Theory(DisplayName = "ShoppingCartService.AddToCart() | Less than 1 item")]
        [InlineData(-1)]
        [InlineData(0)]
        public async Task AddToCart_CannotAddZero(int quantity)
        {
            // Act
            Task action() => Sut.AddToCart(new NewCartItem { Quantity = quantity });

            // Assert
            await Assert.ThrowsAsync<ArgumentException>(action);
        }

        [Fact(DisplayName = "ShoppingCartService.AddToCart() | Dynamic price")]
        public async Task AddToCart_DynamicPrice()
        {
            // Arrange 
            var newCartItem = CreateNewCartItem();
            var originalCartItemEntity = new CartItemEntity
            {
                ProductType = ProductTypes.StaticProduct,
                SKUUnits = 3,
                SKUID = 123
            };
            Setup<IShoppingCartItemsProvider, CartItemEntity>(ip => ip.GetOrCreateCartItem(newCartItem), originalCartItemEntity);
            Setup<IShoppingCartProvider, Sku>(p => p.GetSKU(123), new Sku { });
            Setup<IDynamicPriceRangeProvider, decimal>(dp => dp.GetDynamicPrice(It.IsAny<int>(), It.IsAny<int>()), 12.34m);

            // Act
            var result = await Sut.AddToCart(newCartItem);

            // Assert
            Assert.NotNull(result);
            Verify<IShoppingCartItemsProvider>(i => i.SaveCartItem(It.Is<CartItemEntity>(
                    e => e.CartItemPrice == 12.34m)
                ), Times.Once);
        }

        [Fact(DisplayName = "ShoppingCartService.AddToCart() | Static product")]
        public async Task AddToCart_StaticProduct()
        {
            // Arrange 
            var newCartItem = CreateNewCartItem();
            var originalCartItemEntity = new CartItemEntity
            {
                CartItemText = Name,
                ProductType = ProductTypes.StaticProduct,
                SKUUnits = 3,
                SKUID = 123
            };
            Setup<IShoppingCartItemsProvider, CartItemEntity>(ip => ip.GetOrCreateCartItem(newCartItem), originalCartItemEntity);
            Setup<IShoppingCartProvider, Sku>(p => p.GetSKU(123), new Sku { });

            // Act
            var result = await Sut.AddToCart(newCartItem);

            // Assert
            Assert.NotNull(result);
            VerifyNoOtherCalls<IKListService>();
            Verify<IShoppingCartItemsProvider>(i => i.SaveCartItem(It.Is<CartItemEntity>(
                    e => e.CartItemText == Name &&
                    e.SKUUnits == 5)
                ), Times.Once);
        }

        [Fact(DisplayName = "ShoppingCartService.AddToCart() | Templated product")]
        public async Task AddToCart_TemplatedProduct()
        {
            // Arrange             
            var newCartItem = CreateNewCartItem(CustomName);
            newCartItem.Quantity = 5;

            var originalCartItemEntity = new CartItemEntity
            {
                CartItemText = Name,
                ProductType = ProductTypes.TemplatedProduct,
                SKUUnits = 10,
                SKUID = 123
            };

            Setup<IShoppingCartProvider, Sku>(p => p.GetSKU(123), new Sku { });
            Setup<IShoppingCartItemsProvider, CartItemEntity>(ip => ip.GetOrCreateCartItem(newCartItem), originalCartItemEntity);

            // Act
            var result = await Sut.AddToCart(newCartItem);

            // Assert
            Assert.NotNull(result);
            VerifyNoOtherCalls<IKListService>();
            Verify<IShoppingCartItemsProvider>(i => i.SaveCartItem(It.Is<CartItemEntity>(
                    e => e.CartItemText == CustomName &&
                         e.SKUUnits == 5)
                ), Times.Once);
        }

        [Fact(DisplayName = "ShoppingCartService.AddToCart() | Inventory product")]
        public async Task AddToCart_InventoryProduct_OK()
        {
            // Arrange 
            var newCartItem = CreateNewCartItem();
            var originalCartItemEntity = new CartItemEntity
            {
                CartItemText = Name,
                ProductType = ProductTypes.InventoryProduct,
                SKUID = 6654,
                SKUUnits = 3
            };

            Setup<IShoppingCartItemsProvider, CartItemEntity>(ip => ip.GetOrCreateCartItem(newCartItem), originalCartItemEntity);
            Setup<IShoppingCartProvider, Sku>(cp => cp.GetSKU(originalCartItemEntity.SKUID), new Sku { AvailableItems = 100 });
            Setup<IShoppingCartProvider, Sku>(p => p.GetSKU(6654), new Sku { });

            // Act
            var result = await Sut.AddToCart(newCartItem);

            // Assert
            Assert.NotNull(result);
            VerifyNoOtherCalls<IKListService>();
            Verify<IShoppingCartItemsProvider>(i => i.SaveCartItem(It.Is<CartItemEntity>(
                    e => e.CartItemText == Name &&
                         e.SKUID == 6654 &&
                         e.SKUUnits == 5)
                ), Times.Once);
        }

        [Fact(DisplayName = "ShoppingCartService.AddToCart() | Inventory product out of stock")]
        public async Task AddToCart_InventoryProduct_OutOfStock()
        {
            // Arrange 
            var newCartItem = CreateNewCartItem();

            var originalCartItemEntity = new CartItemEntity
            {
                CartItemText = Name,
                ProductType = ProductTypes.InventoryProduct,
                SKUUnits = 3
            };

            Setup<IShoppingCartItemsProvider, CartItemEntity>(ip => ip.GetOrCreateCartItem(newCartItem), originalCartItemEntity);
            Setup<IShoppingCartProvider, Sku>(cp => cp.GetSKU(originalCartItemEntity.SKUID), new Sku { AvailableItems = 1, SellOnlyIfAvailable = true });

            // Act
            Task action() => Sut.AddToCart(newCartItem);

            // Assert
            await Assert.ThrowsAsync<ArgumentException>(action);
        }

        [Fact(DisplayName = "ShoppingCartService.AddToCart() | Mailing product")]
        public async Task AddToCart_MailingProduct()
        {
            // Arrange 
            const int quantity = 798;
            var containerId = new Guid("1a3b944e-3632-467b-a53a-206305310bae");
            var newCartItem = CreateNewCartItem();
            newCartItem.ContainerId = containerId;
            newCartItem.Quantity = quantity;
            var originalCartItemEntity = new CartItemEntity
            {
                CartItemText = Name,
                ProductType = ProductTypes.MailingProduct,
                SKUUnits = 3,
                SKUID = 123
            };

            Setup<IShoppingCartProvider, Sku>(p => p.GetSKU(123), new Sku { });
            Setup<IShoppingCartItemsProvider, CartItemEntity>(ip => ip.GetOrCreateCartItem(newCartItem), originalCartItemEntity);
            Setup<IKListService, Task<MailingList>>(m => m.GetMailingList(containerId)
                , Task.FromResult(new MailingList { AddressCount = quantity, Id = containerId.ToString() }));

            // Act
            var result = await Sut.AddToCart(newCartItem);

            // Assert
            Assert.NotNull(result);
            Verify<IKListService>(m => m.GetMailingList(containerId), Times.Once);
            Verify<IShoppingCartItemsProvider>(i => i.SaveCartItem(It.Is<CartItemEntity>(
                    e => e.CartItemText == Name &&
                         e.SKUUnits == quantity)
                ), Times.Once);
        }

        [Fact(DisplayName = "ShoppingCartService.GetCheckoutPage()")]
        public async Task GetCheckoutPageTest()
        {
            // Arrange
            SetupBase();

            // Act
            var result = await Sut.GetCheckoutPage();

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

        [Fact(DisplayName = "ShoppingCartService.ChangeItemQuantity()")]
        public void ChangeItemQuantityTest()
        {
            // Arrange
            SetupBase();
            var cartItems = new[] { CreateCartitem(1), CreateCartitem(2) };
            Setup<IShoppingCartProvider, ShoppingCartTotals>(m => m.GetShoppingCartTotals()
                , new ShoppingCartTotals() { TotalItemsPrice = 1, TotalShipping = 2, TotalTax = 3 });

            // Act
            var result = Sut.ChangeItemQuantity(1, 100);

            // Assert
            Assert.NotNull(result);
            Verify<IShoppingCartItemsProvider>(m => m.SetCartItemQuantity(1, 100), Times.Once);
        }

        [Theory(DisplayName = "ShoppingCartService.GetCartItems()")]
        [InlineData(true, false)]
        [InlineData(false, true)]
        public void SeePricesPermissions_Items(bool userHasPermissions, bool priceShouldBeEmpty)
        {
            // Arrange
            SetupBase();
            Setup<IKenticoPermissionsProvider, bool>(p => p.UserCanSeePrices(), userHasPermissions);

            // Act
            var result = Sut.GetCartItems();

            // Assert
            Assert.NotNull(result);
            Assert.Single(result.Items);
            Assert.Equal(string.IsNullOrEmpty(result.Items[0].PriceText), priceShouldBeEmpty);
            Assert.Equal(string.IsNullOrEmpty(result.SummaryPrice.Price), priceShouldBeEmpty);
        }

        [Fact(DisplayName = "ShoppingCartService.GetDeliveryAndTotals() | User can see prices")]
        public async Task CanSeePrices_DeliveryAndTotals()
        {
            // Arrange
            SetupBase();
            Setup<IKenticoPermissionsProvider, bool>(p => p.UserCanSeePrices(), true);

            // Act
            var result = await Sut.GetDeliveryAndTotals();

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.DeliveryMethods);
            Assert.NotNull(result.Totals);
            Assert.True(result.Totals.Items.TrueForAll(i => !string.IsNullOrEmpty(i.Value)));
        }

        [Fact(DisplayName = "ShoppingCartService.GetDeliveryAndTotals() | User can't see prices")]
        public async Task CannotSeePrices_DeliveryAndTotals()
        {
            // Arrange
            SetupBase();
            Setup<IKenticoPermissionsProvider, bool>(p => p.UserCanSeePrices(), false);

            // Act
            var result = await Sut.GetDeliveryAndTotals();

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.DeliveryMethods);
            Assert.NotNull(result.Totals);
            Assert.Null(result.Totals.Items);
        }

        [Fact(DisplayName = "ShoppingCartService.ItemsPreview()")]
        public void ItemPreview()
        {
            // Arrange
            SetupBase();

            // Act
            var result = Sut.ItemsPreview();

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Items);
            Assert.Single(result.Items);
        }

        [Fact(DisplayName = "ShoppingCartService.AddToCart() | Inventory product out of stock without verifying availability")]
        public async Task AddToCart_InventoryProduct_OutOfStock_DontCheckForAvailability()
        {
            // Arrange 
            var newCartItem = CreateNewCartItem();
            var originalCartItemEntity = new CartItemEntity
            {
                CartItemText = Name,
                ProductType = ProductTypes.InventoryProduct,
                SKUUnits = 3
            };

            Setup<IShoppingCartItemsProvider, CartItemEntity>(ip => ip.GetOrCreateCartItem(newCartItem), originalCartItemEntity);
            Setup<IShoppingCartProvider, Sku>(cp => cp.GetSKU(originalCartItemEntity.SKUID), new Sku { AvailableItems = 1, SellOnlyIfAvailable = false });

            // Act
            var result = await Sut.AddToCart(newCartItem);

            // Assert
            Assert.NotNull(result);
        }
    }
}
