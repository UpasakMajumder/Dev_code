using CMS.CustomTables;
using Kadena.WebAPI.KenticoProviders.Contracts;

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
    }
}
