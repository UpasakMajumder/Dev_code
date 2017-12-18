using CMS.Ecommerce.Web.UI;
using CMS.EventLog;
using CMS.Helpers;
using Kadena.Old_App_Code.Kadena.PDFHelpers;
using System;
using System.Data;

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
        /// button click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkGeneratePDF_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable distributorCartData = CartPDFHelper.GetLoggedInUserCartData(InventoryType, CurrentUser.UserID);
                var pdfBytes = CartPDFHelper.CreateProductPDF(distributorCartData);
                CartPDFHelper.WriteresponseToPDF(pdfBytes);
            }
            catch (Exception ex)
            {
                EventLogProvider.LogInformation("GeneratePDFforCart", "lnkGeneratePDF_Click", ex.Message);
            }
        }

        #endregion "Page Events"
    }
}