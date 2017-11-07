using Kadena.Models.Product;
using System.Collections.Generic;

namespace Kadena.WebAPI.KenticoProviders.Contracts
{
    public interface IKenticoProductsProvider
    {
        List<ProductLink> GetProducts(string path);
        List<ProductCategoryLink> GetCategories(string path);
        void UpdateSku(Sku sku);
    }
}
