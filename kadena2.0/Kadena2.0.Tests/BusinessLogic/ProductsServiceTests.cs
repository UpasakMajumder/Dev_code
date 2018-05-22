using Kadena.WebAPI.KenticoProviders.Contracts;
using Kadena.BusinessLogic.Services;
using Xunit;
using System.Collections.Generic;
using Kadena.Models.Product;
using Moq;
using System;
using Kadena.Models.Site;
using Kadena.Models.SiteSettings;
using Kadena2.WebAPI.KenticoProviders.Contracts;
using Kadena.Models.SiteSettings.Permissions;
using System.Linq;
using Kadena.Models;

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

            Setup<IKenticoResourceService, bool>(r => r.GetSiteSettingsKey<bool>(Settings.KDA_ProductThumbnailBorderEnabled), borderOnSite);
            Setup<IKenticoResourceService, string>(r => r.GetSiteSettingsKey(Settings.KDA_ProductThumbnailBorderStyle), borderStyleValue);
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
            Setup<IKenticoSiteProvider, KenticoSite>(sm => sm.GetKenticoSite(), new KenticoSite { Id = 1 });

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

        [Theory(DisplayName = "ProductsService.GetAvailableProductsString() | Non inventory type")]
        [InlineData("KDA.MailingProduct")]
        [InlineData("KDA.TemplatedProduct")]
        [InlineData("KDA.ProductWithAddOns")]
        [InlineData("KDA.StaticProduct")]
        [InlineData("KDA.POD")]
        public void GetAvailableProductStringTest_NonInventory(string productType)
        {
            // Arrange
            Setup<IKenticoResourceService, string>(r => r.GetResourceString(It.IsAny<string>(), It.IsAny<string>()), "value");

            // Act
            var result = Sut.GetInventoryProductAvailability(productType, 10, "cz", 10, "igelitka");

            // Assert
            Assert.Null(result);
        }


        [Theory(DisplayName = "ProductsService.GetAvailableProductsString() | Inventory type")]
        [InlineData(null, 0, "Kadena.Product.Unavailable")]
        [InlineData(0, 0, "Kadena.Product.OutOfStock")]
        [InlineData(5, 10, "10 plasticbags in stock")]
        public void GetAvailableProductStringTest_Inventory(int? numberOfAvailableProducts, int numberOfStockProducts, string expectedResult)
        {
            // Arrange
            const string culture = "cz-CZ";
            const string unit = "plasticbags";
            Setup<IKenticoResourceService, string, string, string>(r => r.GetResourceString(It.IsAny<string>(), culture), (a, b) => a);
            Setup<IKenticoResourceService, string>(r => r.GetResourceString("Kadena.Product.NumberOfAvailableProducts", culture), "{0} {1} in stock");
            Setup<IKenticoUnitOfMeasureProvider, UnitOfMeasure>(r => r.GetUnitOfMeasure(unit), new UnitOfMeasure {LocalizationString = unit });

            // Act
            var result = Sut.GetInventoryProductAvailability("KDA.InventoryProduct", numberOfAvailableProducts, culture, numberOfStockProducts, unit);

            // Assert
            Assert.Equal(expectedResult, result.Text);
        }



        [Fact(DisplayName = "GetPackagingStringTest | Default unit")]
        public void GetPackagingStringTest_DefaultUnit()
        {
            // Arrange
            const string localizationString = "loc.key";
            const string unitOfMeasure = "Each";

            Setup<IKenticoUnitOfMeasureProvider, UnitOfMeasure>(p => p.GetUnitOfMeasure(unitOfMeasure), 
                                                                new UnitOfMeasure { LocalizationString = localizationString, IsDefault = true });
            
            // Act
            var result = Sut.GetPackagingString(10, unitOfMeasure, "cz-CZ");

            // Assert
            Assert.Equal(string.Empty, result);
        }

        [Fact(DisplayName = "GetPackagingStringTest")]
        public void GetPackagingStringTest()
        {
            // Arrange
            const string culture = "cz-CZ";
            const string localizationString = "loc.key";
            const string unitOfMeasure = "Role";

            Setup<IKenticoUnitOfMeasureProvider, UnitOfMeasure>(p => p.GetUnitOfMeasure(unitOfMeasure), 
                                                                new UnitOfMeasure { LocalizationString = localizationString });
            Setup<IKenticoResourceService, string>(r => r.GetResourceString("Kadena.Product.NumberOfItemsInPackagesFormatString", culture), "This comes in {0} of {1}");
            Setup<IKenticoResourceService, string>(r => r.GetResourceString(localizationString, culture), "RoleCZ");
            
            // Act
            var result = Sut.GetPackagingString(10, unitOfMeasure, "cz-CZ");

            // Assert
            Assert.Equal("This comes in RoleCZ of 10", result);
        }

        [Theory(DisplayName = "GetUnitOfMeasureTest")]
        [InlineData("Each", true, "")]
        [InlineData("Role", false, "RoleCZ")]
        public void GetUnitOfMeasureTest(string unitOfMeasure, bool isDefault, string expectedResult)
        {
            // Arrange
            const string culture = "cz-CZ";
            const string localizationString = "loc.key";
            Setup<IKenticoUnitOfMeasureProvider, UnitOfMeasure>(p => p.GetUnitOfMeasure(unitOfMeasure), new UnitOfMeasure { LocalizationString = localizationString, IsDefault = isDefault });
            Setup<IKenticoResourceService, string>(r => r.GetResourceString(localizationString, culture), expectedResult);

            // Act
            var result = Sut.GetUnitOfMeasure(unitOfMeasure, culture);

            // Assert
            Assert.Equal(expectedResult, result);
        }

        [Theory(DisplayName = "TranslateUnitOfMeasureTest")]
        [InlineData("Each", true, "EachCZ")]
        [InlineData("Role", false, "RoleCZ")]
        public void TranslateUnitOfMeasureTest(string unitOfMeasure, bool isDefault, string expectedResult)
        {
            // Arrange
            const string culture = "cz-CZ";
            const string localizationString = "loc.key";
            Setup<IKenticoUnitOfMeasureProvider, UnitOfMeasure>(p => p.GetUnitOfMeasure(unitOfMeasure), new UnitOfMeasure { LocalizationString = localizationString, IsDefault = isDefault });
            Setup<IKenticoResourceService, string>(r => r.GetResourceString(localizationString, culture), expectedResult);

            // Act
            var result = Sut.TranslateUnitOfMeasure(unitOfMeasure, culture);

            // Assert
            Assert.Equal(expectedResult, result);
        }


        [Theory(DisplayName = "ProductsService.GetInventoryProductAvailability() | Non inventory type")]
        [InlineData("KDA.MailingProduct")]
        [InlineData("KDA.TemplatedProduct")]
        [InlineData("KDA.ProductWithAddOns")]
        [InlineData("KDA.StaticProduct")]
        [InlineData("KDA.POD")]
        public void GetInventoryProductAvailablity_NonInventory(string productType)
        {
            // Act
            var result = Sut.GetInventoryProductAvailability(productType, 10, "cs-CZ", 10, "carton");

            // Assert
            Assert.Null(result);
        }

        [Theory(DisplayName = "ProductsService.GetInventoryProductAvailability() | Inventory type")]
        [InlineData(0, 0, "outofstock")]
        [InlineData(null, 0, "unavailable")]
        [InlineData(1, 1, "available")]
        public void GetInventoryProductAvailablity(int? numberOfAvailableProducts, int numberOfStockProducts, string expectedResult)
        {
            // Arrange
            const string culture = "cz-CZ";
            const string unit = "carton";
            Setup<IKenticoUnitOfMeasureProvider, UnitOfMeasure>(r => r.GetUnitOfMeasure(unit), new UnitOfMeasure { LocalizationString = unit });
            Setup<IKenticoResourceService, string, string, string>(r => r.GetResourceString(It.IsAny<string>(), culture), (a, b) => a);
            Setup<IKenticoResourceService, string>(r => r.GetResourceString("Kadena.Product.NumberOfAvailableProducts", culture), "{0} {1} in stock");

            // Act
            var result = Sut.GetInventoryProductAvailability("KDA.InventoryProduct", numberOfAvailableProducts, culture, numberOfStockProducts, unit);

            // Assert
            Assert.NotNull(expectedResult);
            Assert.Equal(expectedResult, result.Type);
        }


        [Theory(DisplayName = "ProductsService.CanDisplayAddToCartButton() | Non inventory type")]
        [InlineData("KDA.MailingProduct", false)]
        [InlineData("KDA.TemplatedProduct", false)]
        [InlineData("KDA.ProductWithAddOns", true)]
        [InlineData("KDA.StaticProduct", true)]
        [InlineData("KDA.POD", true)]
        public void CanDisplayAddToCart_NonInventory(string productType, bool expectedResult)
        {
            // Act
            var result = Sut.CanDisplayAddToCartButton(productType, 0, false);

            // Assert
            Assert.Equal(expectedResult, result);
        }

        [Theory(DisplayName = "ProductsService.CanDisplayAddToCartButton() | Inventory type")]
        [InlineData(null, true, false)]
        [InlineData(null, false, false)]
        [InlineData(0, true, false)]
        [InlineData(0, false, true)]
        [InlineData(1, true, true)]
        [InlineData(1, false, true)]
        public void CanDisplayAddToCart_Inventory(int? numberOfAvailableProducts, bool sellOnlyAvailable, bool expectedResult)
        {
            // Act
            var result = Sut.CanDisplayAddToCartButton("KDA.InventoryProduct", numberOfAvailableProducts, sellOnlyAvailable);

            // Assert
            Assert.Equal(expectedResult, result);
        }

        public static IEnumerable<object[]> GetEmptyOptions()
        {
            yield return new object[]
            {
                new Dictionary<string, int> ()
            };
            yield return new object[]
            {
                new Dictionary<string, int> {
                    { "Option", 0 }
                }
            };
            yield return new object[]
            {
                null
            };
        }

        public static IEnumerable<object[]> GetOptions()
        {
            yield return new object[]
            {
                new Dictionary<string, int> {
                    { "Option", 0 }
                }
            };
        }

        [Theory(DisplayName = "ProductsService.GetPrice()")]
        [MemberData(nameof(GetEmptyOptions))]
        [MemberData(nameof(GetOptions))]
        public void GetPrice(Dictionary<string, int> options)
        {
            Setup<IKenticoProductsProvider, Sku>(p => p.GetVariant(It.IsAny<int>(), It.IsAny<IEnumerable<int>>()), new Sku());

            var result = Sut.GetPrice(0, options);

            Assert.Null(result);
        }

        [Theory(DisplayName = "ProductsService.GetPrice() | Variant not found")]
        [MemberData(nameof(GetOptions))]
        public void GetPrice_NonExistingVariant(Dictionary<string, int> options)
        {
            Assert.Throws<ArgumentException>(() => Sut.GetPrice(0, options));
        }

        [Fact(DisplayName = "ProductsService.GetMinMaxItemsString() | Unlimited")]
        public void GetMinMaxItemsStringTest_Unlimited()
        {
            var result = Sut.GetMinMaxItemsString(0, 0);

            Assert.Empty(result);
        }

        [Fact(DisplayName = "ProductsService.GetMinMaxItemsString() | MinMax")]
        public void GetMinMaxItemsStringTest_MinMax()
        {
            Setup<IKenticoResourceService, string>(r => r.GetResourceString("Kadena.Product.MinMaxInfo.MinMax"), "you can {0} - {1}");

            var result = Sut.GetMinMaxItemsString(10, 20);

            Assert.Equal("you can 10 - 20", result);
        }

        [Fact(DisplayName = "ProductsService.GetMinMaxItemsString() | Min")]
        public void GetMinMaxItemsStringTest_Min()
        {
            Setup<IKenticoResourceService, string>(r => r.GetResourceString("Kadena.Product.MinMaxInfo.Min"), "you have to at least {0}");

            var result = Sut.GetMinMaxItemsString(10, 0);

            Assert.Equal("you have to at least 10", result);
        }


        [Fact(DisplayName = "ProductsService.GetMinMaxItemsString() | Max")]
        public void GetMinMaxItemsStringTest_Max()
        {
            Setup<IKenticoResourceService, string>(r => r.GetResourceString("Kadena.Product.MinMaxInfo.Max"), "you can upto {0}");

            var result = Sut.GetMinMaxItemsString(0, 20);

            Assert.Equal("you can upto 20", result);
        }


        [Fact(DisplayName = "ProductsService.GetProductEstimationsTest() | CanSeePrices")]
        public void GetProductEstimationsTest_CanSeePrices()
        {
            const int productId = 123;
            const string productionTime = "minute";
            const string shipTime = "day";
            const string shippingCost = "1usd";
            const bool canSeePrices = true;
            
            Setup<IKenticoResourceService, string, string>(r => r.GetResourceString(It.IsAny<string>()), s => "localized-" + s);
            Setup<IKenticoProductsProvider, Product>(p => p.GetProductByDocumentId(productId), new Product { ProductionTime = productionTime, ShipTime = shipTime, ShippingCost = shippingCost });
            Setup<IKenticoPermissionsProvider, bool>(p => p.CurrentUserHasPermission(ModulePermissions.KadenaOrdersModule, ModulePermissions.KadenaOrdersModule.SeePrices), canSeePrices);

            var result = Sut.GetProductEstimations(productId)?.ToArray();

            Assert.NotNull(result);
            Assert.Equal(3, result.Length);
            Assert.Equal("localized-Kadena.Product.ProductionTime", result[0].Key);
            Assert.Equal("minute", result[0].Value);
            Assert.Equal("localized-Kadena.Product.ShipTime", result[1].Key);
            Assert.Equal("day", result[1].Value);
            Assert.Equal("localized-Kadena.Product.ShippingCost", result[2].Key);
            Assert.Equal("1usd", result[2].Value);
        }

        [Fact(DisplayName = "ProductsService.GetProductEstimationsTest() | CannotSeePrices")]
        public void GetProductEstimationsTest_CannotSeePrices()
        {
            const int productId = 123;
            const string productionTime = "minute";
            const string shipTime = "day";
            const string shippingCost = "1usd";
            const bool canSeePrices = false;

            Setup<IKenticoResourceService, string, string>(r => r.GetResourceString(It.IsAny<string>()), s => "localized-" + s);
            Setup<IKenticoProductsProvider, Product>(p => p.GetProductByDocumentId(productId), new Product { ProductionTime = productionTime, ShipTime = shipTime, ShippingCost = shippingCost });
            Setup<IKenticoPermissionsProvider, bool>(p => p.CurrentUserHasPermission(ModulePermissions.KadenaOrdersModule, ModulePermissions.KadenaOrdersModule.SeePrices), canSeePrices);

            var result = Sut.GetProductEstimations(productId)?.ToArray();

            Assert.NotNull(result);
            Assert.Equal(2, result.Length);
            Assert.Equal("localized-Kadena.Product.ProductionTime", result[0].Key);
            Assert.Equal("minute", result[0].Value);
            Assert.Equal("localized-Kadena.Product.ShipTime", result[1].Key);
            Assert.Equal("day", result[1].Value);
            Assert.Null(result.FirstOrDefault(r => r.Key == "localized-Kadena.Product.ShippingCost"));
        }

        [Fact(DisplayName = "ProductsService.GetProductPricings() | No dynamic ranges")]
        public void GetProductPricingTest_NoDynamic()
        {
            const int productId = 123;
            const string uomCode = "bag";
            const string uomLocalizationString = "bagstring";
            const string uomCodeLocalized = "locbag";
            const string culture = "cz-CZ";
            const string price = "price";
            const string dollar = "$1";

            Setup<IDynamicPriceRangeProvider, IEnumerable<DynamicPricingRange>>(r => r.GetDynamicRanges(productId), new List<DynamicPricingRange>() );
            Setup<IKenticoUnitOfMeasureProvider, UnitOfMeasure>(u => u.GetUnitOfMeasure(uomCode), new UnitOfMeasure { LocalizationString = uomLocalizationString });
            Setup<IKenticoProductsProvider, ProductPricingInfo>(p => p.GetDefaultVariantPricing(productId, uomCodeLocalized), new ProductPricingInfo { Id = "id", Key = price, Value = dollar });
            Setup<IKenticoResourceService, string>(r => r.GetResourceString(uomLocalizationString, culture), uomCodeLocalized);

            var result = Sut.GetProductPricings(productId, uomCode, "cz-CZ")?.ToArray();

            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal(price, result[0].Key);
            Assert.Equal(dollar, result[0].Value);
        }
    }
}
