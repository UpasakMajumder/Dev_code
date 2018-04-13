using Kadena.Models.Product;
using System.Collections.Generic;

namespace Kadena.BusinessLogic.Contracts
{
    public interface IProductsService
    {
        ProductsPage GetProducts(string path);
        Price GetPrice(int skuId, Dictionary<string, int> skuOptions = null);
        string GetAvailableProductsString(string productType, int? numberOfAvailableProducts, string cultureCode, int numberOfStockProducts, string unitOfMeasure);
        string GetInventoryProductAvailability(string productType, int? numberOfAvailableProducts, int numberOfStockProducts);
        bool CanDisplayAddToCartButton(string productType, int? numberOfAvailableProducts, bool sellOnlyIfAvailable);
        string GetPackagingString(int numberOfItemsInPackage, string unitOfMeasure, string cultureCode);
        string GetUnitOfMeasure(string unitOfMeasure, string cultureCode);
    }
}