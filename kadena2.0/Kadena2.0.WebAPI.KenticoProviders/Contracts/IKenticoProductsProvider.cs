using CMS.CustomTables;
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
        void SetSkuAvailableQty(string skunumber, int availableItems);
        string GetProductStatus(int skuid);
        void SetSkuAvailableQty(int skuid, int qty);
        CustomTableItem GetAllocatedProductQuantityForUser(int productID, int userID);
        void UpdateAllocatedProductQuantityForUser(int productID, int userID, int quantity);
        List<CampaignsProduct> GetCampaignsProductSKUIDs(int campaignID);
        bool IsProductHasAllocation(int productID);
    }
}
