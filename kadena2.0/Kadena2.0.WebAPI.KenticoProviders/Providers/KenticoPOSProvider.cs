using CMS.CustomTables;
using CMS.Ecommerce;
using Kadena.WebAPI.KenticoProviders.Contracts;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Kadena.WebAPI.KenticoProviders
{
    public class KenticoPOSProvider : IKenticoPOSProvider
    {
        private readonly string CustomTableName = "KDA.POSNumber";

        public void TogglePOSStatus(int posID)
        {
            CustomTableItem posItem = CustomTableItemProvider.GetItem(posID, CustomTableName);
            if (posItem != null)
            {
                posItem.SetValue("Enable", !posItem.GetBooleanValue("Enable", false));
                posItem.Update();
            }
        }
        public bool DeletePOS(int posID)
        {
            bool isDeleted = false;
            CustomTableItem posItem = CustomTableItemProvider.GetItem(posID, CustomTableName);
            if (posItem != null)
            {
               var isProductsExist= SKUInfoProvider.GetSKUs().WhereEquals("SKUNumber", posItem.GetStringValue("POSNumber", string.Empty)).Any();
                if (!isProductsExist)
                {
                    posItem.SetValue("Enable", false);
                    posItem.Update();
                    isDeleted = true;
                }
            }
            return isDeleted;
        }
    }
}
