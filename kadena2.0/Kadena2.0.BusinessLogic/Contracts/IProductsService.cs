using Kadena.Models.Product;
using System.Collections.Generic;

namespace Kadena.BusinessLogic.Contracts
{
    public interface IProductsService
    {
        ProductsPage GetProducts(string path);

        Price GetPrice(int skuId, Dictionary<string, int> skuOptions = null);
    }
}