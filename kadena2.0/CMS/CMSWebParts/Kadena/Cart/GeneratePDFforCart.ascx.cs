using CMS.DataEngine;
using CMS.DocumentEngine.Types.KDA;
using CMS.Ecommerce.Web.UI;
using CMS.EventLog;
using CMS.Helpers;
using Kadena.Old_App_Code.Kadena.PDFHelpers;
using Kadena.Old_App_Code.Kadena.Shoppingcart;
using System;
using System.Data;
using System.Linq;

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
        /// <summary>
        ///generate PDF  text
        /// </summary>
        public string TotalCartPDFButtonText
        {
            get
            {
                return ValidationHelper.GetString(ResHelper.GetString("Kadena.Product.TotalCartPDFButtonText"), string.Empty);
            }
            set
            {
                SetValue("TotalCartPDFButtonText", value);
            }
        }
        /// <summary>
        /// gets or sets open campaign
        /// </summary>
        public Campaign OpenCampaign
        {
            get
            {
                return ShoppingCartHelper.GetOpenCampaign();
            }
            set
            {
                SetValue("OpenCampaign", value);
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
                DataTable distributorCartData = CartPDFHelper.GetLoggedInUserCartData(InventoryType, CurrentUser.UserID,OpenCampaign?.CampaignID);
                var pdfBytes = CartPDFHelper.CreateProductPDF(distributorCartData, InventoryType);
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