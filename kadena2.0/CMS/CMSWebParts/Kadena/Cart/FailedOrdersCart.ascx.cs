using CMS.CustomTables;
using CMS.CustomTables.Types.KDA;
using CMS.DataEngine;
using CMS.DocumentEngine.Types.KDA;
using CMS.Ecommerce;
using CMS.Ecommerce.Web.UI;
using CMS.EventLog;
using CMS.Helpers;
using CMS.Membership;
using Kadena.Dto.EstimateDeliveryPrice.MicroserviceResponses;
using Kadena.Dto.General;
using Kadena.Models.BusinessUnit;
using Kadena.Old_App_Code.Kadena.Constants;
using Kadena.Old_App_Code.Kadena.Enums;
using Kadena.Old_App_Code.Kadena.Shoppingcart;
using Kadena.WebAPI.KenticoProviders.Contracts;
using Kadena.Container.Default;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Kadena.CMSWebParts.Kadena.Cart
{
    public partial class FailedOrdersCart : CMSCheckoutWebPart
    {
        private const string _serviceUrlSettingKey = "KDA_ShippingCostServiceUrl";
        private IShoppingCartProvider _shoppingCart;
        private IKenticoBusinessUnitsProvider _businessUnit;
        #region "Private Properties"

        private List<BusinessUnit> BusinessUnits { get; set; }
        private List<ShippingOptionInfo> ShippingOptions { get; set; }

        #endregion "Private Properties"

        #region "Public properties"

        public double ShippingCost { get; set; }
        public bool ValidCart { get; set; }
        public ShoppingCartInfo Cart { get; set; }

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
        /// <summary>
        /// gets or sets open campaign
        /// </summary>
        public Campaign OpenCampaign
        {
            get
            {
                return ShoppingCartHelper.GetOpenCampaign();
            }
            set
            {
                SetValue("OpenCampaign", value);
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
                _shoppingCart = DIContainer.Resolve<IShoppingCartProvider>();
                _businessUnit = DIContainer.Resolve<IKenticoBusinessUnitsProvider>();
                if (AuthenticationHelper.IsAuthenticated() && !IsPostBack)
                {
                    int campaignID = QueryHelper.GetInteger("campid", 0);
                    if (campaignID > 0)
                    {
                        BindData(campaignID);
                    }
                }
            }
            catch (Exception ex)
            {
                EventLogProvider.LogInformation("Kadena_CMSWebParts_Kadena_Cart_FailedOrdersCart", "Page_Load", ex.Message);
            }
        }
        #endregion "Page events"
        #region "Private Methods"
        /// <summary>
        /// binds repeater data
        /// </summary>
        /// <param name="campaignID"></param>
        private void BindData(int campaignID)
        {
            try
            {
                Cart = _shoppingCart.GetShoppingCartByID(CartID);
                GetItems();
                BindBusinessUnit();
                ValidCart = true;
                BindRepeaterData(campaignID);
                var inventoryType = Cart.GetValue("ShoppingCartInventoryType", default(int));
                BindShipping();
                ShippingCost = EstimateSubTotal();
                lblTotalPrice.Text = CurrencyInfoProvider.GetFormattedPrice(ShippingCost, CurrentSite.SiteID);
                var businessUnitID = Cart.GetValue("BusinessUnitIDForDistributor", default(string));
                ddlBusinessUnits.SelectedValue = businessUnitID;
                ddlBusinessUnits.Enabled = false;
            }
            catch (Exception ex)
            {
                EventLogProvider.LogInformation("Kadena_CMSWebParts_Kadena_Cart_FailedOrdersCart", "Page_Load", ex.Message);
            }
        }
        /// <summary>
        /// gets etimated subtotal
        /// </summary>
        /// <returns></returns>
        private double EstimateSubTotal()
        {
            double price = 0;
            try
            {
                foreach (Control item in rptCartItems.Items)
                {
                    var lblSKUPrice = item.Controls[0].FindControl("hdnSKUPrice") as HiddenField;
                    price += ValidationHelper.GetDouble(lblSKUPrice.Value, default(double));
                }
            }
            catch (Exception ex)
            {
                EventLogProvider.LogInformation("Kadena_CMSWebParts_Kadena_Cart_FailedOrdersCart", "EstimateSubTotal", ex.Message);
            }
            return price;
        }
        /// <summary>
        /// binds dropdown based on prodcut type
        /// </summary>
        /// <param name="inventoryType"></param>
        /// <param name="estimatedPrice"></param>
        private void BindShipping()
        {
            try
            {

                lblShippingOption.Text = ResHelper.GetString("KDA.DistributorCart.GroundShippingOptionText");
                lblShippingCharge.Text = CurrencyInfoProvider.GetFormattedPrice(default(double), CurrentSite.SiteID);
            }
            catch (Exception ex)
            {
                EventLogProvider.LogInformation("Kadena_CMSWebParts_Kadena_Cart_FailedOrdersCart", "BindShippingDropdown", ex.Message);
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
                    BusinessUnits = _businessUnit.GetUserBusinessUnits(CurrentUser.UserID);
                }
            }
            catch (Exception ex)
            {
                EventLogProvider.LogInformation("Kadena_CMSWebParts_Kadena_Cart_FailedOrdersCart", "GetItems", ex.Message);
            }
        }

        /// <summary>
        /// This will bind the data to repeater
        /// </summary>
        private void BindRepeaterData(int campaignID)
        {
            try
            {
                rptCartItems.CacheMinutes = 0;
                QueryDataParameters parameters = new QueryDataParameters();
                parameters.Add("@CartItemDistributorID", ShoppingCartDistributorID);
                parameters.Add("@ShoppingCartInventoryType", InventoryType);
                parameters.Add("@ShoppingCartCampaignID", campaignID);
                rptCartItems.QueryParameters = parameters;
                rptCartItems.QueryName = SQLQueries.shoppingCartCartItems;
            }
            catch (Exception ex)
            {
                EventLogProvider.LogInformation("Kadena_CMSWebParts_Kadena_Cart_FailedOrdersCart", "BindRepeaterData", ex.Message);
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
                EventLogProvider.LogInformation("Kadena_CMSWebParts_Kadena_Cart_FailedOrdersCart", "BindBusinessUnit", ex.Message);
            }
        }
    }
    #endregion
}