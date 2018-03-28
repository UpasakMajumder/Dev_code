using Kadena.BusinessLogic.Contracts;
using Kadena.BusinessLogic.Services;
using Kadena.Models;
using Kadena.Models.Checkout;
using Kadena.Models.Product;
using Kadena.WebAPI.KenticoProviders.Contracts;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Kadena.Tests.BusinessLogic
{
    public class AddToCartTests : KadenaUnitTest<ShoppingCartService>
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

        [Theory(DisplayName = "ShoppingCartService.AddToCart() | Less than 1 item")]
        [InlineData(-1)]
        [InlineData(0)]
        public void AddToCart_CannotAddZero(int quantity)
        {
            // Act
            Task action() => Sut.AddToCart(new NewCartItem { Quantity = quantity });

            // Assert
            Assert.ThrowsAsync<ArgumentException>(action);
        }

        [Fact(DisplayName = "ShoppingCartService.AddToCart() | Dynamic price")]
        public async Task AddToCart_DynamicPrice()
        {
            // Arrange 
            var newCartItem = CreateNewCartItem();
            var originalCartItemEntity = new CartItemEntity
            {
                ProductType = ProductTypes.StaticProduct,
                SKUUnits = 3
            };
            Setup<IShoppingCartItemsProvider, CartItemEntity>(ip => ip.GetOrCreateCartItem(newCartItem), originalCartItemEntity);
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
                SKUUnits = 3
            };
            Setup<IShoppingCartItemsProvider, CartItemEntity>(ip => ip.GetOrCreateCartItem(newCartItem), originalCartItemEntity);

            // Act
            var result = await Sut.AddToCart(newCartItem);

            // Assert
            Assert.NotNull(result);
            VerifyNoOtherCalls<IKListService>();
            Verify<IShoppingCartItemsProvider>(ip => ip.SetArtwork(It.IsAny<CartItemEntity>(), 1123), Times.Once);
            Verify<IShoppingCartItemsProvider>(i => i.SaveCartItem(It.Is<CartItemEntity>(
                    e => e.CartItemText == Name &&
                    e.SKUUnits == 3)
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
                SKUUnits = 10
            };

            Setup<IShoppingCartItemsProvider, CartItemEntity>(ip => ip.GetOrCreateCartItem(newCartItem), originalCartItemEntity);

            // Act
            var result = await Sut.AddToCart(newCartItem);

            // Assert
            Assert.NotNull(result);
            VerifyNoOtherCalls<IKListService>();
            Verify<IShoppingCartItemsProvider>(ip => ip.SetArtwork(It.IsAny<CartItemEntity>(), 1123), Times.Once);
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
            Setup<IShoppingCartProvider, int>(cp => cp.GetStockQuantity(originalCartItemEntity.SKUID), 100);

            // Act
            var result = await Sut.AddToCart(newCartItem);

            // Assert
            Assert.NotNull(result);
            VerifyNoOtherCalls<IKListService>();
            Verify<IShoppingCartItemsProvider>(ip => ip.SetArtwork(It.IsAny<CartItemEntity>(), 1123), Times.Once);
            Verify<IShoppingCartItemsProvider>(i => i.SaveCartItem(It.Is<CartItemEntity>(
                    e => e.CartItemText == Name &&
                         e.SKUID == 6654 &&
                         e.SKUUnits == 3)
                ), Times.Once);
        }

        [Fact(DisplayName = "ShoppingCartService.AddToCart() | Inventory product out of stock")]
        public void AddToCart_InventoryProduct_OutOfStock()
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
            Setup<IShoppingCartProvider, int>(cp => cp.GetStockQuantity(originalCartItemEntity.SKUID), 1);

            // Act
            Task action() => Sut.AddToCart(newCartItem);

            // Assert
            Assert.ThrowsAsync<ArgumentException>(action);
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
                SKUUnits = 3
            };

            Setup<IShoppingCartItemsProvider, CartItemEntity>(ip => ip.GetOrCreateCartItem(newCartItem), originalCartItemEntity);
            Setup<IKListService, Task<MailingList>>(m => m.GetMailingList(containerId)
                , Task.FromResult(new MailingList { AddressCount = quantity, Id = containerId.ToString() }));

            // Act
            var result = await Sut.AddToCart(newCartItem);

            // Assert
            Assert.NotNull(result);
            Verify<IKListService>(m => m.GetMailingList(containerId), Times.Once);
            Verify<IShoppingCartItemsProvider>(ip => ip.SetArtwork(It.IsAny<CartItemEntity>(), 1123), Times.Once);
            Verify<IShoppingCartItemsProvider>(i => i.SaveCartItem(It.Is<CartItemEntity>(
                    e => e.CartItemText == Name &&
                         e.SKUUnits == quantity)
                ), Times.Once);
        }
    }
}
