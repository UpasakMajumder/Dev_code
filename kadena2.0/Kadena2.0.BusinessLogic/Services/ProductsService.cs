using Kadena.BusinessLogic.Contracts;
using Kadena.Models.Product;
using Kadena.WebAPI.KenticoProviders.Contracts;
using System;
using System.Linq;

namespace Kadena.BusinessLogic.Services
{
    public class ProductsService : IProductsService
    {
        private readonly IKenticoProductsProvider productsProvider;
        private readonly IKenticoFavoritesProvider favorites;
        private readonly IKenticoResourceService resources;

        public ProductsService(IKenticoProductsProvider products, IKenticoFavoritesProvider favorites, IKenticoResourceService resources)
        {
            this.productsProvider = products ?? throw new ArgumentNullException(nameof(products));
            this.favorites = favorites ?? throw new ArgumentNullException(nameof(favorites));
            this.resources = resources ?? throw new ArgumentNullException(nameof(resources));
        }

        public ProductsPage GetProducts(string path)
        {
            var categories = productsProvider.GetCategories(path);
            var products = productsProvider.GetProducts(path);
            var favoriteIds = favorites.CheckFavoriteProductIds(products.Select(p => p.Id).ToList());
            var pathCategory = productsProvider.GetCategory(path);
            var bordersEnabledOnSite = resources.GetSettingsKey("KDA_ProductThumbnailBorderEnabled").ToLower() == "true";
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

        
    }
}
