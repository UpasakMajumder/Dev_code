using CMS.DataEngine;
using CMS.Ecommerce.Web.UI;
using CMS.EventLog;
using CMS.Helpers;
using Kadena.Old_App_Code.Kadena.PDFHelpers;
using System;
using System.Data;
using System.IO;
using System.Linq;
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
            try
            {
                DataTable distributorCartData = CartPDFHelper.GetLoggedInUserCartData(InventoryType, CurrentUser.UserID);
                CreateProductPDF(distributorCartData);
            }
            catch (Exception ex)
            {
                EventLogProvider.LogInformation("GeneratePDFforCart", "lnkGeneratePDF_Click", ex.Message);
            }
        }

        #endregion "Page Events"

        #region Methods

        /// <summary>
        /// Used to create PDF
        /// </summary>
        private void CreateProductPDF(DataTable distributorCartData)
        {
            try
            {
                var html = SettingsKeyInfoProvider.GetValue($@"{CurrentSiteName}.KDA_DistributorCartPDFHTML");
                var groupCart = distributorCartData.AsEnumerable().GroupBy(x => x["ShoppingCartID"]);
                var PDFBody = "";
                foreach (var cart in groupCart)
                {
                    PDFBody += CartPDFHelper.CreateCarOuterContent(cart.FirstOrDefault(), CurrentSiteName);
                    var cartData = cart.ToList();
                    PDFBody = PDFBody.Replace("{INNERCONTENT}", CartPDFHelper.CreateCartInnerContent(cartData, CurrentSiteName));
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
            catch (Exception ex)
            {
                EventLogProvider.LogInformation("GeneratePDFforCart", "CreateProductPDF", ex.Message);
            }
        }

        #endregion Methods
    }
}