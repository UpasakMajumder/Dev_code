using CMS.CustomTables;
using CMS.CustomTables.Types.KDA;
using CMS.DataEngine;
using CMS.DocumentEngine.Types.KDA;
using CMS.Ecommerce;
using CMS.EventLog;
using CMS.Helpers;
using CMS.SiteProvider;
using Kadena.Models.Common;
using Kadena.Old_App_Code.Kadena.Constants;
using Kadena.Old_App_Code.Kadena.Enums;
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
        public static byte[] CreateProductPDF(DataTable distributorCartData, int inventoryType)
        {
            try
            {
                var html = SettingsKeyInfoProvider.GetValue($@"{SiteContext.CurrentSiteName}.KDA_DistributorCartPDFHTML");
                var groupCart = distributorCartData.AsEnumerable().GroupBy(x => x["ShoppingCartID"]).ToList();
                var PDFBody = "";
                groupCart.ForEach(cart =>
                {
                    PDFBody += CreateCarOuterContent(cart.FirstOrDefault(), SiteContext.CurrentSiteName);
                    var cartData = cart.ToList();
                    PDFBody = PDFBody.Replace("{INNERCONTENT}", CreateCartInnerContent(cartData, SiteContext.CurrentSiteName, inventoryType));
                });
                html = html.Replace("{OUTERCONTENT}", PDFBody);
                NReco.PdfGenerator.HtmlToPdfConverter PDFConverter = new NReco.PdfGenerator.HtmlToPdfConverter();
                PDFConverter.License.SetLicenseKey(SettingsKeyInfoProvider.GetValue("KDA_NRecoOwner", SiteContext.CurrentSiteID), SettingsKeyInfoProvider.GetValue("KDA_NRecoKey", SiteContext.CurrentSiteID));
                PDFConverter.LowQuality = SettingsKeyInfoProvider.GetBoolValue("KDA_NRecoLowQuality", SiteContext.CurrentSiteID);
                return PDFConverter.GeneratePdf(html);
            }
            catch (Exception ex)
            {
                EventLogProvider.LogInformation("CartPDFHelper", "CreateProductPDF", ex.Message);
                return null;
            }
        }
        /// <summary>
        /// Writing response to pdf
        /// </summary>
        /// <param name="pdfBytes"></param>
        public static void WriteresponseToPDF(byte[] pdfBytes)
        {
            try
            {
                string fileName = $"{SettingsKeyInfoProvider.GetValue($"{SiteContext.CurrentSiteName}.{_cartPDFFileName}") }.pdf";
                HttpContext.Current.Response.Clear();
                MemoryStream ms = new MemoryStream(pdfBytes);
                HttpContext.Current.Response.ContentType = ContentTypes.Pdf;
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
        public static DataTable GetDistributorCartData(int cartID, int inventoryType,int? campaignID)
        {
            try
            {
                var query = new DataQuery(SQLQueries.distributorCartData);
                QueryDataParameters queryParams = new QueryDataParameters();
                queryParams.Add("@ShoppingCartUserID", cartID);
                queryParams.Add("@ShoppingCartInventoryType", inventoryType);
                queryParams.Add("@ShoppingCartCampaignID", campaignID);
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
        public static DataTable GetLoggedInUserCartData(int inventoryType, int userID,int? campaignID)
        {
            try
            {
                var query = new DataQuery(SQLQueries.loggedInUserCartData);
                QueryDataParameters queryParams = new QueryDataParameters();
                queryParams.Add("@ShoppingCartUserID", userID);
                queryParams.Add("@ShoppingCartInventoryType", inventoryType);
                queryParams.Add("@ShoppingCartCampaignID", campaignID);
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
        public static string CreateCartInnerContent(List<DataRow> distributorCartData, string CurrentSiteName, int inventoryType)
        {
            try
            {
                string pdfProductContent = string.Empty;
                if (inventoryType == Convert.ToInt32(ProductType.GeneralInventory))
                {
                    pdfProductContent = SettingsKeyInfoProvider.GetValue($@"{CurrentSiteName}.KDA_DistributorCartPDFHTMLBodyGI");
                }
                else
                {
                    pdfProductContent = SettingsKeyInfoProvider.GetValue($@"{CurrentSiteName}.KDA_DistributorCartPDFHTMLBody");
                }
                StringBuilder sb = new StringBuilder();
                var skuIds = distributorCartData.AsEnumerable().Select(x => x.Field<int>("SkUID")).ToList();
                var products = CampaignsProductProvider.GetCampaignsProducts()
                    .WhereEquals("NodeSiteID", SiteContext.CurrentSiteID).WhereIn("NodeSKUID", skuIds).Columns("NodeSKUID,State,ProgramID,QtyPerPack,EstimatedPrice").ToList();
                var programs = ProgramProvider.GetPrograms().WhereIn("ProgramID", products.Select(x => x.ProgramID).ToList()).Columns("ProgramID,ProgramName").ToList();
                var stateGroups = CustomTableItemProvider.GetItems<StatesGroupItem>().WhereIn("ItemID", products.Select(x => x.State).ToList()).Columns("ItemID,States").ToList();
                distributorCartData.ForEach(row =>
                {
                    var pdfContent = pdfProductContent;
                    var product = products.Where(x => x.NodeSKUID == ValidationHelper.GetInteger(row["SKUID"], default(int))).FirstOrDefault();
                    var programName = programs.Where(x => x.ProgramID == product.ProgramID).FirstOrDefault();
                    var states = stateGroups.Where(x => x.ItemID == product.State).FirstOrDefault();
                    var skuValidity = ValidationHelper.GetDateTime(row["SKUValidUntil"], default(DateTime));
                    pdfContent = pdfContent.Replace("{PRODUCTNAME}", ValidationHelper.GetString(row["SKUName"], "&nbsp"))
                                           .Replace("{SKUNUMBER}", ValidationHelper.GetString(row["SKUProductCustomerReferenceNumber"], "&nbsp"))
                                           .Replace("{SKUUNITS}", ValidationHelper.GetString(row["SKUUnits"], "&nbsp"))
                                           .Replace("{BUNDLECOST}", inventoryType == Convert.ToInt32(ProductType.GeneralInventory) ? ($"{CurrencyInfoProvider.GetFormattedPrice(ValidationHelper.GetDouble(row["SKUPrice"], default(double)), SiteContext.CurrentSiteID, true)}"): ($"{CurrencyInfoProvider.GetFormattedPrice(ValidationHelper.GetDouble(product.EstimatedPrice, default(double)), SiteContext.CurrentSiteID, true)}"))
                                           .Replace("{BUNDLEQUANTITY}", ValidationHelper.GetString(product.QtyPerPack, "&nbsp"))
                                           .Replace("{IMAGEURL}", GetProductImage(ValidationHelper.GetString(row["SKUImagePath"], default(string))))
                                           .Replace("{VALIDSTATES}", ValidationHelper.GetString(states?.States, "&nbsp"))
                                           .Replace("{EXPIREDATE}", skuValidity != default(DateTime) ? skuValidity.ToString("MMM dd, yyyy") : "&nbsp")
                                           .Replace("{PROGRAMNAME}", ValidationHelper.GetString(programName?.ProgramName, "&nbsp") );
                    sb.Append(pdfContent);
                });
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
                string personData = $"{distributor["AddressPersonalName"].ToString()} | {distributor["AddressCity"].ToString()} | {distributor["StateDisplayName"].ToString()} | {distributor["AddressZip"].ToString()}";
                string pdfProductContent = SettingsKeyInfoProvider.GetValue($@"{CurrentSiteName}.KDA_DistributorCartPDFOuterBodyHTML");
                pdfProductContent = pdfProductContent.Replace("{DISTRIBUTORDETAILS}", personData)
                                                     .Replace("{PDFNAME}", distributor["AddressPersonalName"].ToString());
                sb.Append(pdfProductContent);
                return sb.ToString();
            }
            catch (Exception ex)
            {
                EventLogProvider.LogInformation("CartPDFHelper", "CreateCarOuterContent", ex.Message);
                return string.Empty;
            }
        }
        /// <summary>
        /// Get product Image by Image path
        /// </summary>
        /// <param name="imagepath"></param>
        /// <returns></returns>
        public static string GetProductImage(string imagepath)
        {
            string returnValue = imagepath;
            try
            {
                if (!string.IsNullOrWhiteSpace(imagepath) && !imagepath.Contains(SiteContext.CurrentSite.DomainName))
                {
                    string strPathAndQuery = HttpContext.Current.Request.Url.PathAndQuery;
                    returnValue = HttpContext.Current.Request.Url.AbsoluteUri.Replace(strPathAndQuery, "") + imagepath.Trim('~');
                }
            }
            catch (Exception ex)
            {
                EventLogProvider.LogException("Get Product Image", "GetProductImage", ex, SiteContext.CurrentSiteID, ex.Message);
            }
            return string.IsNullOrEmpty(returnValue) ? SettingsKeyInfoProvider.GetValue($@"{SiteContext.CurrentSiteName}.KDA_ProductsPlaceHolderImage") : returnValue;
        }
        #endregion Methods
    }
}