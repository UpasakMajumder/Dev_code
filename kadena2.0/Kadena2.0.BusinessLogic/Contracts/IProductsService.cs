using Kadena.Models.Product;

namespace Kadena.BusinessLogic.Contracts
{
    public interface IProductsService
    {
        ProductsPage GetProducts(string path);
    }
}