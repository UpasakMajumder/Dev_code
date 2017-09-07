using Kadena.WebAPI.Contracts;
using AutoMapper;
using Kadena.Models.Product;
using Kadena.WebAPI.KenticoProviders.Contracts;

namespace Kadena.WebAPI.Services
{
    public class ProductsService : IProductsService
    {
        private readonly IMapper mapper;
        private readonly IKenticoProductsProvider productsProvider;
        

        public ProductsService(IMapper mapper, IKenticoProductsProvider products)
        {
            this.mapper = mapper;
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
