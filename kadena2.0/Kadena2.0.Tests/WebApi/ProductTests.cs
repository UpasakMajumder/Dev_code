using Kadena.WebAPI.KenticoProviders.Contracts;
using Kadena.BusinessLogic.Services;
using Moq;
using Xunit;
using System.Collections.Generic;
using Kadena.Models.Product;
using Kadena.BusinessLogic.Contracts;
using Moq.AutoMock;

namespace Kadena.Tests.WebApi
{
    public class ProductTests
    {
        const string borderStyleValue = "1px";

        private IProductsService CreateProductsService(bool borderOnSite, bool borderOnCategory)
        {
            var favorites = new Mock<IKenticoFavoritesProvider>();
            favorites.Setup(f => f.CheckFavoriteProductIds(new List<int>() { 1, 2 }))
                .Returns(new List<int>() { 2 });

            var products = new Mock<IKenticoProductsProvider>();
            products.Setup(p => p.GetProducts("/"))
                .Returns(new List<ProductLink>() { new ProductLink {Id = 1, Border = new Border { Exists = true, Value = borderStyleValue} },
                                                   new ProductLink {Id = 2, Border = new Border { Exists = false, Value = string.Empty} } });
            products.Setup(p => p.GetCategories("/"))
                .Returns(new List<ProductCategoryLink>() { new ProductCategoryLink { Id = 10, ProductBordersEnabled = true } ,
                                                           new ProductCategoryLink { Id = 11, ProductBordersEnabled = false } });
            products.Setup(p => p.GetCategory("/"))
                .Returns(new ProductCategoryLink { Id = 10, ProductBordersEnabled = borderOnCategory });

            var resources = new Mock<IKenticoResourceService>();
            resources.Setup(r => r.GetSiteSettingsKey<bool>("KDA_ProductThumbnailBorderEnabled"))
                .Returns(borderOnSite);
            resources.Setup(r => r.GetSiteSettingsKey("KDA_ProductThumbnailBorderStyle"))
                .Returns(borderStyleValue);

            return new ProductsService(products.Object, favorites.Object, resources.Object);
        }

        [Fact]
        public void GetProductsAndCategories_BasicTest() 
        {
            // Arrange
            var sut = CreateProductsService(true, true);

            // Act
            var result = sut.GetProducts("/");

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Products);
            Assert.NotNull(result.Categories);
            Assert.Equal(2, result.Products.Count);
            Assert.Equal(2, result.Categories.Count);
            Assert.Equal(1, result.Products[0].Id);
            Assert.Equal(2, result.Products[1].Id);
            Assert.Equal(10, result.Categories[0].Id);
            Assert.Equal(11, result.Categories[1].Id);
        }

        [Theory]
        [InlineData(true, true, true, borderStyleValue)]
        [InlineData(true, false, false, "")]
        [InlineData(false, false, false, "")]
        public void GetProductsAndCategories_BordersTest(bool borderOnSite, bool borderOnCategory, bool expectedResult, string style)
        {
            // Arrange
            var sut = CreateProductsService(borderOnSite, borderOnCategory);

            // Act
            var result = sut.GetProducts("/");

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Products);
            Assert.NotNull(result.Categories);
            Assert.Equal(2, result.Products.Count);
            Assert.Equal(2, result.Categories.Count);
            Assert.NotNull(result.Products[0].Border);
            Assert.NotNull(result.Products[1].Border);
            Assert.Equal(1, result.Products[0].Id);
            Assert.Equal(2, result.Products[1].Id);
            Assert.Equal(10, result.Categories[0].Id);
            Assert.Equal(11, result.Categories[1].Id);
            Assert.True(result.Products[0].Border.Exists == expectedResult);
            Assert.Equal(style, result.Products[0].Border.Value);
        }

        [Fact]
        public void FavoriteProductsTest()
        {
            // Arrange
            var sut = CreateProductsService(true, true);

            // Act
            var result = sut.GetProducts("/");

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Products);
            Assert.NotNull(result.Categories);
            Assert.Equal(2, result.Products.Count);
            Assert.Equal(2, result.Categories.Count);
            Assert.Equal(1, result.Products[0].Id);
            Assert.Equal(2, result.Products[1].Id);
            Assert.Equal(10, result.Categories[0].Id);
            Assert.Equal(11, result.Categories[1].Id);
            Assert.False(result.Products[0].IsFavourite);
            Assert.True(result.Products[1].IsFavourite);
        }

        [Fact]
        public void OrderingByOrderTest()
        {
            // Arrange
            var autoMocker = new AutoMocker();
            var productsMock = autoMocker.GetMock<IKenticoProductsProvider>();
            productsMock.Setup(p => p.GetProducts("/"))
                .Returns(new List<ProductLink> {
                    new ProductLink { Order = 3, Title = "p3" },
                    new ProductLink { Order = 1, Title = "p1" },
                    new ProductLink { Order = 2, Title = "p2" },
                });
            productsMock.Setup(p => p.GetCategories("/"))
                .Returns(new List<ProductCategoryLink> {
                    new ProductCategoryLink { Order = 3, Title = "c3" },
                    new ProductCategoryLink { Order = 1, Title = "c1" },
                    new ProductCategoryLink { Order = 2, Title = "c2" },
                });
            var sut = autoMocker.CreateInstance<ProductsService>();

            // Act
            var result = sut.GetProducts("/");

            // Assert
            Assert.True(result.Products.Count == 3);
            Assert.True(result.Categories.Count == 3);
            Assert.Equal("p1", result.Products[0].Title);
            Assert.Equal("p2", result.Products[1].Title);
            Assert.Equal("p3", result.Products[2].Title);
            Assert.Equal("c1", result.Categories[0].Title);
            Assert.Equal("c2", result.Categories[1].Title);
            Assert.Equal("c3", result.Categories[2].Title);
        }
    }
}
