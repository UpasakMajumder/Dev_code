using CMS.DataEngine;
using CMS.Ecommerce;
using CMS.Ecommerce.Web.UI;
using CMS.EventLog;
using CMS.Helpers;
using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using CMS.CustomTables;
using CMS.CustomTables.Types.KDA;
using System.Collections.Generic;

namespace Kadena.CMSWebParts.Kadena.Cart
{
    public partial class DistributorCartDetails : CMSCheckoutWebPart
    {
        #region "Public properties"

        public int CartID
        {
            get
            {
                return ValidationHelper.GetInteger(GetValue("CartID"), default(int));
            }
            set
            {
                SetValue("CartID", value);
            }
        }

        public int ShoppingCartDistributorID
        {
            get
            {
                return ValidationHelper.GetInteger(GetValue("ShoppingCartDistributorID"), default(int));
            }
            set
            {
                SetValue("ShoppingCartDistributorID", value);
            }
        }
        private List<BusinessUnitItem> BusinessUnits { get; set; }
        private List<ShippingOptionInfo> ShippingOptions { get; set; }

        public bool ValidCart { get; set; }
        public ShoppingCartInfo Cart { get; set; }
        public object CustomtableItemProvider { get; private set; }
        public string POSNumber
        {
            get
            {
                return ResHelper.GetString("KDA.DistributorCart.POSNumber");
            }
            set
            {
                SetValue("POSNumber", value);
            }
        }
        public string ProductName
        {
            get
            {
                return ResHelper.GetString("KDA.DistributorCart.ProductName");
            }
            set
            {
                SetValue("ProductName", value);
            }
        }
        public string Quantity
        {
            get
            {
                return ResHelper.GetString("KDA.DistributorCart.Quantity");
            }
            set
            {
                SetValue("Quantity", value);
            }
        }
        public string Price
        {
            get
            {
                return ResHelper.GetString("KDA.DistributorCart.Price");
            }
            set
            {
                SetValue("Price", value);
            }
        }
        public string Save
        {
            get
            {
                return ResHelper.GetString("KDA.DistributorCart.Save");
            }
            set
            {
                SetValue("Save", value);
            }
        }
        public string SaveasPDF
        {
            get
            {
                return ResHelper.GetString("KDA.DistributorCart.SaveasPDF");
            }
            set
            {
                SetValue("SaveasPDF", value);
            }
        }
        public string Print
        {
            get
            {
                return ResHelper.GetString("KDA.DistributorCart.Print");
            }
            set
            {
                SetValue("Print", value);
            }
        }
        public string Shipping
        {
            get
            {
                return ResHelper.GetString("KDA.DistributorCart.Shipping");
            }
            set
            {
                SetValue("Shipping", value);
            }
        }
        public string BusinessUnit
        {
            get
            {
                return ResHelper.GetString("KDA.DistributorCart.BusinessUnit");
            }
            set
            {
                SetValue("BusinessUnit", value);
            }
        }
        public string SubTotal
        {
            get
            {
                return ResHelper.GetString("KDA.DistributorCart.SubTotal");
            }
            set
            {
                SetValue("SubTotal", value);
            }
        }

        #endregion "Public properties"

        #region "Page events"

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Cart= ShoppingCartInfoProvider.GetShoppingCartInfo(CartID); 
                GetItems();
                GetShippingOptions();
                if (!IsPostBack)
                {
                    BindRepeaterData();
                    rptCartItems.ReloadData(true);
                }
                ValidCart = true;
            }
            catch (Exception ex)
            {
                EventLogProvider.LogInformation("Kadena_CMSWebParts_Kadena_Cart_DistributorCartDetails", "Page_Load", ex.Message);
            }
        }

        /// <summary>
        /// Clears cache.
        /// </summary>
        public override void ClearCache()
        {
            try
            {
                rptCartItems.ClearCache();
            }
            catch (Exception ex)
            {
                EventLogProvider.LogInformation("Kadena_CMSWebParts_Kadena_Cart_DistributorCartDetails", "Page_Load", ex.Message);
            }
        }

        /// <summary>
        /// OnPrerender override (Set visibility).
        /// </summary>
        protected override void OnPreRender(EventArgs e)
        {
            try
            {
                if (ValidCart)
                {
                    var quantity = 0;
                    double price = 0;
                    base.OnPreRender(e);
                    BindRepeaterData();
                    rptCartItems.ReloadData(true);
                    Visible = rptCartItems.Visible && !StopProcessing;

                    if (DataHelper.DataSourceIsEmpty(rptCartItems.DataSource))
                    {
                        Visible = false;
                        lnkSaveCartItems.Visible = false;
                        tblCartItems.Visible = false;
                    }
                    foreach (Control item in rptCartItems.Items)
                    {
                        var txtQuantity = item.Controls[0].FindControl("txtUnits") as TextBox;
                        quantity += ValidationHelper.GetInteger(txtQuantity.Text, default(int));
                        var lblSKUPrice = item.Controls[0].FindControl("hdnSKUPrice") as HiddenField;
                        price += ValidationHelper.GetDouble(lblSKUPrice.Value, default(double));
                    }
                    
                    var ddlBusinessUnits = rptCartItems.Controls[rptCartItems.Controls.Count - 1].Controls[0].FindControl("ddlBusinessUnits") as DropDownList;
                    ddlBusinessUnits.DataSource = BusinessUnits;
                    ddlBusinessUnits.DataValueField = "BusinessUnitNumber";
                    ddlBusinessUnits.DataTextField = "BusinessUnitName";
                    ddlBusinessUnits.DataBind();
                    if (!IsPostBack)
                    {
                        var ddlShippingOptions = rptCartItems.Controls[rptCartItems.Controls.Count - 1].Controls[0].FindControl("ddlShippingOptions") as DropDownList;
                        ddlShippingOptions.DataSource = ShippingOptions;
                        ddlShippingOptions.DataValueField = "ShippingOptionID";
                        ddlShippingOptions.DataTextField = "ShippingOptionName";
                        ddlShippingOptions.DataBind();
                    }
                    (rptCartItems.Controls[rptCartItems.Controls.Count - 1].Controls[0].FindControl("lblTotalPrice") as Label).Text = CurrencyInfoProvider.GetFormattedPrice(Cart.Shipping, CurrentSite.SiteID);
                    (rptCartItems.Controls[rptCartItems.Controls.Count - 1].Controls[0].FindControl("lblTotalPrice") as Label).Text = CurrencyInfoProvider.GetFormattedPrice(price, CurrentSite.SiteID);
                }
            }
            catch (Exception ex)
            {
                EventLogProvider.LogInformation("Kadena_CMSWebParts_Kadena_Cart_DistributorCartDetails", "OnPreRender", ex.Message);
            }
        }

        #endregion "Page events"

        #region "Event handling"

        protected void btnSaveCartItems_Click(object sender, EventArgs e)
        {
            try
            {
               // var cart = ShoppingCartInfoProvider.GetShoppingCartInfo(CartID);
                foreach (Control item in rptCartItems.Items)
                {
                    var txtQuantity = item.Controls[0].FindControl("txtUnits") as TextBox;
                    var quantity = ValidationHelper.GetInteger(txtQuantity.Text, default(int));
                    if (quantity > default(int))
                    {
                        var hdnCartItemID = item.Controls[0].FindControl("hdnCartItemID") as HiddenField;
                        var cartItemID = ValidationHelper.GetInteger(hdnCartItemID.Value, default(int));
                        if (cartItemID != default(int))
                        {
                            var cartItem = Cart.CartItems.Where(x => x.CartItemID == cartItemID).FirstOrDefault();
                            ShoppingCartItemInfoProvider.UpdateShoppingCartItemUnits(cartItem, quantity);
                            ShoppingCartInfoProvider.EvaluateShoppingCart(Cart);
                            ComponentEvents.RequestEvents.RaiseEvent(sender, e, SHOPPING_CART_CHANGED);
                            Cart.InvalidateCalculations();
                        }
                    }
                    else
                    {
                        lblCartError.Text = "Please enter valid Quantity";
                        ValidCart = false;
                        return;
                    }
                }
                var ddlShippingOptions = rptCartItems.Controls[rptCartItems.Controls.Count - 1].Controls[0].FindControl("ddlShippingOptions") as DropDownList;
                Cart.ShoppingCartShippingOptionID =ValidationHelper.GetInteger(ddlShippingOptions.SelectedValue,default(int));
                Cart.Update();
                lblCartUpdateSuccess.Text = "Cart updated successfully";
            }
            catch (Exception ex)
            {
                EventLogProvider.LogInformation("Kadena_CMSWebParts_Kadena_Cart_DistributorCartDetails", "btnSaveCartItems_Click", ex.Message);
            }
        }

        #endregion "Event handling"
        public void GetItems()
        {
            if (BusinessUnits == null)
            {
                BusinessUnits = CustomTableItemProvider.GetItems<BusinessUnitItem>()
        .Source(sourceItem => sourceItem.Join<UserBusinessUnitsItem>("ItemID", "BusinessUnitID"))
        .WhereEquals("UserID", CurrentUser.UserID).WhereEquals("SiteID", CurrentSite.SiteID).WhereTrue("Status").Columns("BusinessUnitNumber,BusinessUnitName").ToList();
            }
        }
        public void GetShippingOptions()
        {
            if (ShippingOptions == null)
            {
                ShippingOptions = ShippingOptionInfoProvider.GetShippingOptions()
                                            .OnSite(CurrentSite.SiteID).ToList();
            }
        }
        private void BindRepeaterData()
        {
            rptCartItems.CacheMinutes = 0;
            QueryDataParameters parameters = new QueryDataParameters();
            parameters.Add("@CartItemDistributorID", ShoppingCartDistributorID);
            rptCartItems.QueryParameters = parameters;
            rptCartItems.QueryName = "Ecommerce.Shoppingcart.GetCartItems";
            rptCartItems.TransformationName = "KDA.Transformations.xCartItems";
        }
    }
}