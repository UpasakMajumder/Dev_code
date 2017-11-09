using CMS.CustomTables;
using CMS.CustomTables.Types.KDA;
using CMS.DataEngine;
using CMS.Ecommerce;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;

namespace CMSApp
{
    /// <summary>
    /// Summary description for DeleteAddressWebService
    /// </summary>
    [WebService]
    [ScriptService]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class DeleteAddressWebService : System.Web.Services.WebService
    {

        [WebMethod]
        [ScriptMethod]
        public bool DeleteAddress(int addressID)
        {
            bool returnValue = false;
            var address = AddressInfoProvider.GetAddressInfo(addressID);
            if (address != null)
            {
                AddressInfoProvider.DeleteAddressInfo(addressID);
                //Delete data from Shipping Table..
                ShippingAddressItem shippingAddress = new ShippingAddressItem();
                string customTableClassName = shippingAddress.ClassName;
                DataClassInfo customTable = DataClassInfoProvider.GetDataClassInfo(customTableClassName);
                if (customTable != null)
                {
                    CustomTableItemProvider.DeleteItems(customTableClassName, "COM_AddressID =" + addressID);
                    returnValue = true;
                }
            }

            return returnValue;
        }
    }
}
