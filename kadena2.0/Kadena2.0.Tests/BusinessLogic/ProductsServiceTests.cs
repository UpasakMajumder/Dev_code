using Kadena.WebAPI.KenticoProviders.Contracts;
using Kadena.BusinessLogic.Services;
using Xunit;
using System.Collections.Generic;
using Kadena.Models.Product;

namespace Kadena.Tests.BusinessLogic
{
    public class ProductsServiceTests : KadenaUnitTest<ProductsService>
    {
        const string borderStyleValue = "1px";

        private void SetupBase(bool borderOnSite, bool borderOnCategory)
        {
            Setup<IKenticoFavoritesProvider, List<int>>(f => f.CheckFavoriteProductIds(new List<int>() { 1, 2 }), new List<int>() { 2 });

            Setup<IKenticoProductsProvider, List<ProductLink>>(p => p.GetProducts("/")
                , new List<ProductLink>()
                {
                    new ProductLink {Id = 1, Border = new Border { Exists = true, Value = borderStyleValue} },
                    new ProductLink {Id = 2, Border = new Border { Exists = false, Value = string.Empty} }
                });
            Setup<IKenticoProductsProvider, List<ProductCategoryLink>>(p => p.GetCategories("/")
                , new List<ProductCategoryLink>()
                {
                    new ProductCategoryLink { Id = 10, ProductBordersEnabled = true } ,
                    new ProductCategoryLink { Id = 11, ProductBordersEnabled = false }
                });
            Setup<IKenticoProductsProvider, ProductCategoryLink>(p => p.GetCategory("/")
                , new ProductCategoryLink { Id = 10, ProductBordersEnabled = borderOnCategory });

            Setup<IKenticoResourceService, string>(r => r.GetSettingsKey("KDA_ProductThumbnailBorderEnabled"), borderOnSite.ToString());
            Setup<IKenticoResourceService, string>(r => r.GetSettingsKey("KDA_ProductThumbnailBorderStyle"), borderStyleValue);
        }

        [Fact(DisplayName = "ProductsService.GetProducts() | All borders turned on")]
        public void GetProductsAndCategories_BasicTest()
        {
            // Arrange
            SetupBase(true, true);

            // Act
            var actualResult = Sut.GetProducts("/");

            // Assert
            Assert.NotNull(actualResult);
            Assert.NotNull(actualResult.Products);
            Assert.NotNull(actualResult.Categories);
            Assert.Equal(2, actualResult.Products.Count);
            Assert.Equal(2, actualResult.Categories.Count);
            Assert.Equal(1, actualResult.Products[0].Id);
            Assert.Equal(2, actualResult.Products[1].Id);
            Assert.Equal(10, actualResult.Categories[0].Id);
            Assert.Equal(11, actualResult.Categories[1].Id);
        }

        [Theory(DisplayName = "ProductsService.GetProducts() | Combinations of border settings")]
        [InlineData(true, true, true, borderStyleValue)]
        [InlineData(true, false, false, "")]
        [InlineData(false, false, false, "")]
        public void GetProductsAndCategories_BordersTest(bool borderOnSite, bool borderOnCategory, bool expectedResult, string style)
        {
            // Arrange
            SetupBase(borderOnSite, borderOnCategory);

            // Act
            var actualResult = Sut.GetProducts("/");

            // Assert
            Assert.NotNull(actualResult);
            Assert.NotNull(actualResult.Products);
            Assert.NotNull(actualResult.Categories);
            Assert.Equal(2, actualResult.Products.Count);
            Assert.Equal(2, actualResult.Categories.Count);
            Assert.NotNull(actualResult.Products[0].Border);
            Assert.NotNull(actualResult.Products[1].Border);
            Assert.Equal(1, actualResult.Products[0].Id);
            Assert.Equal(2, actualResult.Products[1].Id);
            Assert.Equal(10, actualResult.Categories[0].Id);
            Assert.Equal(11, actualResult.Categories[1].Id);
            Assert.True(actualResult.Products[0].Border.Exists == expectedResult);
            Assert.Equal(style, actualResult.Products[0].Border.Value);
        }

        [Fact(DisplayName = "ProductsService.GetProducts() | Favorite products")]
        public void FavoriteProductsTest()
        {
            // Arrange
            SetupBase(true, true);

            // Act
            var actualResult = Sut.GetProducts("/");

            // Assert
            Assert.NotNull(actualResult);
            Assert.NotNull(actualResult.Products);
            Assert.NotNull(actualResult.Categories);
            Assert.Equal(2, actualResult.Products.Count);
            Assert.Equal(2, actualResult.Categories.Count);
            Assert.Equal(1, actualResult.Products[0].Id);
            Assert.Equal(2, actualResult.Products[1].Id);
            Assert.Equal(10, actualResult.Categories[0].Id);
            Assert.Equal(11, actualResult.Categories[1].Id);
            Assert.False(actualResult.Products[0].IsFavourite);
            Assert.True(actualResult.Products[1].IsFavourite);
        }

        [Fact(DisplayName = "ProductsService.GetProducts() | Ordering by order")]
        public void OrderingByOrderTest()
        {
            // Arrange
            Setup<IKenticoProductsProvider, List<ProductLink>>(p => p.GetProducts("/")
                , new List<ProductLink> {
                    new ProductLink { Order = 3, Title = "p3" },
                    new ProductLink { Order = 1, Title = "p1" },
                    new ProductLink { Order = 2, Title = "p2" },
                });
            Setup<IKenticoProductsProvider, List<ProductCategoryLink>>(p => p.GetCategories("/")
                , new List<ProductCategoryLink> {
                    new ProductCategoryLink { Order = 3, Title = "c3" },
                    new ProductCategoryLink { Order = 1, Title = "c1" },
                    new ProductCategoryLink { Order = 2, Title = "c2" },
                });

            // Act
            var actualResult = Sut.GetProducts("/");

            // Assert
            Assert.True(actualResult.Products.Count == 3);
            Assert.True(actualResult.Categories.Count == 3);
            Assert.Equal("p1", actualResult.Products[0].Title);
            Assert.Equal("p2", actualResult.Products[1].Title);
            Assert.Equal("p3", actualResult.Products[2].Title);
            Assert.Equal("c1", actualResult.Categories[0].Title);
            Assert.Equal("c2", actualResult.Categories[1].Title);
            Assert.Equal("c3", actualResult.Categories[2].Title);
        }
    }
}
