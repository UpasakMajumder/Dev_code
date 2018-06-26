using Kadena.Models.Product;
using Kadena.Models.ProductOptions;
using System;
using System.Collections.Generic;

namespace Kadena.WebAPI.KenticoProviders.Contracts
{
    public interface IKenticoProductsProvider
    {
        List<ProductLink> GetProducts(string path);
        List<ProductCategoryLink> GetCategories(string path);
        ProductCategoryLink GetCategory(string path);        
        string GetSkuImageUrl(int skuid);
        Product GetProductByDocumentId(int documentId);
        Product GetProductByNodeId(int nodeId);
        Product GetProductBySkuId(int skuId);
        string GetProductStatus(int skuid);        
        int GetAllocatedProductQuantityForUser(int productID, int userID);
        void UpdateAllocatedProductQuantityForUser(int productID, int userID, int quantity);
        List<CampaignsProduct> GetCampaignsProductSKUIDs(int campaignID);
        bool IsProductHasAllocation(int productID);
        OptionCategory GetOptionCategory(string codeName);        
        int GetCampaignProductIDBySKUID(int skuid);        
        CampaignsProduct GetCampaignProduct(int skuid);
        string GetProductImagePath(int productPageId);
        IEnumerable<ProductCategory> GetProductCategories(int skuid);
        ProductPricingInfo GetDefaultVariantPricing(int documentId, string uomLocalized);
        Product[] GetProductsByDocumentIds(int[] documentIds);

        Uri GetProductArtworkUri(int productPageId);
    }
}
