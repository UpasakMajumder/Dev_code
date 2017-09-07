using Kadena.WebAPI.Contracts;
using Kadena.Models.Product;
using Kadena.WebAPI.KenticoProviders.Contracts;

namespace Kadena.WebAPI.Services
{
    public class ProductsService : IProductsService
    {
        private readonly IKenticoProductsProvider productsProvider;

        public ProductsService(IKenticoProductsProvider products)
        {
            this.productsProvider = products;
        }

        public ProductsPage GetProducts(string path)
        {
            var categories = productsProvider.GetCategories(path);
            var products = productsProvider.GetProducts(path);

            return new ProductsPage
            {
                Categories = categories,
                Products = products
            };
        }
    }
}
