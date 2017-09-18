using Kadena.Models.Product;

namespace Kadena.WebAPI.Contracts
{
    public interface IProductsService
    {
        ProductsPage GetProducts(string path);
    }
}