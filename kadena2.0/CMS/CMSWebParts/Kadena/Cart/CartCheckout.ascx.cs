using CMS.Ecommerce;
using CMS.EventLog;
using CMS.Helpers;
using CMS.PortalEngine.Web.UI;
using Kadena.Dto.General;
using Kadena.Dto.SubmitOrder.MicroserviceRequests;
using Kadena.Old_App_Code.Kadena.Constants;
using Kadena.Old_App_Code.Kadena.Enums;
using Kadena.Old_App_Code.Kadena.Shoppingcart;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI.WebControls;

namespace Kadena.CMSWebParts.Kadena.Cart
{
    public partial class CartCheckout : CMSAbstractWebPart
    {
        private const string _serviceUrlSettingKey = "KDA_OrderServiceEndpoint";
        #region properties
        private ShoppingCartInfo Cart { get; set; }
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
        /// popup close button text
        /// </summary>
        public string PopupCloseButtonText
        {
            get
            {
                return ResHelper.GetString("KDA.CartCheckout.PopupCloseButtonText");
            }
            set
            {
                SetValue("PopupCloseButtonText", value);
            }
        }
        #endregion
        #region Events
        /// <summary>
        /// Page load event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            divErrorDailogue.Attributes.Add("class", "dialog");
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
                var loggedInUserCartIDs = ShoppingCartHelper.GetLoggeedInUserCarts(CurrentUser.UserID, ProductType.GeneralInventory);
                var unprocessedDistributorIDs = new List<int>();
                loggedInUserCartIDs.ForEach(care =>
                {
                    Cart = ShoppingCartInfoProvider.GetShoppingCartInfo(care);
                    var response = ProcessOrder();
                    if (response != null && response.Success)
                    {
                        ShoppingCartInfoProvider.DeleteShoppingCartInfo(Cart);
                    }
                    else
                    {
                        unprocessedDistributorIDs.Add(Cart.GetIntegerValue("ShoppingCartDistributorID", default(int)));
                    }
                });
                if (unprocessedDistributorIDs.Count == 0)
                {
                    lblCartUpdateSuccess.Text = ResHelper.GetString("KDA.Checkout.OrderSuccess");
                }
                else
                {
                    var distributors = AddressInfoProvider.GetAddresses().WhereIn("AddressID", unprocessedDistributorIDs).Column("AddressPersonalName").ToList().Select(x => x?.AddressPersonalName).ToList();
                    ShowOrderErrorList(distributors);
                }
                divErrorDailogue.Attributes.Add("class", "dialog active");
            }
            catch (Exception ex)
            {
                EventLogProvider.LogInformation("Kadena_CMSWebParts_Kadena_Cart_CartCheckout", "lnkCheckout_Click", ex.Message);
            }
        }
        #endregion Events

        #region Methods

        /// <summary>
        ///Processes order and returns response
        /// </summary>
        /// <returns></returns>
        private BaseResponseDto<string> ProcessOrder()
        {
            try
            {
                OrderDTO Ordersdto = ShoppingCartHelper.CreateOrdersDTO(Cart, CurrentUser.UserID, OrderType.generalInventory);
                var response = ShoppingCartHelper.CallOrderService(Ordersdto);
                return response;
            }
            catch (Exception ex)
            {
                EventLogProvider.LogInformation("Kadena_CMSWebParts_Kadena_Cart_CartCheckout", "ProcessOrder", ex.Message);
                return null;
            }
        }
        /// <summary>
        /// displays the unprocessed order distributors names
        /// </summary>
        /// <param name="addresses"></param>
        private void ShowOrderErrorList(List<string> addresses)
        {
            try
            {
                lblCartError.Text = ResHelper.GetString("KDA.Checkout.OrderError");
                lstErrors.DataSource = addresses;
                lstErrors.DataBind();
            }
            catch (Exception ex)
            {
                EventLogProvider.LogInformation("Kadena_CMSWebParts_Kadena_Cart_CartCheckout", "ShowError", ex.Message);
            }
        }
        #endregion Methods
    }
}