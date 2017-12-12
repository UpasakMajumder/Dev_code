using CMS.DataEngine;
using Kadena.Old_App_Code.Kadena.Constants;
using System.Data;
using System.Text;

namespace Kadena.Old_App_Code.Kadena.PDFHelpers
{
    public class CartPDFHelper
    {
        /// <summary>
        /// This will returns distributor cart items
        /// </summary>
        /// <returns></returns>
        public DataTable GetDistributorCartData(int cartID, string inventoryType)
        {
            QueryDataParameters queryParams = new QueryDataParameters();
            queryParams.Add("@ShoppingCartUserID", cartID);
            queryParams.Add("@ShoppingCartInventoryType", inventoryType);
            var cartDataSet = ConnectionHelper.ExecuteQuery(StoredProcedures.distributorCartData, queryParams, QueryTypeEnum.StoredProcedure, true);
            return cartDataSet.Tables[0];
        }

        /// <summary>
        /// This will returns distributor cart items
        /// </summary>
        /// <returns></returns>
        public DataTable GetLoggedInUserCartData(string inventoryType)
        {
            QueryDataParameters queryParams = new QueryDataParameters();
            queryParams.Add("@ShoppingCartInventoryType", inventoryType);
            var cartDataSet = ConnectionHelper.ExecuteQuery(StoredProcedures.loggedInUserCartData, queryParams, QueryTypeEnum.StoredProcedure, true);
            return cartDataSet.Tables[0];
        }

        /// <summary>
        /// This methods returns inner HTML for pdf
        /// </summary>
        /// <param name="distributorCartData"></param>
        /// <returns></returns>
        public string CreateCartInnerContent(DataTable distributorCartData, string CurrentSiteName)
        {
            StringBuilder sb = new StringBuilder();
            foreach (DataRow row in distributorCartData.Rows)
            {
                string pdfProductContent = SettingsKeyInfoProvider.GetValue($@"{CurrentSiteName}.KDA_DistributorCartPDFHTMLBody");
                pdfProductContent = pdfProductContent.Replace("{PRODUCTNAME}", row["SKUName"].ToString());
                pdfProductContent = pdfProductContent.Replace("{SKUNUMBER}", row["SKUNumber"].ToString());
                pdfProductContent = pdfProductContent.Replace("{SKUUNITS}", row["SKUUnits"].ToString());
                pdfProductContent = pdfProductContent.Replace("{SKUUNITSPRICE}", row["SKUUnitsPrice"].ToString());
                sb.Append(pdfProductContent);
            }
            return sb.ToString();
        }

        /// <summary>
        /// This methods returns Outer HTML for pdf
        /// </summary>
        /// <param name="distributorCartData"></param>
        /// <returns></returns>
        public string CreateCarOuterContent(DataRow distributor, string CurrentSiteName)
        {
            StringBuilder sb = new StringBuilder();
            string personData = $"{distributor["AddressPersonalName"].ToString()} | {distributor["AddressCity"].ToString()} | {distributor["StateDisplayName"].ToString()}";
            string pdfProductContent = SettingsKeyInfoProvider.GetValue($@"{CurrentSiteName}.KDA_DistributorCartPDFOuterBodyHTML");
            pdfProductContent = pdfProductContent.Replace("{DISTRIBUTORDETAILS}", personData);
            pdfProductContent = pdfProductContent.Replace("{PDFNAME}", distributor["AddressPersonalName"].ToString());
            sb.Append(pdfProductContent);
            return sb.ToString();
        }
    }
}