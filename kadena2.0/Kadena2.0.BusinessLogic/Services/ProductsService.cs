using Kadena.BusinessLogic.Contracts;
using Kadena.Models.Product;
using Kadena.WebAPI.KenticoProviders.Contracts;
using System.Linq;

namespace Kadena.BusinessLogic.Services
{
    public class ProductsService : IProductsService
    {
        private readonly IKenticoProductsProvider productsProvider;
        private readonly IKenticoFavoritesProvider favorites;

        public ProductsService(IKenticoProductsProvider products, IKenticoFavoritesProvider favorites)
        {
            this.productsProvider = products;
            this.favorites = favorites;
        }

        public ProductsPage GetProducts(string path)
        {
            var categories = productsProvider.GetCategories(path);
            var products = productsProvider.GetProducts(path);
            var favoriteIds = favorites.CheckFavoriteProductIds(products.Select(p => p.Id).ToList());

            var productsPage = new ProductsPage
            {
                Categories = categories,
                Products = products
            };

            productsPage.MarkFavoriteProducts(favoriteIds);

            return productsPage;
        }
    }
}
