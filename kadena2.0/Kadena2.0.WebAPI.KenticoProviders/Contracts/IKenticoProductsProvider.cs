using Kadena.Models.Product;
using Kadena.Models.ProductOptions;
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
        Product GetProductBySkuId(int skuId);
        Price GetSkuPrice(int skuId);
        void SetSkuAvailableQty(string skunumber, int availableItems);
        string GetProductStatus(int skuid);
        Sku GetVariant(int skuId, IEnumerable<int> optionsIds);
        void SetSkuAvailableQty(int skuid, int qty);
        int GetAllocatedProductQuantityForUser(int productID, int userID);
        void UpdateAllocatedProductQuantityForUser(int productID, int userID, int quantity);
        List<CampaignsProduct> GetCampaignsProductSKUIDs(int campaignID);
        bool IsProductHasAllocation(int productID);
        OptionCategory GetOptionCategory(string codeName);
        int GetSkuAvailableQty(int skuid);
        int GetCampaignProductIDBySKUID(int skuid);
        bool ProductHasValidSKUNumber(int skuid);
        CampaignsProduct GetCampaignProduct(int skuid);
        string GetProductImagePath(int productPageId);
        IEnumerable<ProductCategory> GetProductCategories(int skuid);

        ProductPricingInfo GetDefaultVariantPricing(int documentId, string uomLocalized);
    }
}
