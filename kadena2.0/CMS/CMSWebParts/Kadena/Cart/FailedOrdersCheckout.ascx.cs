using CMS.CustomTables;
using CMS.CustomTables.Types.KDA;
using CMS.DocumentEngine.Types.KDA;
using CMS.EventLog;
using CMS.Helpers;
using CMS.Membership;
using CMS.PortalEngine.Web.UI;
using CMS.Scheduler;
using Kadena.Old_App_Code.Kadena.Constants;
using Kadena.Old_App_Code.Kadena.EmailNotifications;
using System;
using System.Linq;

namespace Kadena.CMSWebParts.Kadena.Cart
{
    public partial class FailedOrdersCheckout : CMSAbstractWebPart
    {
         public int CampaignID { get; set; }
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
                if (AuthenticationHelper.IsAuthenticated())
                {
                    int CampaignID = QueryHelper.GetInteger("campid", 0);
                    if (CampaignID == 0)
                    {
                        lnkCheckout.Visible = false;
                    }
                    else if(GetCampaignOrderStatus(CampaignID))
                    {
                        lnkCheckout.Enabled = false;
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
                    if (CampaignID > 0 && !GetCampaignOrderStatus(CampaignID))
                    {
                        ProcessOrders(CampaignID);
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
        /// <summary>
        /// Closing the campaign
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ProcessOrders(int campaignID)
        {
            try
            {
                Campaign campaign = CampaignProvider.GetCampaigns()
                    .WhereEquals("NodeSiteID", CurrentSite.SiteID)
                    .WhereEquals("CampaignID", campaignID)
                    .FirstObject;
                if (campaign != null)
                {
                    UpdateCampaignOrderStatus(campaign.CampaignID);
                    TaskInfo runTask = TaskInfoProvider.GetTaskInfo(ScheduledTaskNames.PrebuyOrderCreation, CurrentSite.SiteID);
                    if (runTask != null)
                    {
                        runTask.TaskRunInSeparateThread = true;
                        runTask.TaskEnabled = true;
                        runTask.TaskData = $"{campaign.CampaignID}|{CurrentUser.UserID}";
                        SchedulingExecutor.ExecuteTask(runTask);
                    }
                    var users = UserInfoProvider.GetUsers();
                    if (users != null)
                    {
                        foreach (var user in users)
                        {
                            ProductEmailNotifications.CampaignEmail(campaign.DocumentName, user.Email, "CampaignCloseEmail");
                        }
                    }
                    Response.Redirect(Request.RawUrl);
                }
            }
            catch (Exception ex)
            {
                EventLogProvider.LogException("Kadena_CMSWebParts_Kadena_Cart_FailedOrdersCheckout", "ProcessOrders", ex, CurrentSite.SiteID, ex.Message);
            }
        }
        /// <summary>
        /// Inserts orders processing Campaign id
        /// </summary>
        /// <param name="campaignID"></param>
        private void UpdateCampaignOrderStatus(int campaignID)
        {
            try
            {
                var foData = CustomTableItemProvider.GetItems<FailedOrdersItem>().WhereEquals("CapmaignID", campaignID).FirstOrDefault();
                if (!DataHelper.DataSourceIsEmpty(foData))
                {
                    foData.IsCampaignOrdersInProgress = true;
                    foData.IsCampaignOrdersFailed = false;
                    foData.Update();
                }
            }
            catch (Exception ex)
            {
                EventLogProvider.LogException("CMSWebParts_Kadena_Campaign_Web_Form_CampaignWebFormActions", "UpdateCampaignOrderStatus", ex, CurrentSite.SiteID, ex.Message);
            }
        }
        /// <summary>
        /// Inserts orders processing Campaign id
        /// </summary>
        /// <param name="campaignID"></param>
        private bool GetCampaignOrderStatus(int campaignID)
        {
            try
            {
                var foData = CustomTableItemProvider.GetItems<FailedOrdersItem>().WhereEquals("CapmaignID", campaignID).FirstOrDefault();
                return foData!=null? foData.IsCampaignOrdersInProgress:false;
            }
            catch (Exception ex)
            {
                EventLogProvider.LogException("CMSWebParts_Kadena_Campaign_Web_Form_CampaignWebFormActions", "InsertCampaignOrdersInProgress", ex, CurrentSite.SiteID, ex.Message);
                return false;
            }
        }
    }
}