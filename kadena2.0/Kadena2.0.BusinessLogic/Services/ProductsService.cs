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
        private readonly IKenticoSiteProvider site;

        public ProductsService(IKenticoProductsProvider products, IKenticoFavoritesProvider favorites, IKenticoResourceService resources, IKenticoSiteProvider site)
        {
            this.products = products ?? throw new ArgumentNullException(nameof(products));
            this.favorites = favorites ?? throw new ArgumentNullException(nameof(favorites));
            this.resources = resources ?? throw new ArgumentNullException(nameof(resources));
            this.site = site ?? throw new ArgumentNullException(nameof(site));
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
            var siteId = site.GetKenticoSite().Id;
            var categories = this.products.GetCategories(path).OrderBy(c => c.Order).ToList();
            var products = this.products.GetProducts(path).OrderBy(p => p.Order).ToList();
            var favoriteIds = favorites.CheckFavoriteProductIds(products.Select(p => p.Id).ToList());
            var pathCategory = this.products.GetCategory(path);
            var bordersEnabledOnSite = resources.GetSettingsKey(siteId, "KDA_ProductThumbnailBorderEnabled")?.ToLower() == "true";
            var borderEnabledOnParentCategory = pathCategory?.ProductBordersEnabled ?? true; // true to handle product in the root, without parent category
            var borderStyle = resources.GetSettingsKey(siteId, "KDA_ProductThumbnailBorderStyle");

            var productsPage = new ProductsPage
            {
                Categories = categories,
                Products = products
            };

            productsPage.MarkFavoriteProducts(favoriteIds);
            productsPage.Products.ForEach(p => p.SetBorderInfo(bordersEnabledOnSite, borderEnabledOnParentCategory, borderStyle));

            return productsPage;
        }
    }
}
