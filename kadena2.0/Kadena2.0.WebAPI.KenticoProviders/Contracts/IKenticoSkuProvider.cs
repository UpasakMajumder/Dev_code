using Kadena.Models.Product;
using System.Collections.Generic;

namespace Kadena.WebAPI.KenticoProviders.Contracts
{
    public interface IKenticoSkuProvider
    {
        Sku GetSKU(int skuId);
        /// <summary>
        /// Stores SKU fields which were made Mandatory by workaround - copying form Product in ProductEventHandler
        /// </summary>
        void UpdateSkuMandatoryFields(Sku sku);
        int GetSkuAvailableQty(int skuid);
        Price GetSkuPrice(int skuId);
        void SetSkuAvailableQty(string skunumber, int availableItems);
        Sku GetVariant(int skuId, IEnumerable<int> optionsIds);
        void SetSkuAvailableQty(int skuid, int qty);
    }
}
