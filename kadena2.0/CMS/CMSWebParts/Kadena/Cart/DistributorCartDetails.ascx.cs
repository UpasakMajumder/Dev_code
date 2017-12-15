using CMS.CustomTables;
using CMS.CustomTables.Types.KDA;
using CMS.DataEngine;
using CMS.Ecommerce;
using CMS.Ecommerce.Web.UI;
using CMS.EventLog;
using CMS.Helpers;
using Kadena.Old_App_Code.Kadena.Constants;
using Kadena.Old_App_Code.Kadena.Enums;
using Kadena.Old_App_Code.Kadena.PDFHelpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Kadena.CMSWebParts.Kadena.Cart
{
    public partial class DistributorCartDetails : CMSCheckoutWebPart
    {
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
                    var quantity = 0;
                    double price = 0;
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
                    foreach (Control item in rptCartItems.Items)
                    {
                        var txtQuantity = item.Controls[0].FindControl("txtUnits") as TextBox;
                        quantity += ValidationHelper.GetInteger(txtQuantity.Text, default(int));
                        var lblSKUPrice = item.Controls[0].FindControl("hdnSKUPrice") as HiddenField;
                        price += ValidationHelper.GetDouble(lblSKUPrice.Value, default(double));
                    }
                    var inventoryType = Cart.GetValue("ShoppingCartInventoryType", default(int));
                    var businessUnitID = Cart.GetValue("BusinessUnitIDForDistributor", default(string));
                    if (inventoryType == (Int32)ProductType.GeneralInventory)
                    {
                        ddlShippingOption.SelectedValue = ValidationHelper.GetString(Cart.ShoppingCartShippingOptionID, default(string));
                        lblTotalPrice.Text = CurrencyInfoProvider.GetFormattedPrice(ShippingCost, CurrentSite.SiteID);
                    }
                    else
                    {
                        ddlShippingOption.Items[0].Selected = true;
                        ddlShippingOption.Attributes["disabled"] = "disabled";
                        lblTotalPrice.Text = CurrencyInfoProvider.GetFormattedPrice(price, CurrentSite.SiteID);
                    }
                    ddlBusinessUnits.SelectedValue = businessUnitID;
                }
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
                        ValidCart = false;
                        return;
                    }
                }
                Cart.ShoppingCartShippingOptionID = ValidationHelper.GetValue<int>(ddlShippingOption.SelectedValue);
                Cart.SetValue("BusinessUnitIDForDistributor", ddlBusinessUnits.SelectedValue);
                ShoppingCartInfoProvider.SetShoppingCartInfo(Cart);
                lblCartUpdateSuccess.Text = ResHelper.GetString("KDA.DistributorCart.CartUpdateSuccessMessage");
            }
            catch (Exception ex)
            {
                EventLogProvider.LogInformation("Kadena_CMSWebParts_Kadena_Cart_DistributorCartDetails", "btnSaveCartItems_Click", ex.Message);
            }
        }
        /// <summary>
        /// Save pdf click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkSaveasPDF_Click(object sender, EventArgs e)
        {
            DataTable distributorCartData = CartPDFHelper.GetDistributorCartData(CartID, InventoryType);
            CreateDistributorCartPDF(distributorCartData);
        }
        #endregion "Event handling"

        #region "Private Methods"

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
                                                .OnSite(CurrentSite.SiteID).Where(x=>x.ShippingOptionEnabled==true).ToList();
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
        /// This will returns Shipping price based on Shipping option
        /// </summary>
        /// <returns></returns>
        private double GetPriceByShippingOption()
        {
            try
            {
                var shippingOption = ShippingOptions.Where(x => x.ShippingOptionID == ValidationHelper.GetValue<int>(ddlShippingOption.SelectedValue)).FirstOrDefault();
                var costs = ShippingCostInfoProvider.GetShippingCosts().WhereEquals("ShippingCostShippingOptionID", ValidationHelper.GetValue<int>(ddlShippingOption.SelectedValue)).ToList();
                return default(double);
            }
            catch (Exception ex)
            {
                EventLogProvider.LogInformation("Kadena_CMSWebParts_Kadena_Cart_DistributorCartDetails", "GetPriceByShippingOption ", ex.Message);
                return default(double);
            }
        }

        #endregion "Private Methods"
        /// <summary>
        /// This will create cart Pdf o download
        /// </summary>
        /// <param name="distributorCartData"></param>
        private void CreateDistributorCartPDF(DataTable distributorCartData)
        {
            try
            {
                var html = SettingsKeyInfoProvider.GetValue($@"{CurrentSiteName}.KDA_DistributorCartPDFHTML");
                var groupCart = distributorCartData.AsEnumerable().GroupBy(x => x["ShoppingCartID"]);
                var PDFBody = "";
                foreach (var cart in groupCart)
                {
                    PDFBody += CartPDFHelper.CreateCarOuterContent(cart.FirstOrDefault(), CurrentSiteName);
                    var cartData = cart.ToList();
                    PDFBody = PDFBody.Replace("{INNERCONTENT}", CartPDFHelper.CreateCartInnerContent(cartData, CurrentSiteName));
                   html= html.Replace("{PDFNAME}", $"{(cart.FirstOrDefault())["AddressPersonalName"].ToString()}");
                }
                html = html.Replace("{OUTERCONTENT}", PDFBody);
                var pdfBytes = (new NReco.PdfGenerator.HtmlToPdfConverter()).GeneratePdf(html);
                string fileName = "test" + DateTime.Now.Ticks + ".pdf";
                Response.Clear();
                MemoryStream ms = new MemoryStream(pdfBytes);
                Response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition", "attachment;filename=" + fileName);
                Response.Buffer = true;
                ms.WriteTo(Response.OutputStream);
                Response.End();
            }
            catch (Exception ex)
            {
                EventLogProvider.LogInformation("CartPDFHelper", "CreateCarOuterContent", ex.Message);
            }
        }
    }
}