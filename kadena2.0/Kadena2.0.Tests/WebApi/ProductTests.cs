using Kadena.WebAPI.KenticoProviders.Contracts;
using Kadena.BusinessLogic.Services;
using Moq;
using Xunit;
using System.Collections.Generic;
using Kadena.Models.Product;
using Kadena.BusinessLogic.Contracts;

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
            resources.Setup(r => r.GetSettingsKey("KDA_ProductThumbnailBorderEnabled"))
                .Returns(borderOnSite.ToString());
            resources.Setup(r => r.GetSettingsKey("KDA_ProductThumbnailBorderStyle"))
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
        }
    }
}
