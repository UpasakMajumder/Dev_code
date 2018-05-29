using Kadena.BusinessLogic.Contracts;
using Kadena.Models.Product;
using Kadena.Models.SiteSettings;
using Kadena.Models.SiteSettings.Permissions;
using Kadena.WebAPI.KenticoProviders.Contracts;
using Kadena2.WebAPI.KenticoProviders.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Kadena.BusinessLogic.Services
{
    public class ProductsService : IProductsService
    {
        private readonly IKenticoProductsProvider products;
        private readonly IKenticoFavoritesProvider favorites;
        private readonly IKenticoResourceService resources;
        private readonly IKenticoUnitOfMeasureProvider units;
        private readonly IImageService imageService;
        private readonly IKenticoPermissionsProvider permissions;
        private readonly IDynamicPriceRangeProvider dynamicRanges;

        public ProductsService(IKenticoProductsProvider products, IKenticoFavoritesProvider favorites, IKenticoResourceService resources, IKenticoUnitOfMeasureProvider units, IImageService imageService, IKenticoPermissionsProvider permissions, IDynamicPriceRangeProvider dynamicRanges)
        {
            this.products = products ?? throw new ArgumentNullException(nameof(products));
            this.favorites = favorites ?? throw new ArgumentNullException(nameof(favorites));
            this.resources = resources ?? throw new ArgumentNullException(nameof(resources));
            this.imageService = imageService ?? throw new ArgumentNullException(nameof(imageService));
            this.units = units ?? throw new ArgumentNullException(nameof(units));
            this.permissions = permissions ?? throw new ArgumentNullException(nameof(permissions));
            this.dynamicRanges = dynamicRanges ?? throw new ArgumentNullException(nameof(dynamicRanges));
        }

        public Price GetPrice(int skuId, Dictionary<string, int> skuOptions = null)
        {
            if ((skuOptions?.Count ?? 0) == 0)
            {
                return products.GetSkuPrice(skuId);
            }

            var selectedVariant = products.GetVariant(skuId, new HashSet<int>(skuOptions.Values.Distinct()));
            if (selectedVariant == null)
            {
                throw new ArgumentException("Product Variant for specified SKU and Options not found.");
            }
            return products.GetSkuPrice(selectedVariant.SkuId);
        }

        public ProductsPage GetProducts(string path)
        {
            var categories = this.products.GetCategories(path).OrderBy(c => c.Order).ToList();
            var products = this.products.GetProducts(path).OrderBy(p => p.Order).ToList();
            var favoriteIds = favorites.CheckFavoriteProductIds(products.Select(p => p.Id).ToList());
            var pathCategory = this.products.GetCategory(path);
            var bordersEnabledOnSite = resources.GetSiteSettingsKey<bool>(Settings.KDA_ProductThumbnailBorderEnabled);
            var borderEnabledOnParentCategory = pathCategory?.ProductBordersEnabled ?? true; // true to handle product in the root, without parent category
            var borderStyle = resources.GetSiteSettingsKey(Settings.KDA_ProductThumbnailBorderStyle);

            products.ForEach(p => p.ImageUrl = imageService.GetThumbnailLink(p.ImageUrl));
            categories.ForEach(c => c.ImageUrl = imageService.GetThumbnailLink(c.ImageUrl));

            var productsPage = new ProductsPage
            {
                Categories = categories,
                Products = products
            };

            productsPage.MarkFavoriteProducts(favoriteIds);
            productsPage.Products.ForEach(p => p.SetBorderInfo(bordersEnabledOnSite, borderEnabledOnParentCategory, borderStyle));

            return productsPage;
        }

        string GetAvailableProductsString(string productType, int? numberOfAvailableProducts, string cultureCode, int numberOfStockProducts, string unitOfMeasureCode)
        {
            string formattedValue = string.Empty;

            if (!ProductTypes.IsOfType(productType, ProductTypes.InventoryProduct))
            {
                return formattedValue;
            }

            if (!numberOfAvailableProducts.HasValue)
            {
                formattedValue = resources.GetResourceString("Kadena.Product.Unavailable", cultureCode);
            }
            else if (numberOfStockProducts == 0)
            {
                formattedValue = resources.GetResourceString("Kadena.Product.OutOfStock", cultureCode);
            }
            else
            {
                var baseString = resources.GetResourceString("Kadena.Product.NumberOfAvailableProducts", cultureCode);
                var uomDisplayName = resources.GetResourceString(units.GetUnitOfMeasure(unitOfMeasureCode).LocalizationString, cultureCode);
                formattedValue = string.Format(baseString, numberOfStockProducts, uomDisplayName);
            }

            return formattedValue;
        }

        public ProductAvailability GetInventoryProductAvailability(string productType, int? numberOfAvailableProducts, string cultureCode, int numberOfStockProducts, string unitOfMeasureCode)
        {
            if (ProductTypes.IsOfType(productType, ProductTypes.InventoryProduct))
            {
                var availability = new ProductAvailability();

                if (!numberOfAvailableProducts.HasValue)
                {
                    availability.Type = ProductAvailability.Unavailable;
                }
                else if (numberOfStockProducts == 0)
                {
                    availability.Type = ProductAvailability.OutOfStock;
                }
                else
                {
                    availability.Type = ProductAvailability.Available;
                }

                availability.Text = GetAvailableProductsString(productType, numberOfAvailableProducts, cultureCode, numberOfStockProducts, unitOfMeasureCode);
                return availability;
            }

            return null;
        }

        public bool CanDisplayAddToCartButton(string productType, int? numberOfAvailableProducts, bool sellOnlyIfAvailable)
        {
            var isStatic = ProductTypes.IsOfType(productType, ProductTypes.StaticProduct);
            var isPod = ProductTypes.IsOfType(productType, ProductTypes.POD);
            var isWithAddons = ProductTypes.IsOfType(productType, ProductTypes.ProductWithAddOns);
            var isTemplated = ProductTypes.IsOfType(productType, ProductTypes.TemplatedProduct);
            var isInventory = ProductTypes.IsOfType(productType, ProductTypes.InventoryProduct);

            if ((isStatic || isPod || isWithAddons) && !isTemplated)
            {
                return true;
            }

            if (isInventory)
            {
                if (!numberOfAvailableProducts.HasValue)
                {
                    return false;
                }

                if (sellOnlyIfAvailable)
                {
                    return numberOfAvailableProducts.Value > 0;
                }
                else
                {
                    return true;
                }
            }

            return false;
        }

        public string GetPackagingString(int numberOfItemsInPackage, string unitOfMeasure, string cultureCode)
        {
            if (numberOfItemsInPackage <= 0 || string.IsNullOrEmpty(unitOfMeasure))
            {
                return string.Empty;
            }

            var unit = units.GetUnitOfMeasure(unitOfMeasure);

            if (unit == null || unit.IsDefault)
            {
                return string.Empty;
            }

            var localizedUnit = resources.GetResourceString(unit.LocalizationString, cultureCode);
            var stringBase = resources.GetResourceString("Kadena.Product.NumberOfItemsInPackagesFormatString", cultureCode);

            return string.Format(stringBase, localizedUnit, numberOfItemsInPackage);
        }

        public string GetUnitOfMeasure(string unitOfMeasure, string cultureCode)
        {
            var unit = units.GetUnitOfMeasure(unitOfMeasure);

            if (unit == null || unit.IsDefault)
            {
                return string.Empty;
            }

            return resources.GetResourceString(unit.LocalizationString, cultureCode);
        }

        public string TranslateUnitOfMeasure(string unitOfMeasure, string cultureCode)
        {
            var unit = units.GetUnitOfMeasure(unitOfMeasure);
            return resources.GetResourceString(unit.LocalizationString, cultureCode);
        }

        public IEnumerable<ProductEstimation> GetProductEstimations(int documentId)
        {
            var product = products.GetProductByDocumentId(documentId);
            bool canSeePrices = permissions.CurrentUserHasPermission(ModulePermissions.KadenaOrdersModule, ModulePermissions.KadenaOrdersModule.SeePrices);
            var estimates = new List<ProductEstimation>();

            if (!string.IsNullOrEmpty(product.ProductionTime))
            {
                estimates.Add(new ProductEstimation
                {
                    Key = resources.GetResourceString("Kadena.Product.ProductionTime"),
                    Value = product.ProductionTime
                });
            }

            if (!string.IsNullOrEmpty(product.ShipTime))
            {
                estimates.Add(new ProductEstimation
                {
                    Key = resources.GetResourceString("Kadena.Product.ShipTime"),
                    Value = product.ShipTime
                });
            }

            if (!string.IsNullOrEmpty(product.ShippingCost) && canSeePrices)
            {
                estimates.Add(new ProductEstimation
                {
                    Key = resources.GetResourceString("Kadena.Product.ShippingCost"),
                    Value = product.ShippingCost
                });
            }

            return estimates;
        }

        public IEnumerable<ProductPricingInfo> GetProductPricings(int documentId, string unitOfMeasure, string cultureCode)
        {
            var ranges = dynamicRanges.GetDynamicRanges(documentId);
            var localizedUom = TranslateUnitOfMeasure(unitOfMeasure, cultureCode);
            var pricings = new List<ProductPricingInfo>();
            var currencySymbol = "$";

            if (ranges == null || ranges.Count() == 0)
            {
                pricings.Add(products.GetDefaultVariantPricing(documentId, localizedUom));
            }
            else
            {
                foreach (var r in ranges)
                {
                    pricings.Add(new ProductPricingInfo
                    {
                        Key = $"{r.MinVal}-{r.MaxVal} {localizedUom}",
                        Value = $"{currencySymbol}{r.Price}"
                    });
                }
            }

            return pricings;
        }

        public string GetMinMaxItemsString(int min, int max)
        {
            if (min > 0 && max > 0)
            {
                var format = resources.GetResourceString("Kadena.Product.MinMaxInfo.MinMax");
                return string.Format(format, min, max);
            }

            if (min > 0)
            {
                var format = resources.GetResourceString("Kadena.Product.MinMaxInfo.Min");
                return string.Format(format, min);
            }

            if (max > 0)
            {
                var format = resources.GetResourceString("Kadena.Product.MinMaxInfo.Max");
                return string.Format(format, max);
            }

            return string.Empty;
        }
    }
}
