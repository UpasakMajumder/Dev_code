using CMS.Ecommerce;
using CMS.EventLog;
using CMS.Helpers;
using CMS.PortalEngine.Web.UI;
using Kadena.Dto.General;
using Kadena.Dto.SubmitOrder.MicroserviceRequests;
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
        private ShoppingCartInfo Cart { get; set; }

        #region Events

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
                    lblCartError.Text = ResHelper.GetString("KDA.Checkout.OrderError");
                    var distributors = AddressInfoProvider.GetAddresses().WhereIn("AddressID", unprocessedDistributorIDs).Column("AddressPersonalName").ToList().Select(x => x?.AddressPersonalName).ToList();
                    ShowError(distributors);
                }
                divDailogue.Attributes.Add("class", "dialog active");
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
                OrdersDTO Ordersdto = ShoppingCartHelper.CreateOrdersDTO(Cart, CurrentUser.UserID);
                var response = ShoppingCartHelper.CallOrderService(Ordersdto);
                return response;
            }
            catch (Exception ex)
            {
                EventLogProvider.LogInformation("Kadena_CMSWebParts_Kadena_Cart_CartCheckout", "ProcessOrder", ex.Message);
                return null;
            }
        }

        private void ShowError(List<string> addresses)
        {
            try
            {
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