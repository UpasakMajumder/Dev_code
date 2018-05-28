using Kadena.Models.Product;
using System.Collections.Generic;

namespace Kadena.BusinessLogic.Contracts
{
    public interface IProductsService
    {
        ProductsPage GetProducts(string path);
        Price GetPrice(int skuId, Dictionary<string, int> skuOptions = null);
        bool CanDisplayAddToCartButton(string productType, int? numberOfAvailableProducts, bool sellOnlyIfAvailable);
        string GetPackagingString(int numberOfItemsInPackage, string unitOfMeasure, string cultureCode);
        string GetUnitOfMeasure(string unitOfMeasure, string cultureCode);
        string TranslateUnitOfMeasure(string unitOfMeasure, string cultureCode);
        IEnumerable<ProductEstimation> GetProductEstimations(int documentId);
        IEnumerable<ProductPricingInfo> GetProductPricings(int documentId, string pricingModel, string unitOfMeasure, string cultureCode);
        ProductAvailability GetInventoryProductAvailability(string productType, int? numberOfAvailableProducts, string cultureCode, int numberOfStockProducts, string unitOfMeasureCode);
        string GetMinMaxItemsString(int min, int max);
    }
}