using Kadena.BusinessLogic.Contracts;
using Kadena.Models.Product;
using Kadena.WebAPI.KenticoProviders.Contracts;
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

        public ProductsService(IKenticoProductsProvider products, IKenticoFavoritesProvider favorites, IKenticoResourceService resources)
        {
            this.products = products ?? throw new ArgumentNullException(nameof(products));
            this.favorites = favorites ?? throw new ArgumentNullException(nameof(favorites));
            this.resources = resources ?? throw new ArgumentNullException(nameof(resources));
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
            var bordersEnabledOnSite = resources.GetSettingsKey("KDA_ProductThumbnailBorderEnabled")?.ToLower() == "true";
            var borderEnabledOnParentCategory = pathCategory?.ProductBordersEnabled ?? true; // true to handle product in the root, without parent category
            var borderStyle = resources.GetSettingsKey("KDA_ProductThumbnailBorderStyle");

            var productsPage = new ProductsPage
            {
                Categories = categories,
                Products = products
            };

            productsPage.MarkFavoriteProducts(favoriteIds);
            productsPage.Products.ForEach(p => p.SetBorderInfo(bordersEnabledOnSite, borderEnabledOnParentCategory, borderStyle));

            return productsPage;
        }

        public string GetAvailableProductsString(string productType, int? numberOfAvailableProducts, string cultureCode, int numberOfStockProducts)
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
                formattedValue = string.Format(baseString, numberOfStockProducts);
            }

            return formattedValue;
        }

        public string GetInventoryProductAvailability(string productType, int? numberOfAvailableProducts, int numberOfStockProducts)
        {
            if (ProductTypes.IsOfType(productType, ProductTypes.InventoryProduct))
            {
                if (!numberOfAvailableProducts.HasValue)
                {
                    return ProductAvailability.Unavailable;
                }

                if (numberOfStockProducts == 0)
                {
                    return ProductAvailability.OutOfStock;
                }

                return ProductAvailability.Available;
            }

            return string.Empty;
        }

        public bool CanDisplayAddToCartButton(string productType, int? numberOfAvailableProducts, bool sellOnlyIfAvailable)
        {
            var isStatic = ProductTypes.IsOfType(productType, ProductTypes.StaticProduct);
            var isPod = ProductTypes.IsOfType(productType, ProductTypes.POD);
            var isWithAddons = ProductTypes.IsOfType(productType, ProductTypes.ProductWithAddOns);
            var isTemplated = ProductTypes.IsOfType(productType, ProductTypes.TemplatedProduct);
            var isInventory = ProductTypes.IsOfType(productType, ProductTypes.InventoryProduct);

            if ( (isStatic || isPod || isWithAddons) && !isTemplated )
            {
                return true;
            }

            if (isInventory)
            {
                if (sellOnlyIfAvailable)
                {
                    return numberOfAvailableProducts.HasValue && numberOfAvailableProducts.Value > 0;
                }
                else
                {
                    return true;
                }
            }

            return false;
        }
    }
}
