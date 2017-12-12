using CMS.DataEngine;
using CMS.Ecommerce.Web.UI;
using CMS.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;

namespace Kadena.CMSWebParts.Kadena.Cart
{
    public partial class GeneratePDFforCart : CMSCheckoutWebPart
    {
        #region "Properties"

        /// <summary>
        /// Gets or sets the ProductName.
        /// </summary>
        public int InventoryType
        {
            get
            {
                return ValidationHelper.GetInteger(GetValue("InventoryType"), default(int));
            }
            set
            {
                SetValue("InventoryType", value);
            }
        }

        #endregion "Properties"

        #region "Page Events"
        /// <summary>
        /// PDF generation button click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkGeneratePDF_Click(object sender, EventArgs e)
        {
            DataTable distributorCartData = GetLoggedInUserCartData();
            CreateProductPDF(distributorCartData);
        }

        #endregion "Page Events"

        #region Methods
        /// <summary>
        /// Used to create PDF
        /// </summary>
        public void CreateProductPDF(DataTable distributorCartData)
        {
            var html = SettingsKeyInfoProvider.GetValue($@"{CurrentSiteName}.KDA_DistributorCartPDFHTML");
            var groupCart = distributorCartData.AsEnumerable().GroupBy(x => x["ShoppingCartID"]);
            var PDFBody = "";
            foreach (var cart in groupCart)
            {
                PDFBody += CreateCarOuterContent(cart.FirstOrDefault());
                var cartData = cart.ToList();
                PDFBody = PDFBody.Replace("{INNERCONTENT}", CreateCartInnerContent(cartData));
            }
            html = html.Replace("{OUTERCONTENT}", PDFBody);
            var pdfBytes = (new NReco.PdfGenerator.HtmlToPdfConverter()).GeneratePdf(html);
            string fileName = "test" + DateTime.Now.Ticks + ".pdf";
            Response.Clear();
            MemoryStream ms = new MemoryStream(pdfBytes);
            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", "attachment;filename=" + fileName);
            Response.Buffer = true;
            ms.WriteTo(Response.OutputStream);
            Response.End();
        }
        /// <summary>
        /// This will returns distributor cart items
        /// </summary>
        /// <returns></returns>
        private DataTable GetLoggedInUserCartData()
        {
            QueryDataParameters queryParams = new QueryDataParameters();
            queryParams.Add("@ShoppingCartInventoryType", InventoryType);
            var cartDataSet = ConnectionHelper.ExecuteQuery("Proc_Custom_LoggedInUSerCartData", queryParams, QueryTypeEnum.StoredProcedure, true);
            return cartDataSet.Tables[0];
        }
        /// <summary>
        /// This methods returns inner HTML for pdf
        /// </summary>
        /// <param name="distributorCartData"></param>
        /// <returns></returns>
        private string CreateCartInnerContent(List<DataRow> distributorCartData)
        {
            StringBuilder sb = new StringBuilder();
            foreach (DataRow row in distributorCartData)
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
        private string CreateCarOuterContent(DataRow distributor)
        {
            StringBuilder sb = new StringBuilder();
            string personData = $"{distributor["AddressPersonalName"].ToString()} | {distributor["AddressCity"].ToString()} | {distributor["StateDisplayName"].ToString()}";
            string pdfProductContent = SettingsKeyInfoProvider.GetValue($@"{CurrentSiteName}.KDA_DistributorCartPDFOuterBodyHTML");
            pdfProductContent = pdfProductContent.Replace("{DISTRIBUTORDETAILS}", personData);
            pdfProductContent = pdfProductContent.Replace("{PDFNAME}", distributor["AddressPersonalName"].ToString());
            sb.Append(pdfProductContent);
            return sb.ToString();
        }
        #endregion Methods
    }
}