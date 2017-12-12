using CMS.DataEngine;
using CMS.Ecommerce.Web.UI;
using CMS.EventLog;
using CMS.Helpers;
using System;

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
                int userID = CurrentUser.UserID;
                QueryDataParameters queryParams = new QueryDataParameters();
                queryParams.Add("@ShoppingCartUserID", userID);
                queryParams.Add("@ShoppingCartInventoryType", ShoppingCartInventoryType);
                var countData = ConnectionHelper.ExecuteScalar("Proc_Custom_GetShoppingCartCount", queryParams, QueryTypeEnum.StoredProcedure, true);
                var count = ValidationHelper.GetInteger(countData, default(int));
                linkCheckoutPage.HRef = URL;
                lblCartName.Text = CartDisplayName;
                lblCount.Text = count == 0 ? "" : $"{count}";
            }
            catch (Exception ex)
            {
                EventLogProvider.LogInformation("Kadena_CMSWebParts_Kadena_Cart_ShoppingCartCount", "Page_Load", ex.Message);
            }
        }
    }
}