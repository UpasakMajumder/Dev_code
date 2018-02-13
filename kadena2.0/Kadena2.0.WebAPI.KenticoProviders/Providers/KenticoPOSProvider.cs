using CMS.CustomTables;
using CMS.Ecommerce;
using Kadena.WebAPI.KenticoProviders.Contracts;
using System.Linq;

namespace Kadena.WebAPI.KenticoProviders
{
    public class KenticoPOSProvider : IKenticoPOSProvider
    {
        private readonly string CustomTableName = "KDA.POSNumber";
        public bool DeletePOS(int posID)
        {
            bool isDeleted = false;
            CustomTableItem posItem = CustomTableItemProvider.GetItem(posID, CustomTableName);
            if (posItem != null)
            {
                var isProductsExist = SKUInfoProvider.GetSKUs().WhereEquals("SKUProductCustomerReferenceNumber", posItem.GetStringValue("POSNumber", string.Empty)).Any();
                if (!isProductsExist)
                {
                    posItem.Delete();
                    isDeleted = true;
                }
            }
            return isDeleted;
        }
    }
}
