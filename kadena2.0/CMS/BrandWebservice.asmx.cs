using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

using System.Web.Script.Services;
using CMS.CustomTables.Types.KDA;
using CMS.CustomTables;
using CMS.EventLog;


namespace CMSApp
{
    /// <summary>
    /// Summary description for BrandWebservice
    /// </summary>
    [WebService]
    [ScriptService]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class BrandWebservice : System.Web.Services.WebService
    {

        [WebMethod]
        [ScriptMethod]

        public bool DeleteBrand(int itemID)
        {
            var flag = false;

            try
            {
                BrandItem objBrand = new BrandItem();
                objBrand = CustomTableItemProvider.GetItems<BrandItem>().WhereEquals("ItemID", itemID);
                flag = objBrand.Delete();
            }
            catch (Exception ex)
            {
                EventLogProvider.LogException("Webservice.asmx.cs", "DeleteBrand()", ex);
            }
            return flag;
        }
    }
}
