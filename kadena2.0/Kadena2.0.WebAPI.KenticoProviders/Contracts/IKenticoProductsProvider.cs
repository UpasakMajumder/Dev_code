using Kadena.Models.Product;
using System.Collections.Generic;

namespace Kadena.WebAPI.KenticoProviders.Contracts
{
    public interface IKenticoProductsProvider
    {
        List<ProductLink> GetProducts(string path);
        List<ProductCategoryLink> GetCategories(string path);
        ProductCategoryLink GetCategory(string path);
        void UpdateSku(Sku sku);
        string GetSkuImageUrl(int skuid);
        Product GetProductByDocumentId(int documentId);
        Product GetProductByNodeId(int nodeId);
        string GetProductTeaserImageUrl(int documentId);
        void SetSkuAvailableQty(string skunumber, int availableItems);
    }
}
