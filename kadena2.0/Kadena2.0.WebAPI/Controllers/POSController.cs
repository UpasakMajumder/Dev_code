using CMS.CustomTables;
using CMS.DataEngine;
using Kadena.WebAPI.Infrastructure;
using System.Web.Http;

namespace Kadena.WebAPI.Controllers
{
    public class POSController : ApiControllerBase
    {
        [HttpGet]
        [Route("api/pos/{posID}")]
        public void TogglePOSStatus(int posID)
        {
            string customTableClassName = "KDA.POSNumber";
            DataClassInfo brandTable = DataClassInfoProvider.GetDataClassInfo(customTableClassName);
            if (brandTable != null)
            {
                // Gets all data records from the POS table whose 'ItemId' field value equal to PosId
                CustomTableItem customTableData = CustomTableItemProvider.GetItem(posID, customTableClassName);
                if (customTableData != null)
                {
                    customTableData.SetValue("Enable", !customTableData.GetBooleanValue("Enable", false));
                    customTableData.Update();
                }
            }
        }
    }
}
