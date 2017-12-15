using CMS.DataEngine;
using CMS.Ecommerce.Web.UI;
using CMS.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using System.Drawing.Printing;
using Kadena.Old_App_Code.Kadena.Constants;
using Kadena.Old_App_Code.Kadena.PDFHelpers;
using CMS.EventLog;

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
        #endregion
        #region "Page Events"
        /// <summary>
        /// button click event
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
                EventLogProvider.LogInformation("CartPDFHelper", "CreateCarOuterContent", ex.Message);
            }
        }
        #endregion
        #region Methods
        /// <summary>
        /// Create Pdf method
        /// </summary>
        /// <param name="distributorCartData"></param>
        public void CreateProductPDF(DataTable distributorCartData)
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
                html = html.Replace("{PDFNAME}", $"{CurrentUser.FullName}");
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
                EventLogProvider.LogInformation("CartPDFHelper", "CreateCarOuterContent", ex.Message);
            }
        }
        #endregion
    }
}