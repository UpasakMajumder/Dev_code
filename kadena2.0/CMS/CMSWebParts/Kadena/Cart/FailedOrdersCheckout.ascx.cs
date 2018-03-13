using CMS.EventLog;
using CMS.Helpers;
using CMS.Membership;
using CMS.PortalEngine.Web.UI;
using Kadena.Old_App_Code.Kadena.Shoppingcart;
using Kadena.WebAPI.KenticoProviders.Contracts;
using Kadena.Container.Default;
using System;

namespace Kadena.CMSWebParts.Kadena.Cart
{
    public partial class FailedOrdersCheckout : CMSAbstractWebPart
    {
         public int CampaignID { get; set; }
        private IFailedOrderStatusProvider _failedOrders;
        /// <summary>
        /// Checkout button text
        /// </summary>
        public string CheckoutButtonText
        {
            get
            {
                return ResHelper.GetString("KDA.CartCheckout.CheckoutButtonText");
            }
            set
            {
                SetValue("CheckoutButtonText", value);
            }
        }
        /// <summary>
        /// Page load event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                _failedOrders = DIContainer.Resolve<IFailedOrderStatusProvider>();
                if (AuthenticationHelper.IsAuthenticated())
                {
                    int CampaignID = QueryHelper.GetInteger("campid", 0);
                    if (CampaignID == 0)
                    {
                        lnkCheckout.Visible = false;
                    }
                    else if(_failedOrders.GetCampaignOrderStatus(CampaignID))
                    {
                        lnkCheckout.Enabled = true;
                    }
                }
            }
            catch (Exception ex)
            {
                EventLogProvider.LogInformation("Kadena_CMSWebParts_Kadena_Cart_FailedOrdersCheckout", "Page_Load", ex.Message);
            }
        }
        /// <summary>
        /// Chekcou click event for order processing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkCheckout_Click(object sender, EventArgs e)
        {
            try
            {
                if (AuthenticationHelper.IsAuthenticated() )
                {
                    int CampaignID = QueryHelper.GetInteger("campid", 0);
                    if (CampaignID > 0 && _failedOrders.GetCampaignOrderStatus(CampaignID))
                    {
                        ShoppingCartHelper.ProcessOrders(CampaignID);
                        Response.Redirect(Request.RawUrl);
                    }
                    else
                    {
                        lnkCheckout.Visible = false;
                    }
                }
            }
            catch (Exception ex)
            {
                EventLogProvider.LogInformation("Kadena_CMSWebParts_Kadena_Cart_FailedOrdersCheckout", "lnkCheckout_Click", ex.Message);
            }
        }
      
    }
}