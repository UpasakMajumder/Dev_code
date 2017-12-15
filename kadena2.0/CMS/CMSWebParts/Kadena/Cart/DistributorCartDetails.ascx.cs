using CMS.CustomTables;
using CMS.CustomTables.Types.KDA;
using CMS.DataEngine;
using CMS.Ecommerce;
using CMS.Ecommerce.Web.UI;
using CMS.EventLog;
using CMS.Globalization;
using CMS.Helpers;
using CMS.SiteProvider;
using Kadena.Dto.EstimateDeliveryPrice.MicroserviceRequests;
using Kadena.Dto.EstimateDeliveryPrice.MicroserviceResponses;
using Kadena.Dto.General;
using Kadena.Helpers;
using Kadena.Old_App_Code.Kadena.Constants;
using Kadena.Old_App_Code.Kadena.Enums;
using Kadena.WebAPI.KenticoProviders;
using Kadena2.MicroserviceClients.Clients;
using Kadena2.MicroserviceClients.Contracts.Base;
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
        private readonly IMicroProperties _properties;

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
        /// Gets or sets the Save.
        /// </summary>
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
                GetShippingOptions();
                BindRepeaterData();
                BindBusinessUnit();
                BindShippingOptions();
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
                var txtQuantity = Cart.CartItems.Sum(x => x.CartItemUnits);
                var estimation = CallEstimationService(GetEstimationDTO());
                var estimatedPrice = ValidationHelper.GetDouble(estimation?.Payload?.Cost, default(double));
                var inventoryType = Cart.GetValue("ShoppingCartInventoryType", default(int));
                SelectShippingoption(inventoryType, estimatedPrice);
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
        /// Save Cart items event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSaveCartItems_Click(object sender, EventArgs e)
        {
            try
            {
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
                        lblCartError.Text = ResHelper.GetString("KDA.DistributorCart.QuantityError");
                        divDailogue.Attributes.Add("class", "dialog active");
                        ValidCart = false;
                        return;
                    }
                }
                Cart.ShoppingCartShippingOptionID = ValidationHelper.GetValue<int>(ddlShippingOption.SelectedValue);
                Cart.SetValue("BusinessUnitIDForDistributor", ddlBusinessUnits.SelectedValue);
                ShoppingCartInfoProvider.SetShoppingCartInfo(Cart);
                lblCartUpdateSuccess.Text = ResHelper.GetString("KDA.DistributorCart.CartUpdateSuccessMessage");
                divDailogue.Attributes.Add("class", "dialog active");
            }
            catch (Exception ex)
            {
                EventLogProvider.LogInformation("Kadena_CMSWebParts_Kadena_Cart_DistributorCartDetails", "btnSaveCartItems_Click", ex.Message);
            }
        }

        #endregion "Event handling"

        #region "Private Methods"
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
        private void SelectShippingoption(int inventoryType, double estimatedPrice)
        {
            try
            {
                if (inventoryType == (Int32)ProductType.GeneralInventory)
                {
                    ddlShippingOption.SelectedValue = ValidationHelper.GetString(Cart.ShoppingCartShippingOptionID, default(string));
                    lblShippingCharge.Text = CurrencyInfoProvider.GetFormattedPrice(estimatedPrice, CurrentSite.SiteID);
                }
                else
                {
                    ddlShippingOption.Items[0].Selected = true;
                    ddlShippingOption.Attributes["disabled"] = "disabled";
                    lblShippingCharge.Text = CurrencyInfoProvider.GetFormattedPrice(estimatedPrice, CurrentSite.SiteID);
                }
            }
            catch (Exception ex)
            {
                EventLogProvider.LogInformation("Kadena_CMSWebParts_Kadena_Cart_DistributorCartDetails", "SelectShippingoption", ex.Message);
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
        /// This will get shipping options
        /// </summary>
        private void GetShippingOptions()
        {
            try
            {
                if (ShippingOptions == null)
                {
                    ShippingOptions = ShippingOptionInfoProvider.GetShippingOptions()
                                                .OnSite(CurrentSite.SiteID).Where(x => x.ShippingOptionEnabled == true).ToList();
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
                rptCartItems.QueryParameters = parameters;
                rptCartItems.QueryName = SQLQueries.shoppingCartCartItems;
                rptCartItems.TransformationName = TransformationNames.cartItems;
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
                ddlBusinessUnits.DataSource = BusinessUnits;
                ddlBusinessUnits.DataValueField = "BusinessUnitNumber";
                ddlBusinessUnits.DataTextField = "BusinessUnitName";
                ddlBusinessUnits.DataBind();
            }
            catch (Exception ex)
            {
                EventLogProvider.LogInformation("Kadena_CMSWebParts_Kadena_Cart_DistributorCartDetails", "BindBusinessUnit", ex.Message);
            }
        }

        /// <summary>
        /// Calling shipping estimation service
        /// </summary>
        /// <param name="requestBody"></param>
        /// <returns></returns>
        private BaseResponseDto<EstimateDeliveryPricePayloadDto> CallEstimationService(EstimateDeliveryPriceRequestDto requestBody)
        {
            try
            {
                var microserviceClient = new ShippingCostServiceClient(new MicroProperties(new KenticoResourceService()));
                var response = microserviceClient.EstimateShippingCost(requestBody).Result;

                if (!response.Success || response.Payload == null)
                {
                    EventLogProvider.LogInformation("DeliveryPriceEstimationClient", "ERROR", $"Call from '{Cart.ShippingOption.ShippingOptionName}' provider to service URL '{_properties.GetServiceUrl(_serviceUrlSettingKey)}' resulted with error {response.Error?.Message ?? string.Empty}");
                }
                return response;
            }
            catch (Exception ex)
            {
                EventLogProvider.LogInformation("Kadena_CMSWebParts_Kadena_Cart_DistributorCartDetails", "CallEstimationService", ex.Message);
                return null;
            }
        }

        /// <summary>
        /// getting total weight
        /// </summary>
        /// <returns></returns>
        private WeightDto GetWeight()
        {
            try
            {
                var weight = Cart.CartItems.Sum(x => (x.CartItemUnits * x.UnitWeight));
                var weight2 = Cart.TotalItemsWeight; ;
                return new WeightDto { Unit = "Kg", Value = weight };
            }
            catch (Exception ex)
            {
                EventLogProvider.LogInformation("Kadena_CMSWebParts_Kadena_Cart_DistributorCartDetails", "GetWeight", ex.Message);
                return null;
            }
        }

        /// <summary>
        /// creating estimation DTO
        /// </summary>
        /// <returns></returns>
        private EstimateDeliveryPriceRequestDto GetEstimationDTO()
        {
            try
            {
                return new EstimateDeliveryPriceRequestDto
                {
                    SourceAddress = GetSourceAddressFromConfig(),
                    TargetAddress = GetTargetAddress(),
                    Weight = GetWeight(),
                    Provider = Cart.ShippingOption.ShippingOptionName,
                    ProviderService = Cart.ShippingOption.ShippingOptionCarrierServiceName
                };
            }
            catch (Exception ex)
            {
                EventLogProvider.LogInformation("Kadena_CMSWebParts_Kadena_Cart_DistributorCartDetails", "GetEstimationDTO", ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Gets target shipping address
        /// </summary>
        /// <returns></returns>
        private AddressDto GetTargetAddress()
        {
            try
            {
                var distributorID = Cart.GetIntegerValue("ShoppingCartDistributorID", default(int));
                var distributorAddress = AddressInfoProvider.GetAddresses().WhereEquals("AddressID", distributorID).FirstOrDefault();
                var addressLines = new[]{
                                            distributorAddress.GetStringValue("AddressLine1",string.Empty),
                                            distributorAddress.GetStringValue("AddressLine2",string.Empty)
                                        }.Where(a => !string.IsNullOrWhiteSpace(a)).ToList();
                var country = CountryInfoProvider.GetCountries().WhereEquals("CountryID", distributorAddress.GetStringValue("AddressCountryID", string.Empty))
                                    .Column("CountryTwoLetterCode").FirstOrDefault();
                var state = StateInfoProvider.GetStates().WhereEquals("StateID", distributorAddress.GetStringValue("AddressStateID", string.Empty)).Column("StateCode").FirstOrDefault();
                return new AddressDto()
                {
                    City = distributorAddress.GetStringValue("AddressCity", string.Empty),
                    Country = country.CountryTwoLetterCode,
                    Postal = distributorAddress.GetStringValue("AddressZip", string.Empty),
                    State = state.StateCode,
                    StreetLines = addressLines
                };
            }
            catch (Exception ex)
            {
                EventLogProvider.LogInformation("Kadena_CMSWebParts_Kadena_Cart_DistributorCartDetails", "GetTargetAddress", ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Gets source  address
        /// </summary>
        /// <returns></returns>
        private AddressDto GetSourceAddressFromConfig()
        {
            try
            {
                var addressLines = new[]
                                    {
                                        SettingsKeyInfoProvider.GetValue(SiteContext.CurrentSiteName + ".KDA_EstimateDeliveryPrice_SenderAddressLine1"),
                                        SettingsKeyInfoProvider.GetValue(SiteContext.CurrentSiteName + ".KDA_EstimateDeliveryPrice_SenderAddressLine2")
                                    }.Where(a => !string.IsNullOrWhiteSpace(a)).ToList();

                return new AddressDto()
                {
                    City = SettingsKeyInfoProvider.GetValue(SiteContext.CurrentSiteName + ".KDA_EstimateDeliveryPrice_SenderCity"),
                    Country = SettingsKeyInfoProvider.GetValue(SiteContext.CurrentSiteName + ".KDA_EstimateDeliveryPrice_SenderCountry"),
                    Postal = SettingsKeyInfoProvider.GetValue(SiteContext.CurrentSiteName + ".KDA_EstimateDeliveryPrice_SenderPostal"),
                    State = SettingsKeyInfoProvider.GetValue(SiteContext.CurrentSiteName + ".KDA_EstimateDeliveryPrice_SenderState"),
                    StreetLines = addressLines
                };
            }
            catch (Exception ex)
            {
                EventLogProvider.LogInformation("Kadena_CMSWebParts_Kadena_Cart_DistributorCartDetails", "GetSourceAddressFromConfig", ex.Message);
                return null;
            }
        }

        #endregion "Private Methods"
    }
}