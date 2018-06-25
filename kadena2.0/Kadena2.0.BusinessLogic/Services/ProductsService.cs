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
        private readonly IKenticoSkuProvider skus;
        private readonly IKenticoFavoritesProvider favorites;
        private readonly IKenticoResourceService resources;
        private readonly IKenticoUnitOfMeasureProvider units;
        private readonly IImageService imageService;
        private readonly IKenticoPermissionsProvider permissions;
        private readonly IDynamicPriceRangeProvider dynamicRanges;
        private readonly ITieredPriceRangeProvider tieredRanges;

        public ProductsService(IKenticoProductsProvider products,
                               IKenticoSkuProvider skus,
                               IKenticoFavoritesProvider favorites, 
                               IKenticoResourceService resources, 
                               IKenticoUnitOfMeasureProvider units, 
                               IImageService imageService, 
                               IKenticoPermissionsProvider permissions, 
                               IDynamicPriceRangeProvider dynamicRanges, 
                               ITieredPriceRangeProvider tieredRanges)
        {
            this.products = products ?? throw new ArgumentNullException(nameof(products));
            this.skus = skus ?? throw new ArgumentNullException(nameof(skus));
            this.favorites = favorites ?? throw new ArgumentNullException(nameof(favorites));
            this.resources = resources ?? throw new ArgumentNullException(nameof(resources));
            this.imageService = imageService ?? throw new ArgumentNullException(nameof(imageService));
            this.units = units ?? throw new ArgumentNullException(nameof(units));
            this.permissions = permissions ?? throw new ArgumentNullException(nameof(permissions));
            this.dynamicRanges = dynamicRanges ?? throw new ArgumentNullException(nameof(dynamicRanges));
            this.tieredRanges = tieredRanges ?? throw new ArgumentNullException(nameof(tieredRanges));
        }

        public Price GetPrice(int skuId, Dictionary<string, int> skuOptions = null)
        {
            if ((skuOptions?.Count ?? 0) == 0)
            {
                return skus.GetSkuPrice(skuId);
            }

            var selectedVariant = skus.GetVariant(skuId, new HashSet<int>(skuOptions.Values.Distinct()));
            if (selectedVariant == null)
            {
                throw new ArgumentException("Product Variant for specified SKU and Options not found.");
            }
            return skus.GetSkuPrice(selectedVariant.SkuId);
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

        string GetAvailableProductsString(int? numberOfAvailableProducts, string unitOfMeasureCode)
        {
            string formattedValue = string.Empty;

            if (!numberOfAvailableProducts.HasValue)
            {
                formattedValue = resources.GetResourceString("Kadena.Product.Unavailable");
            }
            else if (numberOfAvailableProducts.Value == 0)
            {
                formattedValue = resources.GetResourceString("Kadena.Product.OutOfStock");
            }
            else
            {
                var baseString = resources.GetResourceString("Kadena.Product.NumberOfAvailableProducts");
                var uomDisplayName = resources.GetResourceString(units.GetUnitOfMeasure(unitOfMeasureCode).LocalizationString);
                formattedValue = string.Format(baseString, numberOfAvailableProducts, uomDisplayName);
            }

            return formattedValue;
        }

        string GetAvailabilityType(int? availableItems)
        {
            if (!availableItems.HasValue)
            {
                return ProductAvailability.Unavailable;
            }
            else if (availableItems.Value == 0)
            {
                return ProductAvailability.OutOfStock;
            }
            else
            {
                return ProductAvailability.Available;
            }
        }

        public ProductAvailability GetInventoryProductAvailability(int skuId)
        {
            var sku = skus.GetSKU(skuId);

            if (sku == null)
            {
                return null;
            }

            return new ProductAvailability
            {
                Text = GetAvailableProductsString(sku.AvailableItems, sku.UnitOfMeasure),
                Type = GetAvailabilityType(sku.AvailableItems)
            };
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

        public IEnumerable<int> GetProductTiers(int documentId)
        {
            var product = products.GetProductByDocumentId(documentId);
            var pricingModel = product?.PricingModel;
            var tiers = new List<int>();

            if (pricingModel == PricingModel.Tiered)
            {
                var ranges = tieredRanges.GetTieredRanges(documentId);
                if (ranges != null)
                {
                    tiers.AddRange(ranges.Select(tr => tr.Quantity));
                }
            }

            return tiers;
        }

        public IEnumerable<ProductPricingInfo> GetProductPricings(int documentId, string pricingModel, string unitOfMeasure, string cultureCode)
        {
            var pricings = new List<ProductPricingInfo>();
            var localizedUom = TranslateUnitOfMeasure(unitOfMeasure, cultureCode);
            var currencySymbol = "$";
            
            if (pricingModel == PricingModel.Dynamic)
            {
                FillDynamicPrices(pricings, documentId, localizedUom, currencySymbol);
            }
            else if (pricingModel == PricingModel.Tiered)
            {
                FillTieredPrices(pricings, documentId, localizedUom, currencySymbol);
            }

            return pricings;
        }

        private void FillDynamicPrices(List<ProductPricingInfo> pricings, int documentId, string localizedUom, string currencySymbol)
        {
            var dynamicRanges = this.dynamicRanges.GetDynamicRanges(documentId);

            if((dynamicRanges?.Count() ?? 0) == 0)
            {
                pricings.Add(products.GetDefaultVariantPricing(documentId, localizedUom));
                return;
            }

            foreach (var r in dynamicRanges)
            {
                pricings.Add(new ProductPricingInfo
                {
                    Key = $"{r.MinVal}-{r.MaxVal} {localizedUom}",
                    Value = $"{currencySymbol}{r.Price}"
                });
            }
        }

        private void FillTieredPrices(List<ProductPricingInfo> pricings, int documentId, string localizedUom, string currencySymbol)
        {
            var tieredRanges = this.tieredRanges.GetTieredRanges(documentId);

            if ((tieredRanges?.Count() ?? 0) == 0)
            {
                pricings.Add(products.GetDefaultVariantPricing(documentId, localizedUom));
                return;
            }

            foreach (var r in tieredRanges)
            {
                pricings.Add(new ProductPricingInfo
                {
                    Key = $"{r.Quantity} {localizedUom}",
                    Value = $"{currencySymbol}{r.Price.ToString("N2")}"
                });
            }
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
