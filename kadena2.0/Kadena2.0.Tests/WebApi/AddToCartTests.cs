using Kadena.BusinessLogic.Contracts;
using Kadena.BusinessLogic.Services;
using Kadena.Models.Checkout;
using Kadena.Models.Product;
using Kadena.WebAPI.KenticoProviders.Contracts;
using Moq;
using Moq.AutoMock;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Kadena.Tests.WebApi
{
    public class AddToCartTests
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

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        public async Task AddToCart_CannotAddZero(int quantity)
        {
            // Arrange 
            var autoMocker = new AutoMocker();
            var sut = autoMocker.CreateInstance<ShoppingCartService>();

            // Act
            var result = sut.AddToCart(new NewCartItem { Quantity = quantity });

            // Assert
            await Assert.ThrowsAsync<ArgumentException>(async () => await result);
        }

        [Fact]
        public async Task AddToCart_DynamicPrice()
        {
            // Arrange 
            var newCartItem = CreateNewCartItem();

            var originalCartItemEntity = new CartItemEntity
            {
                ProductType = ProductTypes.StaticProduct,
                SKUUnits = 3
            };

            var autoMocker = new AutoMocker();
            var itemsProvider = autoMocker.GetMock<IShoppingCartItemsProvider>();
            itemsProvider.Setup(ip => ip.GetOrCreateCartItem(newCartItem))
                .Returns(originalCartItemEntity);
            autoMocker.GetMock<IDynamicPriceRangeProvider>()
                .Setup(dp => dp.GetDynamicPrice(It.IsAny<int>(), It.IsAny<int>()))
                .Returns(12.34m);
            
            var sut = autoMocker.CreateInstance<ShoppingCartService>();

            // Act
            var result = await sut.AddToCart(newCartItem);

            // Assert
            Assert.NotNull(result);
            itemsProvider.Verify(i => i.SaveCartItem(It.Is<CartItemEntity>(
                    e => e.CartItemPrice == 12.34m)
                ), Times.Once);
        }

        [Fact]
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

            var autoMocker = new AutoMocker();
            var itemsProvider = autoMocker.GetMock<IShoppingCartItemsProvider>();
            itemsProvider.Setup(ip => ip.GetOrCreateCartItem(newCartItem))
                .Returns(originalCartItemEntity);
            
            var sut = autoMocker.CreateInstance<ShoppingCartService>();

            // Act
            var result = await sut.AddToCart(newCartItem);

            // Assert
            Assert.NotNull(result);
            autoMocker.GetMock<IKListService>().VerifyNoOtherCalls();
            autoMocker.GetMock<IShoppingCartItemsProvider>().Verify(ip => ip.SetArtwork(It.IsAny<CartItemEntity>(), 1123), Times.Once);
            itemsProvider.Verify(i => i.SaveCartItem(It.Is<CartItemEntity>(
                    e => e.CartItemText == Name &&
                    e.SKUUnits == 3)
                ), Times.Once);
        }

        [Fact]
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

            var autoMocker = new AutoMocker();
            var itemsProvider = autoMocker.GetMock<IShoppingCartItemsProvider>();
            itemsProvider.Setup(ip => ip.GetOrCreateCartItem(newCartItem))
                .Returns(originalCartItemEntity);
            
            var sut = autoMocker.CreateInstance<ShoppingCartService>();

            // Act
            var result = await sut.AddToCart(newCartItem);

            // Assert
            Assert.NotNull(result);
            autoMocker.GetMock<IKListService>().VerifyNoOtherCalls();
            autoMocker.GetMock<IShoppingCartItemsProvider>().Verify(ip => ip.SetArtwork(It.IsAny<CartItemEntity>(), 1123), Times.Once);
            itemsProvider.Verify(i => i.SaveCartItem(It.Is<CartItemEntity>(
                    e => e.CartItemText == CustomName &&
                         e.SKUUnits == 5)
                ), Times.Once);
        }

        [Fact]
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

            var autoMocker = new AutoMocker();
            var itemsProvider = autoMocker.GetMock<IShoppingCartItemsProvider>();
            itemsProvider.Setup(ip => ip.GetOrCreateCartItem(newCartItem))
                .Returns(originalCartItemEntity);
            var cartProvider = autoMocker.GetMock<IShoppingCartProvider>();
            cartProvider.Setup(cp => cp.GetSKU(originalCartItemEntity.SKUID))
                .Returns(new Sku { AvailableItems = 100 });
            
            var sut = autoMocker.CreateInstance<ShoppingCartService>();

            // Act
            var result = await sut.AddToCart(newCartItem);

            // Assert
            Assert.NotNull(result);
            autoMocker.GetMock<IKListService>().VerifyNoOtherCalls();
            autoMocker.GetMock<IShoppingCartItemsProvider>().Verify(ip => ip.SetArtwork(It.IsAny<CartItemEntity>(), 1123), Times.Once);
            itemsProvider.Verify(i => i.SaveCartItem(It.Is<CartItemEntity>(
                    e => e.CartItemText == Name &&
                         e.SKUID == 6654 &&
                         e.SKUUnits == 3)
                ), Times.Once);
        }

        [Fact]
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

            var autoMocker = new AutoMocker();
            var itemsProvider = autoMocker.GetMock<IShoppingCartItemsProvider>();
            itemsProvider.Setup(ip => ip.GetOrCreateCartItem(newCartItem))
                .Returns(originalCartItemEntity);
            var cartProvider = autoMocker.GetMock<IShoppingCartProvider>();
            cartProvider.Setup(cp => cp.GetSKU(originalCartItemEntity.SKUID))
                .Returns(new Sku { AvailableItems = 1, SellOnlyIfAvailable = true});

            var sut = autoMocker.CreateInstance<ShoppingCartService>();

            // Act
            var result = sut.AddToCart(newCartItem);

            // Assert
            await Assert.ThrowsAsync<ArgumentException>(async () => await result);
        }

        [Fact]
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

            var autoMocker = new AutoMocker();
            var itemsProvider = autoMocker.GetMock<IShoppingCartItemsProvider>();
            itemsProvider.Setup(ip => ip.GetOrCreateCartItem(newCartItem))
                .Returns(originalCartItemEntity);
            var cartProvider = autoMocker.GetMock<IShoppingCartProvider>();
            cartProvider.Setup(cp => cp.GetSKU(originalCartItemEntity.SKUID))
                .Returns(new Sku { AvailableItems = 1, SellOnlyIfAvailable = false });

            var sut = autoMocker.CreateInstance<ShoppingCartService>();

            // Act
            var result = await sut.AddToCart(newCartItem);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
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

            var autoMocker = new AutoMocker();
            var itemsProvider = autoMocker.GetMock<IShoppingCartItemsProvider>();
            itemsProvider.Setup(ip => ip.GetOrCreateCartItem(newCartItem))
                .Returns(originalCartItemEntity);
            var klistClientMock = autoMocker.GetMock<IKListService>();
            klistClientMock.Setup(m => m.GetMailingList(containerId))
                .Returns( Task.FromResult(new Kadena.Models.MailingList { AddressCount = quantity, Id = containerId.ToString()}));

            var sut = autoMocker.CreateInstance<ShoppingCartService>();

            // Act
            var result = await sut.AddToCart(newCartItem);

            // Assert
            Assert.NotNull(result);
            klistClientMock.Verify(m => m.GetMailingList(containerId), Times.Once);
            autoMocker.GetMock<IShoppingCartItemsProvider>().Verify(ip => ip.SetArtwork(It.IsAny<CartItemEntity>(), 1123), Times.Once);
            itemsProvider.Verify(i => i.SaveCartItem(It.Is<CartItemEntity>(
                    e => e.CartItemText == Name &&
                         e.SKUUnits == quantity)
                ), Times.Once);
        }
    }
}
