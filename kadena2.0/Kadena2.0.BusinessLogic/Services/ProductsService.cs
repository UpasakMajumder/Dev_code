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
            if (products == null)
            {
                throw new ArgumentNullException(nameof(products));
            }
            if (favorites == null)
            {
                throw new ArgumentNullException(nameof(favorites));
            }
            if (resources == null)
            {
                throw new ArgumentNullException(nameof(resources));
            }

            this.productsProvider = products;
            this.favorites = favorites;
            this.resources = resources;
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
