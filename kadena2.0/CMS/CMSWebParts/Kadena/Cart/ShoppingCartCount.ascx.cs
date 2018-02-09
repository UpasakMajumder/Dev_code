using CMS.DataEngine;
using CMS.DocumentEngine.Types.KDA;
using CMS.Ecommerce.Web.UI;
using CMS.EventLog;
using CMS.Helpers;
using Kadena.Old_App_Code.Kadena.Constants;
using Kadena.Old_App_Code.Kadena.Shoppingcart;
using Kadena.WebAPI.KenticoProviders.Contracts;
using Kadena2.Container.Default;
using System;
using System.Linq;

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
                var campaign = DIContainer.Resolve<IKenticoCampaignsProvider>();
                OpenCampaignID= campaign.GetOpenCampaignID();
                int userID = CurrentUser.UserID;
                linkCheckoutPage.HRef = URL;
                lblCartName.Text = CartDisplayName;
                var query = new DataQuery(SQLQueries.getShoppingCartCount);
                QueryDataParameters queryParams = new QueryDataParameters();
                queryParams.Add("@ShoppingCartUserID", userID);
                queryParams.Add("@ShoppingCartInventoryType", ShoppingCartInventoryType);
                queryParams.Add("@ShoppingCartCampaignID", OpenCampaignID);
                var countData = ConnectionHelper.ExecuteScalar(query.QueryText, queryParams, QueryTypeEnum.SQLQuery, true);
                var count = ValidationHelper.GetInteger(countData, default(int));
                lblCount.Text = count == 0 ? "" : $"{count}";
            }
            catch (Exception ex)
            {
                EventLogProvider.LogInformation("Kadena_CMSWebParts_Kadena_Cart_ShoppingCartCount", "Page_Load", ex.Message);
            }
        }
    }
}