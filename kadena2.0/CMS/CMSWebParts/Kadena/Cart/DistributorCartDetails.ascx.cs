using CMS.CustomTables;
using CMS.CustomTables.Types.KDA;
using CMS.DataEngine;
using CMS.Ecommerce;
using CMS.Ecommerce.Web.UI;
using CMS.EventLog;
using CMS.Helpers;
using Kadena.Dto.EstimateDeliveryPrice.MicroserviceRequests;
using Kadena.Dto.EstimateDeliveryPrice.MicroserviceResponses;
using Kadena.Dto.General;
using Kadena.Old_App_Code.Kadena.Constants;
using Kadena.Old_App_Code.Kadena.Enums;
using Kadena.Old_App_Code.Kadena.PDFHelpers;
using Kadena.Old_App_Code.Kadena.Shoppingcart;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Kadena.CMSWebParts.Kadena.Cart
{
    public partial class DistributorCartDetails : CMSCheckoutWebPart
    {
        private const string _serviceUrlSettingKey = "KDA_ShippingCostServiceUrl";

        #region "Private Properties"

        private List<BusinessUnitItem> BusinessUnits { get; set; }
        private List<ShippingOptionInfo> ShippingOptions { get; set; }

        #endregion "Private Properties"

        #region "Public properties"

        public double ShippingCost { get; set; }
        public bool ValidCart { get; set; }
        public ShoppingCartInfo Cart { get; set; }
        public object CustomtableItemProvider { get; private set; }

        /// <summary>
        ///  /// <summary>
        /// Gets or sets the CartID.
        /// </summary>
        /// </summary>
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

        /// <summary>
        /// Gets or sets the ShoppingCartDistributorID.
        /// </summary>
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

        /// <summary>
        /// gets or sets Inventory Type
        /// </summary>
        public int InventoryType
        {
            get
            {
                return ValidationHelper.GetInteger(GetValue("InventoryType"), default(int));
            }
            set
            {
                SetValue("InventoryType", value);
            }
        }

        /// <summary>
        /// Gets or sets the POSNumber.
        /// </summary>
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

        /// <summary>
        /// Gets or sets the ProductName.
        /// </summary>
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

        /// <summary>
        /// Gets or sets the Quantity.
        /// </summary>
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

        /// <summary>
        /// Gets or sets the Price.
        /// </summary>
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

        /// <summary>
        /// Gets or sets the SaveasPDF
        /// </summary>
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

        /// <summary>
        /// Gets or sets the Print
        /// </summary>
        public string Action
        {
            get
            {
                return ResHelper.GetString("KDA.DistributorCart.Action");
            }
            set
            {
                SetValue("Action", value);
            }
        }

        /// <summary>
        /// Gets or sets the Shipping
        /// </summary>
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

        /// <summary>
        /// Gets or sets the BusinessUnit
        /// </summary>
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
        } /// <summary>

        /// Gets or sets the SubTotal
        /// </summary>
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

        /// <summary>
        /// Page load event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Cart = ShoppingCartInfoProvider.GetShoppingCartInfo(CartID);
                GetItems();
                BindBusinessUnit();
                if (InventoryType == (Int32)ProductType.GeneralInventory)
                {
                    BindShippingOptions();
                }
                ValidCart = true;
                BindRepeaterData();
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
                BaseResponseDto<EstimateDeliveryPricePayloadDto> estimation = null;
                var estimatedPrice = default(double);
                if (ValidCart)
                {
                    base.OnPreRender(e);
                    BindRepeaterData();
                    rptCartItems.ReloadData(true);
                    Visible = rptCartItems.Visible && !StopProcessing;
                    if (DataHelper.DataSourceIsEmpty(rptCartItems.DataSource))
                    {
                        Visible = false;
                        tblCartItems.Visible = false;
                        tblCartItems.Visible = false;
                    }
                }
                var inventoryType = Cart.GetValue("ShoppingCartInventoryType", default(int));
                if (inventoryType == (Int32)ProductType.GeneralInventory)
                {
                    if (Cart.ShippingOption != null && Cart.ShippingOption.ShippingOptionCarrierServiceName.ToLower() != ShippingOption.Ground)
                    {
                        estimation = GetShippingResponse();
                    }
                    if (estimation != null && estimation.Success)
                    {
                        estimatedPrice = ValidationHelper.GetDouble(estimation?.Payload?.Cost, default(double));
                    }
                }
                BindShippingDropdown(inventoryType, estimatedPrice);
                ShippingCost = estimatedPrice + EstimateSubTotal(inventoryType);
                var businessUnitID = Cart.GetValue("BusinessUnitIDForDistributor", default(string));
                ddlBusinessUnits.SelectedValue = businessUnitID;
                lblTotalPrice.Text = CurrencyInfoProvider.GetFormattedPrice(ShippingCost, CurrentSite.SiteID);
              
            }
            catch (Exception ex)
            {
                EventLogProvider.LogInformation("Kadena_CMSWebParts_Kadena_Cart_DistributorCartDetails", "OnPreRender", ex.Message);
            }
        }
        #endregion "Page events"

        #region "Event handling"
        /// <summary>
        /// Updates the Shipping option selected by the user
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlBusinessUnits_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                var businessUnitID = ValidationHelper.GetLong(ddlBusinessUnits.SelectedValue, default(long));
                if (CartID != default(int) && businessUnitID > 0)
                {
                    Cart = ShoppingCartInfoProvider.GetShoppingCartInfo(CartID);
                    if (Cart != null)
                    {
                        Cart.SetValue("BusinessUnitIDForDistributor", businessUnitID);
                        Cart.Update();
                    }
                }
            }
            catch (Exception ex)
            {
                EventLogProvider.LogInformation("Kadena_CMSWebParts_Kadena_Cart_DistributorCartDetails", "ddlBusinessUnits_SelectedIndexChanged", ex.Message);
            }
        }

        /// <summary>
        /// Updates the businessunit selected by the user
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlShippingOption_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                var shippingID = ValidationHelper.GetInteger(ddlShippingOption.SelectedValue, default(int));
                if (CartID != default(int) && shippingID > 0 && InventoryType == (int)ProductType.GeneralInventory)
                {
                    if (Cart != null)
                    {
                        Cart.ShoppingCartShippingOptionID = shippingID;
                        Cart.Update();
                    }
                }
            }
            catch (Exception ex)
            {
                EventLogProvider.LogInformation("Kadena_CMSWebParts_Kadena_Cart_DistributorCartDetails", "ddlShippingOption_SelectedIndexChanged", ex.Message);
            }
        }

        /// <summary>
        /// Save pdf click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkSaveasPDF_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable distributorCartData = CartPDFHelper.GetDistributorCartData(CartID, InventoryType);
                var pdfBytes = CartPDFHelper.CreateProductPDF(distributorCartData, InventoryType);
                CartPDFHelper.WriteresponseToPDF(pdfBytes);
            }
            catch (Exception ex)
            {
                EventLogProvider.LogInformation("Kadena_CMSWebParts_Kadena_Cart_DistributorCartDetails", "lnkSaveasPDF_Click", ex.Message);
            }
        }
        /// <summary>
        /// Removes the current cart item and the associated product options from the shopping cart.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void linkRemoveItem_Click(object sender, EventArgs e)
        {
            try
            {
                RepeaterItem item = (sender as LinkButton).Parent as RepeaterItem;
                int cartItemID = int.Parse((item.FindControl("lblCartItemID") as Label).Text);
                ShoppingCartItemInfoProvider.DeleteShoppingCartItemInfo(cartItemID);
                ShoppingCartInfoProvider.RemoveShoppingCartItem(Cart, cartItemID);
                if (Cart.CartItems.Count == 0)
                {
                    ShoppingCartInfoProvider.DeleteShoppingCartInfo(Cart.ShoppingCartID);
                }
                ShoppingCartInfoProvider.EvaluateShoppingCart(Cart);
                ComponentEvents.RequestEvents.RaiseEvent(sender, e, SHOPPING_CART_CHANGED);
                URLHelper.Redirect($"{Request.RawUrl}?status={QueryStringStatus.Deleted}");
            }
            catch (Exception ex)
            {
                EventLogProvider.LogInformation("Kadena_CMSWebParts_Kadena_Cart_RemoveItemFromCart", "Remove", ex.Message);
            }
        }
        #endregion "Event handling"

        #region "Private Methods"
        /// <summary>
        /// gets Shipping cost response
        /// </summary>
        /// <returns></returns>
        private BaseResponseDto<EstimateDeliveryPricePayloadDto> GetShippingResponse()
        {
            try
            {
                EstimateDeliveryPriceRequestDto estimationdto = ShoppingCartHelper.GetEstimationDTO(Cart);
                return ShoppingCartHelper.CallEstimationService(estimationdto);
            }
            catch (Exception ex)
            {
                EventLogProvider.LogInformation("Kadena_CMSWebParts_Kadena_Cart_DistributorCartDetails", "OnPreRender", ex.Message);
                return null;
            }
        }
        /// <summary>
        /// gets etimated subtotal
        /// </summary>
        /// <returns></returns>
        private double EstimateSubTotal(int inventoryType)
        {
            double price = 0;
            try
            {
                if (inventoryType == (Int32)ProductType.PreBuy)
                {
                    foreach (Control item in rptCartItems.Items)
                    {
                        var lblSKUPrice = item.Controls[0].FindControl("hdnSKUPrice") as HiddenField;
                        price += ValidationHelper.GetDouble(lblSKUPrice.Value, default(double));
                    }
                }
            }
            catch (Exception ex)
            {
                EventLogProvider.LogInformation("Kadena_CMSWebParts_Kadena_Cart_DistributorCartDetails", "EstimateSubTotal", ex.Message);
            }
            return price;
        }
        /// <summary>
        /// binds dropdown based on prodcut type
        /// </summary>
        /// <param name="inventoryType"></param>
        /// <param name="estimatedPrice"></param>
        private void BindShippingDropdown(int inventoryType, double estimatedPrice)
        {
            try
            {
                if (inventoryType == (Int32)ProductType.GeneralInventory)
                {
                    ddlShippingOption.SelectedValue = ValidationHelper.GetString(Cart.ShoppingCartShippingOptionID, default(string));
                }
                else
                {
                    ddlShippingOption.Visible = false;
                    lblShippingOption.Visible = true;
                    lblShippingOption.Text = ResHelper.GetString("KDA.DistributorCart.GroundShippingOptionText");
                }
                lblShippingCharge.Text = CurrencyInfoProvider.GetFormattedPrice(estimatedPrice, CurrentSite.SiteID);
            }
            catch (Exception ex)
            {
                EventLogProvider.LogInformation("Kadena_CMSWebParts_Kadena_Cart_DistributorCartDetails", "BindShippingDropdown", ex.Message);
            }
        }

        /// <summary>
        /// This will get Business Units
        /// </summary>
        private void GetItems()
        {
            try
            {
                if (BusinessUnits == null)
                {
                    BusinessUnits = CustomTableItemProvider.GetItems<BusinessUnitItem>()
                                    .Source(sourceItem => sourceItem.Join<UserBusinessUnitsItem>("ItemID", "BusinessUnitID"))
                                    .WhereEquals("UserID", CurrentUser.UserID).WhereEquals("SiteID", CurrentSite.SiteID)
                                    .WhereTrue("Status").Columns("BusinessUnitNumber,BusinessUnitName").ToList();
                }
            }
            catch (Exception ex)
            {
                EventLogProvider.LogInformation("Kadena_CMSWebParts_Kadena_Cart_DistributorCartDetails", "BindRepeaterData", ex.Message);
            }
        }

        /// <summary>
        /// This will bind the data to repeater
        /// </summary>
        private void BindRepeaterData()
        {
            try
            {
                rptCartItems.CacheMinutes = 0;
                QueryDataParameters parameters = new QueryDataParameters();
                parameters.Add("@CartItemDistributorID", ShoppingCartDistributorID);
                parameters.Add("@ShoppingCartInventoryType", InventoryType);
                rptCartItems.QueryParameters = parameters;
                rptCartItems.QueryName = SQLQueries.shoppingCartCartItems;
            }
            catch (Exception ex)
            {
                EventLogProvider.LogInformation("Kadena_CMSWebParts_Kadena_Cart_DistributorCartDetails", "BindRepeaterData", ex.Message);
            }
        }

        /// <summary>
        /// This will bind Shipping options to dropdown
        /// </summary>
        private void BindShippingOptions()
        {
            try
            {
                if (ShippingOptions == null)
                {
                    ShippingOptions = ShippingOptionInfoProvider.GetShippingOptions()
                                                .OnSite(CurrentSite.SiteID).Where(x => x.ShippingOptionEnabled == true).ToList();
                }
                ddlShippingOption.DataSource = ShippingOptions;
                ddlShippingOption.DataValueField = "ShippingOptionID";
                ddlShippingOption.DataTextField = "ShippingOptionName";
                ddlShippingOption.DataBind();
            }
            catch (Exception ex)
            {
                EventLogProvider.LogInformation("Kadena_CMSWebParts_Kadena_Cart_DistributorCartDetails", "BindShippingOptions", ex.Message);
            }
        }

        /// <summary>
        /// This will bind business units to dropdown
        /// </summary>
        private void BindBusinessUnit()
        {
            try
            {
                if (BusinessUnits != null && BusinessUnits.Count > 0)
                {
                    ddlBusinessUnits.DataSource = BusinessUnits;
                    ddlBusinessUnits.DataValueField = "BusinessUnitNumber";
                    ddlBusinessUnits.DataTextField = "BusinessUnitName";
                    ddlBusinessUnits.DataBind();

                    Cart.SetValue("BusinessUnitIDForDistributor", BusinessUnits.FirstOrDefault().BusinessUnitNumber);
                    Cart.Update();
                }
            }
            catch (Exception ex)
            {
                EventLogProvider.LogInformation("Kadena_CMSWebParts_Kadena_Cart_DistributorCartDetails", "BindBusinessUnit", ex.Message);
            }
        }
        #endregion "Private Methods"
      
    }
}