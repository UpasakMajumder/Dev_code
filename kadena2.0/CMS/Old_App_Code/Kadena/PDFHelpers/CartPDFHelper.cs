using CMS.DataEngine;
using CMS.EventLog;
using CMS.SiteProvider;
using Kadena.Old_App_Code.Kadena.Constants;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace Kadena.Old_App_Code.Kadena.PDFHelpers
{
    public class CartPDFHelper
    {
        private const string _cartPDFFileName = "KDA_CartPDFFileName";

        #region Methods

        /// <summary>
        /// Create Pdf method
        /// </summary>
        /// <param name="distributorCartData"></param>
        public static byte[] CreateProductPDF(DataTable distributorCartData)
        {
            try
            {
                var html = SettingsKeyInfoProvider.GetValue($@"{SiteContext.CurrentSiteName}.KDA_DistributorCartPDFHTML");
                var groupCart = distributorCartData.AsEnumerable().GroupBy(x => x["ShoppingCartID"]);
                var PDFBody = "";
                foreach (var cart in groupCart)
                {
                    PDFBody += CreateCarOuterContent(cart.FirstOrDefault(), SiteContext.CurrentSiteName);
                    var cartData = cart.ToList();
                    PDFBody = PDFBody.Replace("{INNERCONTENT}", CreateCartInnerContent(cartData, SiteContext.CurrentSiteName));
                }
                html = html.Replace("{OUTERCONTENT}", PDFBody);
                return (new NReco.PdfGenerator.HtmlToPdfConverter()).GeneratePdf(html);
            }
            catch (Exception ex)
            {
                EventLogProvider.LogInformation("CartPDFHelper", "CreateProductPDF", ex.Message);
                return null;
            }
        }

        public static void WriteresponseToPDF(byte[] pdfBytes)
        {
            try
            {
                string fileName = $"{SettingsKeyInfoProvider.GetValue($"{SiteContext.CurrentSiteName}.{_cartPDFFileName}") }.pdf";
                HttpContext.Current.Response.Clear();
                MemoryStream ms = new MemoryStream(pdfBytes);
                HttpContext.Current.Response.ContentType = "application/pdf";
                HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=" + fileName);
                HttpContext.Current.Response.Buffer = true;
                ms.WriteTo(HttpContext.Current.Response.OutputStream);
                HttpContext.Current.Response.End();
            }
            catch (Exception ex)
            {
                EventLogProvider.LogInformation("CartPDFHelper", "WriteresponseToPDF", ex.Message);
            }
        }

        /// <summary>
        /// This will returns distributor cart items
        /// </summary>
        /// <returns></returns>
        public static DataTable GetDistributorCartData(int cartID, int inventoryType)
        {
            try
            {
                var query = new DataQuery(SQLQueries.distributorCartData);
                QueryDataParameters queryParams = new QueryDataParameters();
                queryParams.Add("@ShoppingCartUserID", cartID);
                queryParams.Add("@ShoppingCartInventoryType", inventoryType);
                var cartDataSet = ConnectionHelper.ExecuteQuery(query.QueryText, queryParams, QueryTypeEnum.SQLQuery, true);
                return cartDataSet.Tables[0];
            }
            catch (Exception ex)
            {
                EventLogProvider.LogInformation("CartPDFHelper", "GetDistributorCartData", ex.Message);
                return null;
            }
        }

        /// <summary>
        /// This will returns distributor cart items
        /// </summary>
        /// <returns></returns>
        public static DataTable GetLoggedInUserCartData(int inventoryType, int userID)
        {
            try
            {
                var query = new DataQuery(SQLQueries.loggedInUserCartData);
                QueryDataParameters queryParams = new QueryDataParameters();
                queryParams.Add("@ShoppingCartUserID", userID);
                queryParams.Add("@ShoppingCartInventoryType", inventoryType);
                var cartDataSet = ConnectionHelper.ExecuteQuery(query.QueryText, queryParams, QueryTypeEnum.SQLQuery, true);
                return cartDataSet.Tables[0];
            }
            catch (Exception ex)
            {
                EventLogProvider.LogInformation("CartPDFHelper", "GetLoggedInUserCartData", ex.Message);
                return null;
            }
        }

        /// <summary>
        /// This methods returns inner HTML for pdf
        /// </summary>
        /// <param name="distributorCartData"></param>
        /// <returns></returns>
        public static string CreateCartInnerContent(DataTable distributorCartData, string CurrentSiteName)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                foreach (DataRow row in distributorCartData.Rows)
                {
                    string pdfProductContent = SettingsKeyInfoProvider.GetValue($@"{CurrentSiteName}.KDA_DistributorCartPDFHTMLBody");
                    pdfProductContent = pdfProductContent.Replace("{PRODUCTNAME}", row["SKUName"].ToString());
                    pdfProductContent = pdfProductContent.Replace("{SKUNUMBER}", row["SKUNumber"].ToString());
                    pdfProductContent = pdfProductContent.Replace("{SKUUNITS}", row["SKUUnits"].ToString());
                    pdfProductContent = pdfProductContent.Replace("{SKUUNITSPRICE}", row["SKUUnitsPrice"].ToString());
                    pdfProductContent = pdfProductContent.Replace("{IMAGEURL}", row["SKUImagePath"].ToString());
                    sb.Append(pdfProductContent);
                }
                return sb.ToString();
            }
            catch (Exception ex)
            {
                EventLogProvider.LogInformation("CartPDFHelper", "CreateCartInnerContent", ex.Message);
                return string.Empty;
            }
        }

        /// <summary>
        /// This methods returns inner HTML for pdf
        /// </summary>
        /// <param name="distributorCartData"></param>
        /// <returns></returns>
        public static string CreateCartInnerContent(List<DataRow> distributorCartData, string CurrentSiteName)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                foreach (DataRow row in distributorCartData)
                {
                    string pdfProductContent = SettingsKeyInfoProvider.GetValue($@"{CurrentSiteName}.KDA_DistributorCartPDFHTMLBody");
                    pdfProductContent = pdfProductContent.Replace("{PRODUCTNAME}", row["SKUName"].ToString());
                    pdfProductContent = pdfProductContent.Replace("{SKUNUMBER}", row["SKUNumber"].ToString());
                    pdfProductContent = pdfProductContent.Replace("{SKUUNITS}", row["SKUUnits"].ToString());
                    pdfProductContent = pdfProductContent.Replace("{SKUUNITSPRICE}", row["SKUUnitsPrice"].ToString());
                    pdfProductContent = pdfProductContent.Replace("{IMAGEURL}", row["SKUImagePath"].ToString());
                    sb.Append(pdfProductContent);
                }
                return sb.ToString();
            }
            catch (Exception ex)
            {
                EventLogProvider.LogInformation("CartPDFHelper", "CreateCartInnerContent", ex.Message);
                return string.Empty;
            }
        }

        /// <summary>
        /// This methods returns Outer HTML for pdf
        /// </summary>
        /// <param name="distributorCartData"></param>
        /// <returns></returns>
        public static string CreateCarOuterContent(DataRow distributor, string CurrentSiteName)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                string personData = $"{distributor["AddressPersonalName"].ToString()} | {distributor["AddressCity"].ToString()} | {distributor["StateDisplayName"].ToString()}";
                string pdfProductContent = SettingsKeyInfoProvider.GetValue($@"{CurrentSiteName}.KDA_DistributorCartPDFOuterBodyHTML");
                pdfProductContent = pdfProductContent.Replace("{DISTRIBUTORDETAILS}", personData);
                pdfProductContent = pdfProductContent.Replace("{PDFNAME}", distributor["AddressPersonalName"].ToString());
                sb.Append(pdfProductContent);
                return sb.ToString();
            }
            catch (Exception ex)
            {
                EventLogProvider.LogInformation("CartPDFHelper", "CreateCarOuterContent", ex.Message);
                return string.Empty;
            }
        }

        #endregion Methods
    }
}