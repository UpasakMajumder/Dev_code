using CMS.DataEngine;
using CMS.Ecommerce;
using CMS.EventLog;
using CMS.Helpers;
using CMS.PortalEngine.Web.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Kadena.CMSWebParts.Kadena.ShoppingCart
{
    public partial class CustomerCartOperations : CMSAbstractWebPart
    {
        #region Properties
        public string InventoryType
        {
            get
            {
                return ValidationHelper.GetString(GetValue("InventoryType"), "general");
            }
            set
            {
                SetValue("InventoryType", value);
            }
        }
        #endregion

        #region "Variables"
        private int productSKU;
        #endregion

        #region "Methods"

        /// <summary>
        /// Content loaded event handler.
        /// </summary>
        public override void OnContentLoaded()
        {
            base.OnContentLoaded();
            SetupControl();
        }
        /// <summary>
        /// Initializes the control properties.
        /// </summary>
        protected void SetupControl()
        {
            if (this.StopProcessing)
            {
                // Do not process
            }
            else
            {
                productSKU = ValidationHelper.GetInteger(Request.QueryString["id"], default(int));
                if (!this.IsPostBack)
                {
                    lblProductName.Text = ValidationHelper.GetString(Request.QueryString["skuname"], string.Empty);
                    BindCustomersList(productSKU);
                }
            }
        }
        /// <summary>
        /// Reloads the control data.
        /// </summary>
        public override void ReloadData()
        {
            base.ReloadData();

            SetupControl();
        }
        /// <summary>
        /// Get all Cusromers / Distributers list based on product ID
        /// </summary>
        /// <param name="productID">Producct skuid</param>
        private void BindCustomersList(int productID)
        {
            try
            {
                QueryDataParameters parameters = new QueryDataParameters();
                parameters.Add("@SKUID", productID);
                var customers = new DataQuery("ecommerce.Customer.GetCustomersList")
                     .Columns("CustomerID", "CustomerEmail", "CASE WHEN SKUUnits IS NULL THEN 0 ELSE 1 END IsSelected ", " ISNULL(SKUUnits,0) SKUUnits ", $"CASE WHEN ShoppingCartInventoryType='{InventoryType}' THEN NULL ELSE C2.ShoppingCartID  END AS [ShoppingCartID]", "C3.SKUID SKUID");
                customers.Parameters = parameters;
                gvCustomersCart.DataSource = customers.Result;
                gvCustomersCart.DataBind();
            }
            catch (Exception ex)
            {
                EventLogProvider.LogException("CustomerCartOperations.ascx.cs", "BindCustomersList()", ex);
            }
        }
        /// <summary>
        /// Add to
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btmAddItemsToCart_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (GridViewRow row in gvCustomersCart.Rows)
                {
                    if (row.RowType == DataControlRowType.DataRow)
                    {
                        CheckBox chkRow = (row.Cells[0].FindControl("chkSelected") as CheckBox);
                        int customerID = Convert.ToInt32(row.Cells[1].Text);
                        TextBox txtQty = (row.Cells[3].FindControl("txtQuanityOrdering") as TextBox);
                        var quantityPlacing = ValidationHelper.GetInteger(txtQty.Text, default(int));
                        var customerShoppingCartID = ValidationHelper.GetInteger(row.Cells[4].Text, default(int));
                        if (chkRow.Checked)
                        {
                            var cartID = CustomerHasAlreadyShoppingCartWithSameType(customerID);
                            if (customerShoppingCartID == default(int) && quantityPlacing > 0)
                            {
                                if (cartID == default(int))
                                {
                                    CreateShoppingCartByCustomer(customerID, quantityPlacing, 10, 10, InventoryType);
                                }
                                else
                                {
                                    Updatingtheunitcountofcartitem(cartID, quantityPlacing, customerID);
                                }
                                BindCustomersList(productSKU);
                            }
                            else if (customerShoppingCartID > 0 && quantityPlacing > 0)
                            {
                                Updatingtheunitcountofcartitem(customerShoppingCartID, quantityPlacing, customerID);
                                BindCustomersList(productSKU);
                            }
                            else if (customerShoppingCartID > 0 && quantityPlacing == 0)
                            {
                                RemovingProductFromShoppingCart(customerShoppingCartID);
                                BindCustomersList(productSKU);
                            }
                        }
                        else if (customerShoppingCartID > 0 && quantityPlacing > 0)
                        {
                            RemovingProductFromShoppingCart(customerShoppingCartID);
                            BindCustomersList(productSKU);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                EventLogProvider.LogException("CustomerCartOperations.ascx.cs", "btmAddItemsToCart_Click()", ex);
            }

        }
        /// <summary>
        /// Updating the unit count of shopping cart Item
        /// </summary>
        private void Updatingtheunitcountofcartitem(int shoppinCartID, int unitCount, int customerID)
        {
            try
            {
                SKUInfo product = SKUInfoProvider.GetSKUs().WhereEquals("SKUID", productSKU).WhereNull("SKUOptionCategoryID").FirstObject;
                if (!DataHelper.DataSourceIsEmpty(product))
                {
                    ShoppingCartItemInfo item = null;
                    ShoppingCartInfo cart = ShoppingCartInfoProvider.GetShoppingCartInfo(shoppinCartID);
                    var campaingnID = ValidationHelper.GetInteger(cart.GetValue("ShoppingCartCampaignID"), default(int));
                    var programID = ValidationHelper.GetInteger(cart.GetValue("ShoppingCartProgramID"), default(int));
                    var inventoryType = ValidationHelper.GetString(cart.GetValue("ShoppingCartInventoryType"), string.Empty);
                    foreach (ShoppingCartItemInfo cartItem in cart.CartItems)
                    {
                        if (cartItem.SKUID == product.SKUID)
                        {
                            item = cartItem;
                            break;
                        }
                    }
                    if (!DataHelper.DataSourceIsEmpty(item))
                    {
                        ShoppingCartItemInfoProvider.UpdateShoppingCartItemUnits(item, unitCount);
                        cart.InvalidateCalculations();
                    }
                    else
                    {
                        ShoppingCartItemParameters parameters = new ShoppingCartItemParameters(product.SKUID, unitCount);
                        parameters.CustomParameters.Add("CartItemCustomerID", customerID);
                        ShoppingCartItemInfo cartItem = cart.SetShoppingCartItem(parameters);
                        cartItem.SetValue("CartItemDistributorID", customerID);
                        cartItem.SetValue("CartItemCampaignID", campaingnID);
                        cartItem.SetValue("CartItemProgramID", programID);
                        ShoppingCartItemInfoProvider.SetShoppingCartItemInfo(cartItem);
                    }
                }
            }
            catch (Exception ex)
            {
                EventLogProvider.LogException("CustomerCartOperations.ascx.cs", "Updatingtheunitcountofcartitem()", ex);
            }
        }

        /// <summary>
        /// Removing Shopping Cart and cart items by cart id
        /// </summary>
        /// <param name="shoppingCartID"></param>
        private void RemovingProductFromShoppingCart(int shoppingCartID)
        {
            try
            {
                SKUInfo product = SKUInfoProvider.GetSKUs().WhereEquals("SKUID", productSKU).WhereNull("SKUOptionCategoryID").FirstObject;
                if (!DataHelper.DataSourceIsEmpty(product))
                {
                    ShoppingCartItemInfo item = null;
                    ShoppingCartInfo cart = ShoppingCartInfoProvider.GetShoppingCartInfo(shoppingCartID);
                    foreach (ShoppingCartItemInfo cartItem in cart.CartItems)
                    {
                        if (cartItem.SKUID == product.SKUID)
                        {
                            item = cartItem;
                            break;
                        }
                    }
                    if (!DataHelper.DataSourceIsEmpty(item))
                    {
                        ShoppingCartInfoProvider.RemoveShoppingCartItem(cart, item.CartItemID);
                        ShoppingCartItemInfoProvider.DeleteShoppingCartItemInfo(item);
                        if (cart.CartItems.Count == 0)
                        {
                            ShoppingCartInfoProvider.DeleteShoppingCartInfo(shoppingCartID);
                        }
                        cart.InvalidateCalculations();
                    }
                }
            }
            catch (Exception ex)
            {
                EventLogProvider.LogException("CustomerCartOperations.ascx.cs", "RemovingProductFromShoppingCart()", ex);
            }
        }

        /// <summary>
        /// Create Shopping cart with item by customer
        /// </summary>
        /// <param name="customerID"></param>
        /// <param name="txtQty"></param>
        private void CreateShoppingCartByCustomer(int customerID, int productQty, int campaingnID, int programID, string inventoryType)
        {
            try
            {
                SKUInfo product = SKUInfoProvider.GetSKUs().WhereEquals("SKUID", productSKU).WhereNull("SKUOptionCategoryID").FirstObject;
                if (!DataHelper.DataSourceIsEmpty(product))
                {
                    ShoppingCartInfo cart = new ShoppingCartInfo();
                    cart.ShoppingCartSiteID = CurrentSite.SiteID;
                    cart.ShoppingCartShippingOptionID = 48;
                    cart.ShoppingCartCustomerID = customerID;
                    cart.SetValue("ShoppingCartCampaignID", campaingnID);
                    cart.SetValue("ShoppingCartProgramID", programID);
                    cart.SetValue("ShoppingCartDistributorID", customerID);
                    cart.SetValue("ShoppingCartInventoryType", inventoryType);
                    ShoppingCartInfoProvider.SetShoppingCartInfo(cart);
                    ShoppingCartItemParameters parameters = new ShoppingCartItemParameters(product.SKUID, productQty);
                    parameters.CustomParameters.Add("CartItemCustomerID", customerID);
                    ShoppingCartItemInfo cartItem = cart.SetShoppingCartItem(parameters);
                    //cartItem.SetValue("CartItemCustomerID", customerID);
                    cartItem.SetValue("CartItemDistributorID", customerID);
                    cartItem.SetValue("CartItemCampaignID", campaingnID);
                    cartItem.SetValue("CartItemProgramID", programID);
                    ShoppingCartItemInfoProvider.SetShoppingCartItemInfo(cartItem);
                    cart.InvalidateCalculations();
                }
            }
            catch (Exception ex)
            {
                EventLogProvider.LogException("CustomerCartOperations.ascx.cs", "CreateShoppingCartByCustomer()", ex);
            }
        }

        private int CustomerHasAlreadyShoppingCartWithSameType(int customerID)
        {
            var result = default(int);
            try
            {
                if (customerID > 0)
                {
                    var cart = ShoppingCartInfoProvider.GetShoppingCarts().WhereEquals("ShoppingCartCustomerID", customerID).WhereEquals("ShoppingCartInventoryType", InventoryType).FirstOrDefault();
                    result = (DataHelper.DataSourceIsEmpty(cart)) ? default(int) : cart.ShoppingCartID;
                }
            }
            catch (Exception ex)
            {
                EventLogProvider.LogException("CustomerCartOperations.ascx.cs", "CustomerHasAlreadyShoppingCartWithSameType()", ex);
            }
            return result;
        }
        #endregion
    }
}