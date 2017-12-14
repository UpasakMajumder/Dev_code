using CMS.CustomTables;
using CMS.CustomTables.Types.KDA;
using CMS.DataEngine;
using CMS.Ecommerce;
using CMS.EventLog;
using CMS.Helpers;
using CMS.PortalEngine.Web.UI;
using Kadena.Old_App_Code.Kadena.Enums;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;
using System.Linq;

namespace Kadena.CMSWebParts.Kadena.ShoppingCart
{
    public partial class CustomerCartOperations : CMSAbstractWebPart
    {
        #region Properties
        /// <summary>
        /// The property describe's inventory type
        /// </summary>
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
        /// <summary>
        /// SKUID of the product adding to cart
        /// </summary>
        public int ProductSKUID { get; set; }
        /// <summary>
        /// Campaign id of the product if it has
        /// </summary>
        public int ProductCampaignID { get; set; }
        /// <summary>
        /// Programm id of the product if it has
        /// </summary>
        public int ProductProgramID { get; set; }
        /// <summary>
        /// Product Shipping id of the product if it product type is pre-buy
        /// </summary>
        public int ProductShippingID { get; set; }
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
                if (!this.IsPostBack)
                {
                    var product = SKUInfoProvider.GetSKUInfo(ProductSKUID);
                    if (!DataHelper.DataSourceIsEmpty(product) && InventoryType == (int)ProductType.GeneralInventory)
                    {
                        lblProductName.Text = product.SKUName;
                        lblAvailbleItems.Text = $"{product.SKUAvailableItems} {ResHelper.GetString("Kadena.AddToCart.StockAvilable")}";
                    }
                    else
                    {
                        lblProductName.Text = product?.SKUName;
                        lblAvailbleItems.Visible = false;
                    }
                    var hasBusinessUnit = CheckPersonHasBusinessUnit();
                    if (hasBusinessUnit)
                    {
                        lblErrorMsg.Visible = false;
                        gvCustomersCart.Visible = true;
                        btnDisplay.Visible = true;
                        BindCustomersList(ProductSKUID);
                    }
                    else
                    {
                        btnDisplay.Visible = false;
                        lblErrorMsg.Visible = true;
                        lblErrorMsg.Text = ResHelper.GetString("Kadena.AddToCart.BusinessUnitError");
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
                var customer = CustomerInfoProvider.GetCustomerInfoByUserID(CurrentUser.UserID);
                if (!DataHelper.DataSourceIsEmpty(customer))
                {
                    var finalDistributors = new List<DistributorData>();
                    var distributorsAddress = AddressInfoProvider.GetAddresses(customer.CustomerID).Columns("AddressID", "AddressPersonalName").ToList();
                    if (distributorsAddress.Count > 0)
                    {
                        var distributorsCarts = ShoppingCartInfoProvider.GetShoppingCarts().WhereIn("ShoppingCartDistributorID", distributorsAddress.Select(g => g.AddressID).ToList())
                                                                     .WhereEquals("ShoppingCartInventoryType", InventoryType)
                                                                     .Columns("ShoppingCartID", "ShoppingCartDistributorID").Distinct().ToList();
                        var distributorsCartItems = ShoppingCartItemInfoProvider.GetShoppingCartItems().WhereIn("CartItemDistributorID", distributorsAddress.Select(g => g.AddressID).ToList())
                                                                     .WhereIn("ShoppingCartID", distributorsCarts.Select(g => g.ShoppingCartID).ToList()).ToList();
                        distributorsAddress.ForEach(g =>
                        {
                            var cartItem = distributorsCartItems.Where(k => k.GetValue<int>("CartItemDistributorID", default(int)) == g.AddressID && k.SKUID == productID).FirstOrDefault();
                            var existingItem = finalDistributors.Where(k => k.SKUID == cartItem?.SKUID && k.AddressID == g.AddressID).FirstOrDefault();
                            if (DataHelper.DataSourceIsEmpty(existingItem))
                            {
                                var distributor = new DistributorData();
                                distributor.AddressID = g.AddressID;
                                distributor.AddressPersonalName = g.AddressPersonalName;
                                distributor.IsSelected = cartItem != null ? cartItem.CartItemUnits > 0 ? true : false : false;
                                distributor.ShoppingCartID = cartItem != null ? cartItem.ShoppingCartID : 0;
                                distributor.SKUID = cartItem != null ? cartItem.SKUID : 0;
                                distributor.SKUUnits = cartItem != null ? cartItem.CartItemUnits : 0;
                                finalDistributors.Add(distributor);
                            }
                        });
                    }
                    gvCustomersCart.DataSource = finalDistributors;
                    gvCustomersCart.DataBind();
                }
                else
                {
                    lblError.Text = ResHelper.GetString("Kadena.AddToCart.DistributorError");
                    lblError.Visible = true;
                }
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
                SKUInfo product = SKUInfoProvider.GetSKUs().WhereEquals("SKUID", ProductSKUID).WhereNull("SKUOptionCategoryID").FirstObject;
                var itemsPlaced = default(int);
                foreach (GridViewRow row in gvCustomersCart.Rows)
                {
                    if (row.RowType == DataControlRowType.DataRow && !DataHelper.DataSourceIsEmpty(product))
                    {
                        CheckBox chkRow = (row.Cells[0].FindControl("chkSelected") as CheckBox);
                        int customerAddressID = Convert.ToInt32(row.Cells[1].Text);
                        TextBox txtQty = (row.Cells[3].FindControl("txtQuanityOrdering") as TextBox);
                        var quantityPlacing = ValidationHelper.GetInteger(txtQty.Text, default(int));
                        var customerShoppingCartID = ValidationHelper.GetInteger(row.Cells[4].Text, default(int));
                        if (chkRow.Checked)
                        {
                            if (InventoryType == (int)ProductType.GeneralInventory)
                            {
                                itemsPlaced += quantityPlacing;
                                if (itemsPlaced < product.SKUAvailableItems)
                                {
                                    CartProcessOperations(customerShoppingCartID, quantityPlacing, product, customerAddressID);
                                }
                                else
                                {
                                    lblErrorMsg.Text = ResHelper.GetString("Kadena.AddToCart.StockError");
                                    lblErrorMsg.Visible = true;
                                }
                            }
                            else
                            {
                                CartProcessOperations(customerShoppingCartID, quantityPlacing, product, customerAddressID);
                            }
                        }
                        else if (customerShoppingCartID > 0 && quantityPlacing > 0)
                        {
                            RemovingProductFromShoppingCart(product, customerShoppingCartID);
                            BindCustomersList(ProductSKUID);
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
        /// Cart operations based on the values 
        /// </summary>
        /// <param name="cartID">shoppingcart id</param>
        /// <param name="quantity">units placing</param>
        /// <param name="productInfo">skuinfo object</param>
        /// <param name="addressID">distributor addressid</param>
        private void CartProcessOperations(int cartID, int quantity, SKUInfo productInfo, int addressID)
        {
            try
            {
                if (!DataHelper.DataSourceIsEmpty(productInfo) && addressID != default(int))
                {
                    if (cartID == default(int) && quantity > 0)
                    {
                        CreateShoppingCartByCustomer(productInfo, addressID, quantity);
                        BindCustomersList(ProductSKUID);
                    }
                    else if (cartID > 0 && quantity > 0)
                    {
                        Updatingtheunitcountofcartitem(productInfo, cartID, quantity, addressID);
                        BindCustomersList(ProductSKUID);
                    }
                    else if (cartID > 0 && quantity == 0)
                    {
                        RemovingProductFromShoppingCart(productInfo, cartID);
                        BindCustomersList(ProductSKUID);
                    }
                }
            }
            catch (Exception ex)
            {
                EventLogProvider.LogException("CustomerCartOperations.ascx.cs", "CartProcess()", ex);
            }
        }
        /// <summary>
        /// Updating the unit count of shopping cart Item
        /// </summary>
        private void Updatingtheunitcountofcartitem(SKUInfo product, int shoppinCartID, int unitCount, int customerAddressID)
        {
            try
            {
                var customerAddress = AddressInfoProvider.GetAddressInfo(customerAddressID);
                if (!DataHelper.DataSourceIsEmpty(product))
                {
                    ShoppingCartItemInfo item = null;
                    ShoppingCartInfo cart = ShoppingCartInfoProvider.GetShoppingCartInfo(shoppinCartID);
                    cart.User = CurrentUser;
                    cart.ShoppingCartShippingAddress = customerAddress;
                    var campaingnID = ValidationHelper.GetInteger(cart.GetValue("ShoppingCartCampaignID"), default(int));
                    var programID = ValidationHelper.GetInteger(cart.GetValue("ShoppingCartProgramID"), default(int));
                    var inventoryType = ValidationHelper.GetString(cart.GetValue("ShoppingCartInventoryType"), string.Empty);
                    item = cart.CartItems.Where(g => g.SKUID == product.SKUID).FirstOrDefault();
                    if (!DataHelper.DataSourceIsEmpty(item))
                    {
                        item.CartItemPrice = product.SKUPrice;
                        ShoppingCartItemInfoProvider.UpdateShoppingCartItemUnits(item, unitCount);
                        cart.InvalidateCalculations();
                    }
                    else
                    {
                        ShoppingCartItemParameters parameters = new ShoppingCartItemParameters(product.SKUID, unitCount);
                        parameters.CustomParameters.Add("CartItemCustomerID", customerAddressID);
                        parameters.Price = (InventoryType == (int)ProductType.GeneralInventory) ? default(double) : product.SKUPrice;
                        ShoppingCartItemInfo cartItem = cart.SetShoppingCartItem(parameters);
                        cartItem.SetValue("CartItemDistributorID", customerAddressID);
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
        private void RemovingProductFromShoppingCart(SKUInfo product, int shoppingCartID)
        {
            try
            {
                if (!DataHelper.DataSourceIsEmpty(product))
                {
                    ShoppingCartItemInfo item = null;
                    ShoppingCartInfo cart = ShoppingCartInfoProvider.GetShoppingCartInfo(shoppingCartID);
                    cart.User = CurrentUser;
                    item = cart.CartItems.Where(g => g.SKUID == product.SKUID).FirstOrDefault();
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
        /// <param name="customerAddressID"></param>
        /// <param name="txtQty"></param>
        private void CreateShoppingCartByCustomer(SKUInfo product, int customerAddressID, int productQty)
        {
            try
            {
                var customerAddress = AddressInfoProvider.GetAddressInfo(customerAddressID);
                if (!DataHelper.DataSourceIsEmpty(product))
                {
                    ShoppingCartInfo cart = new ShoppingCartInfo();
                    cart.ShoppingCartSiteID = CurrentSite.SiteID;
                    cart.ShoppingCartCustomerID = customerAddressID;
                    cart.SetValue("ShoppingCartCampaignID", ProductCampaignID);
                    cart.SetValue("ShoppingCartProgramID", ProductProgramID);
                    cart.SetValue("ShoppingCartDistributorID", customerAddressID);
                    cart.SetValue("ShoppingCartInventoryType", InventoryType);
                    cart.User = CurrentUser;
                    cart.ShoppingCartShippingAddress = customerAddress;
                    if (InventoryType == (int)ProductType.PreBuy)
                    {
                        cart.ShoppingCartShippingOptionID = ProductShippingID;
                    }
                    ShoppingCartInfoProvider.SetShoppingCartInfo(cart);
                    ShoppingCartItemParameters parameters = new ShoppingCartItemParameters(product.SKUID, productQty);
                    parameters.CustomParameters.Add("CartItemCustomerID", customerAddressID);
                    parameters.Price = (InventoryType == (int)ProductType.GeneralInventory) ? default(double) : product.SKUPrice;
                    ShoppingCartItemInfo cartItem = cart.SetShoppingCartItem(parameters);
                    cartItem.SetValue("CartItemDistributorID", customerAddressID);
                    cartItem.SetValue("CartItemCampaignID", ProductCampaignID);
                    cartItem.SetValue("CartItemProgramID", ProductProgramID);
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
    /// <summary>
    /// DistributorData object only for binding data to gridview
    /// </summary>
    public class DistributorData
    {
        public int AddressID { get; set; }
        public string AddressPersonalName { get; set; }
        public bool IsSelected { get; set; }
        public int ShoppingCartID { get; set; }
        public int SKUID { get; set; }
        public int SKUUnits { get; set; }
    }
}