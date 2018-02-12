using CMS.Ecommerce;
using CMS.EventLog;
using CMS.Helpers;
using CMS.PortalEngine.Web.UI;
using Kadena.Old_App_Code.Kadena.Constants;
using Kadena.Old_App_Code.Kadena.Enums;
using static Kadena.Old_App_Code.Kadena.Shoppingcart.ShoppingCartHelper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI.WebControls;
using Kadena.Dto.SubmitOrder.MicroserviceRequests;
using Kadena.Old_App_Code.Kadena.Shoppingcart;
using Kadena2.Container.Default;
using Kadena.BusinessLogic.Contracts;
using Kadena.WebAPI.KenticoProviders.Contracts;

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
                if (!DIContainer.Resolve<IShoppingCartProvider>().ValidateAllCarts(userID: CurrentUser.UserID))
                {
                    Response.Cookies["status"].Value = QueryStringStatus.InvalidCartItems;
                    Response.Cookies["status"].HttpOnly = false;
                    return;
                }
                var loggedInUserCartIDs = GetCartsByUserID(CurrentUser.UserID, ProductType.GeneralInventory);
                var unprocessedDistributorIDs = new List<Tuple<int, string>>();
                loggedInUserCartIDs.ForEach(distributorCart =>
                {
                    Cart = ShoppingCartInfoProvider.GetShoppingCartInfo(distributorCart);
                    decimal shippingCost = default(decimal);
                    if (Cart.ShippingOption != null && Cart.ShippingOption.ShippingOptionName.ToLower() != ShippingOption.Ground)
                    {
                        var shippingResponse = GetOrderShippingTotal(Cart);
                        if (shippingResponse != null && shippingResponse.Success)
                        {
                            shippingCost = ValidationHelper.GetDecimal(shippingResponse?.Payload?.Cost, default(decimal));
                        }
                        else
                        {
                            unprocessedDistributorIDs.Add(new Tuple<int, string>(Cart.GetIntegerValue("ShoppingCartDistributorID", default(int)), shippingResponse.ErrorMessages));
                            return;
                        }
                    }
                    OrderDTO Ordersdto = CreateOrdersDTO(Cart, Cart.ShoppingCartUserID, OrderType.generalInventory, shippingCost);
                    var response = ProcessOrder(Cart, CurrentUser.UserID, OrderType.generalInventory, Ordersdto, shippingCost);
                    if (response != null && response.Success)
                    {
                        UpdateAvailableSKUQuantity(Cart);
                        ShoppingCartInfoProvider.DeleteShoppingCartInfo(Cart);
                    }
                    else
                    {
                        unprocessedDistributorIDs.Add(new Tuple<int, string>(Cart.GetIntegerValue("ShoppingCartDistributorID", default(int)), response.ErrorMessages));
                    }
                });
                if (unprocessedDistributorIDs.Count == 0)
                {
                    Response.Cookies["status"].Value = QueryStringStatus.OrderSuccess;
                    Response.Cookies["status"].HttpOnly = false;
                    URLHelper.Redirect(Request.RawUrl);
                }
                else
                {
                    if(loggedInUserCartIDs.Count> unprocessedDistributorIDs.Count)
                    {
                        Response.Cookies["status"].Value = QueryStringStatus.OrderSuccess;
                        Response.Cookies["status"].HttpOnly = false;
                    }
                    Response.Cookies["error"].Value = QueryStringStatus.OrderFail;
                    Response.Cookies["error"].HttpOnly = false;
                    ShowOrderErrorList(unprocessedDistributorIDs);
                    divErrorDailogue.Attributes.Add("class", "dialog active");
                }
            }
            catch (Exception ex)
            {
                EventLogProvider.LogInformation("Kadena_CMSWebParts_Kadena_Cart_CartCheckout", "lnkCheckout_Click", ex.Message);
            }
        }
        /// <summary>
        /// popup click close event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnClose_ServerClick(object sender, EventArgs e)
        {
            Response.Redirect(Request.RawUrl);
        }
        #endregion Events

        #region Methods

        /// <summary>
        /// displays the unprocessed order distributors names
        /// </summary>
        /// <param name="addresses"></param>
        private void ShowOrderErrorList(List<Tuple<int, string>> unprocessedDistributorIDs)
        {
            try
            {
                var addrerss = DIContainer.Resolve<IAddressBookService>();
              var distributors=  addrerss.GetAddressesByAddressIds(unprocessedDistributorIDs.Select(x => x.Item1).ToList()).Select(x =>
                    {
                        return new { AddressID = x?.Id, AddressPersonalName = x?.AddressPersonalName };
                    }).ToList();
                rptErrors.DataSource = unprocessedDistributorIDs.Select(x =>
                {
                    var distributor = distributors.Where(y => y.AddressID == x.Item1).FirstOrDefault();
                    return new
                    {
                        AddressPersonalName = distributor.AddressPersonalName,
                        Reason = x.Item2
                    };
                }).ToList();
                rptErrors.DataBind();
            }
            catch (Exception ex)
            {
                EventLogProvider.LogInformation("Kadena_CMSWebParts_Kadena_Cart_CartCheckout", "ShowError", ex.Message);
            }
        }

        #endregion Methods

    }
}