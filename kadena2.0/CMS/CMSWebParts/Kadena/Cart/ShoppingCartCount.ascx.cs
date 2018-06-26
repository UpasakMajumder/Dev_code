using CMS.DataEngine;
using CMS.DocumentEngine.Types.KDA;
using CMS.Ecommerce.Web.UI;
using CMS.EventLog;
using CMS.Helpers;
using Kadena.Old_App_Code.Kadena.Constants;
using Kadena.Old_App_Code.Kadena.Shoppingcart;
using Kadena.WebAPI.KenticoProviders.Contracts;
using Kadena.Container.Default;
using System;
using System.Linq;
using Kadena.Models.ShoppingCarts;

namespace Kadena.CMSWebParts.Kadena.Cart
{
    public partial class ShoppingCartCount : CMSCheckoutWebPart
    {
        #region properties

        /// <summary>
        /// Distributor Cart ID.
        /// </summary>
        public int ShoppingCartInventoryType
        {
            get
            {
                return ValidationHelper.GetInteger(GetValue("ShoppingCartInventoryType"), default(int));
            }
            set
            {
                SetValue("ShoppingCartInventoryType", value);
            }
        }

        /// <summary>
        /// Distributor Cart ID.
        /// </summary>
        public string URL
        {
            get
            {
                return ValidationHelper.GetString(GetValue("URL"), string.Empty);
            }
            set
            {
                SetValue("URL", value);
            }
        }

        /// <summary>
        /// Distributor Cart ID.
        /// </summary>
        public string CartDisplayName
        {
            get
            {
                return ValidationHelper.GetString(GetValue("CartDisplayName"), string.Empty);
            }
            set
            {
                SetValue("CartDisplayName", value);
            }
        }
       
        /// <summary>
        /// Get current open campaign
        /// </summary>
        public int  OpenCampaignID { get; set; }
        #endregion properties

        /// <summary>
        /// page load event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                var campaignProvider = DIContainer.Resolve<IKenticoCampaignsProvider>();
                OpenCampaignID = campaignProvider.GetOpenCampaignID();
                linkCheckoutPage.HRef = URL;
                lblCartName.Text = CartDisplayName;

                var shoppingCartProvider = DIContainer.Resolve<IShoppingCartProvider>();
                var count = shoppingCartProvider.GetDistributorCartCount(CurrentUser.UserID, OpenCampaignID, (ShoppingCartTypes)ShoppingCartInventoryType);
                lblCount.Text = count == 0 
                    ? "" 
                    : $"{count}";
            }
            catch (Exception ex)
            {
                EventLogProvider.LogInformation("Kadena_CMSWebParts_Kadena_Cart_ShoppingCartCount", "Page_Load", ex.Message);
            }
        }
    }
}