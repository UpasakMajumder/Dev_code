using CMS.DataEngine;
using CMS.Ecommerce;
using CMS.EventLog;
using CMS.Helpers;
using CMS.PortalEngine.Web.UI;
using System;
using System.Web.UI.WebControls;
using CMS.CustomTables;
using CMS.CustomTables.Types.KDA;

namespace Kadena.CMSWebParts.Kadena.ShoppingCart
{
    public partial class CustomerCartOperations : CMSAbstractWebPart
    {
        #region Properties
        public int InventoryType
        {
            get
            {
                return ValidationHelper.GetInteger(GetValue("InventoryType"), 1);
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
                    var hasBusinessUnit = CheckPersonHasBusinessUnit();
                    if (hasBusinessUnit)
                    {
                        lblErrorMsg.Visible = false;
                        gvCustomersCart.Visible = true;
                        btnDisplay.Visible = true;
                        lblProductName.Text = ValidationHelper.GetString(Request.QueryString["skuname"], string.Empty);
                        BindCustomersList(productSKU);
                    }
                    else
                    {
                        btnDisplay.Visible = false;
                        lblErrorMsg.Visible = true;
                        gvCustomersCart.Visible = false;
                    }
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
                var customers = new DataQuery().From(new QuerySource(new QuerySourceTable("COM_Customer", "C1"))
                    .LeftJoin("COM_ShoppingCart C2", "C1.CustomerID", "C2.ShoppingCartCustomerID")
                    .LeftJoin("COM_ShoppingCartSKU C3", "C2.ShoppingCartID", "C3.ShoppingCartID"))
                    .Columns("CustomerID", "CustomerEmail", "CASE WHEN SKUUnits IS NULL THEN 0 ELSE 1 END IsSelected ", " ISNULL(SKUUnits,0) SKUUnits ", $"CASE WHEN ShoppingCartInventoryType='{InventoryType}' THEN NULL ELSE C2.ShoppingCartID  END AS [ShoppingCartID]", "C3.SKUID SKUID");
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
                            if (customerShoppingCartID == default(int) && quantityPlacing > 0)
                            {
                                CreateShoppingCartByCustomer(customerID, quantityPlacing, 10, 10, InventoryType);
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
                    cart.User = CurrentUser;
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
                    cart.User = CurrentUser;
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
        private void CreateShoppingCartByCustomer(int customerID, int productQty, int campaingnID, int programID, int inventoryType)
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
                    cart.User = CurrentUser;
                    ShoppingCartInfoProvider.SetShoppingCartInfo(cart);
                    ShoppingCartItemParameters parameters = new ShoppingCartItemParameters(product.SKUID, productQty);
                    parameters.CustomParameters.Add("CartItemCustomerID", customerID);
                    ShoppingCartItemInfo cartItem = cart.SetShoppingCartItem(parameters);
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
        /// <summary>
        /// Checks whether the current user has mapped to any business unit
        /// </summary>
        /// <returns>Boolean Value</returns>
        private bool CheckPersonHasBusinessUnit()
        {
            var result = default(bool);
            try
            {
                var personBusinessUnits = CustomTableItemProvider.GetItems<UserBusinessUnitsItem>().WhereEquals("UserID", CurrentUser.UserID).TopN(2).Result.Tables[0];
                if (!DataHelper.DataSourceIsEmpty(personBusinessUnits))
                {
                    result = personBusinessUnits.Rows.Count > 0;
                }
            }
            catch (Exception ex)
            {
                EventLogProvider.LogException("CustomerCartOperations.ascx.cs", "RemovingProductFromShoppingCart()", ex);
            }
            return result;
        }
        #endregion
    }
    public enum ProductsType
    {
        GeneralInventory = 1,
        PreBuy
    }
}