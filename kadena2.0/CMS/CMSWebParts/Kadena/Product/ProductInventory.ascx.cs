using CMS.CustomTables;
using CMS.CustomTables.Types.KDA;
using CMS.DataEngine;
using CMS.DocumentEngine;
using CMS.DocumentEngine.Types.KDA;
using CMS.Ecommerce;
using CMS.EventLog;
using CMS.Helpers;
using CMS.MediaLibrary;
using CMS.PortalEngine.Web.UI;
using Kadena.Models;
using Kadena.Old_App_Code.Kadena.Constants;
using Kadena.WebAPI.KenticoProviders.Contracts;
using Kadena2.Container.Default;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI.WebControls;

public partial class CMSWebParts_Kadena_Product_ProductInventory : CMSAbstractWebPart
{
    #region "Properties"

    /// <summary>
    /// Get the Product type.
    /// </summary>
    public int ProductType
    {
        get
        {
            return ValidationHelper.GetInteger(GetValue("ProductType"), default(int));
        }
        set
        {
            SetValue("ProductType", value);
        }
    }
    /// <summary>
    /// get the open campaign
    /// </summary>
    public CMS.DocumentEngine.Types.KDA.Campaign OpenCampaign
    {
        get
        {
            return CampaignProvider.GetCampaigns().Columns("CampaignID,Name,StartDate,EndDate")
                                .WhereEquals("OpenCampaign", true)
                                .Where(new WhereCondition().WhereEquals("CloseCampaign", false).Or()
                                .WhereEquals("CloseCampaign", null))
                                .WhereEquals("NodeSiteID", CurrentSite.SiteID).FirstOrDefault();
        }
        set
        {
            SetValue("OpenCampaign", value);
        }
    }
    // <summary>
    /// Get the Product type.
    /// </summary>
    public int ShippingID
    {
        get
        {
            return ValidationHelper.GetInteger(GetValue("ShippingID"), default(int));
        }
        set
        {
            SetValue("ShippingID", value);
        }
    }

    /// <summary>
    /// Add to cart Link text
    /// </summary>
    public string AddToCartLinkText
    {
        get
        {
            return ValidationHelper.GetString(ResHelper.GetString("Kadena.Product.AddToCartText"), string.Empty);
        }
        set
        {
            SetValue("AddToCartLinkText", value);
        }
    }
    /// <summary>
    /// NoDataText Link text
    /// </summary>
    public string NoDataText
    {
        get
        {
            return ResHelper.GetString("Kadena.ItemList.NoDataFoundText");
        }
        set
        {
            SetValue("NoDataText", value);
        }
    }

    /// <summary>
    /// No campaign open text
    /// </summary>
    public string NoCampaignOpen
    {
        get
        {
            return ResHelper.GetString("Kadena.ItemList.NoCampaignOpen");
        }
        set
        {
            SetValue("NoCampaignOpen", value);
        }
    }
    /// <summary>
    /// Search placeholder text
    /// </summary>
    public string PosSearchPlaceholder
    {
        get
        {
            return ValidationHelper.GetString(ResHelper.GetString("Kadena.Product.PosSearchPlaceholderText"), string.Empty);
        }
        set
        {
            SetValue("PosSearchPlaceholder", value);
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
    /// <summary>
    /// Loads the addressid column heading
    /// </summary>
    public string AddressIDText
    {
        get
        {
            return ValidationHelper.GetString(ResHelper.GetString("KDA.ShoppingCart.StoreID"), string.Empty);
        }
    }
    /// <summary>
    /// Loads the AddressPersonalName column heading
    /// </summary>
    public string AddressPersonalNameText
    {
        get
        {
            return ValidationHelper.GetString(ResHelper.GetString("KDA.ShoppingCart.CustomerName"), string.Empty);
        }
    }
    /// <summary>
    /// Loads the CartCloseText button text
    /// </summary>
    public string CartCloseText
    {
        get
        {
            return ValidationHelper.GetString(ResHelper.GetString("KDA.ShoppingCart.DiscardChanges"), string.Empty);
        }
    }

    /// <summary>
    /// Checks whether the start an dend dates of campaign are in range
    /// </summary>
    public bool EnableAddToCart
    {
        get
        {
            if (OpenCampaign != null)
            {
                return OpenCampaign.StartDate <= DateTime.Now.Date && OpenCampaign.EndDate >= DateTime.Now.Date ? true : false;
            }
            else
            {
                return false;
            }
        }
        set
        {
            SetValue("EnableAddToCart", value);
        }
    }
    #endregion "Properties"

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
            if (!IsPostBack)
            {
                divNoRecords.Visible = false;
                txtPos.Attributes.Add("placeholder", PosSearchPlaceholder);
                BindPrograms();
                BindCategories();
                BindBrands();
                BindData();
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
    /// Get product Image by Image path
    /// </summary>
    /// <param name="imagepath"></param>
    /// <returns></returns>
    public string GetProductImage(object imagepath)
    {
        string returnValue = string.Empty;
        try
        {
            if (ProductType == (int)ProductsType.PreBuy)
            {
                string folderName = SettingsKeyInfoProvider.GetValue(CurrentSite.SiteName + ".KDA_ImagesFolderName");
                folderName = !string.IsNullOrEmpty(folderName) ? folderName.Replace(" ", "") : "CampaignProducts";
                if (imagepath != null && folderName != null)
                {
                    returnValue = MediaFileURLProvider.GetMediaFileAbsoluteUrl(CurrentSiteName, folderName, ValidationHelper.GetString(imagepath, string.Empty));
                }
            }
            else
            {
                string folderName = SettingsKeyInfoProvider.GetValue(CurrentSite.SiteName + ".KDA_InventoryProductImageFolderName");
                folderName = !string.IsNullOrEmpty(folderName) ? folderName.Replace(" ", "") : "InventoryProducts";
                if (imagepath != null && folderName != null)
                {
                    returnValue = MediaFileURLProvider.GetMediaFileAbsoluteUrl(CurrentSiteName, folderName, ValidationHelper.GetString(imagepath, string.Empty));
                }
            }
        }
        catch (Exception ex)
        {
            EventLogProvider.LogException("Get Product Image", "GetProductImage", ex, CurrentSite.SiteID, ex.Message);
        }
        return string.IsNullOrEmpty(returnValue) ? SettingsKeyInfoProvider.GetValue($@"{CurrentSiteName}.KDA_ProductsPlaceHolderImage") : returnValue;
    }

    /// <summary>
    /// Get Product details
    /// </summary>
    /// <param name="programID"></param>
    /// <param name="categoryID"></param>
    /// <param name="posNumber"></param>
    /// <returns></returns>
    public List<CampaignsProduct> GetProductsDetails(int programID = default(int), int categoryID = default(int), string posNumber = null, int brandID = default(int))
    {
        List<CampaignsProduct> productsDetails = new List<CampaignsProduct>();
        try
        {
            if (ProductType != default(int))
            {
                if (ProductType == (int)ProductsType.GeneralInventory)
                {
                    ddlCategory.Visible = true;
                    ddlBrand.Visible = true;
                    productsDetails = CampaignsProductProvider.GetCampaignsProducts()
                                      .WhereEquals("NodeSiteID", CurrentSite.SiteID)
                                      .WhereEquals("ProgramID", null)
                                      .ToList();
                    if (!DataHelper.DataSourceIsEmpty(productsDetails))
                    {
                        if (categoryID != default(int))
                        {
                            productsDetails = productsDetails
                                .Where(x => x.CategoryID == categoryID)
                                .ToList();
                        }
                        if (brandID != default(int))
                        {
                            productsDetails = productsDetails
                                .Where(x => x.BrandID == brandID)
                                .ToList();
                        }
                    }
                }
                else if (ProductType == (int)ProductsType.PreBuy)
                {
                    ddlProgram.Visible = true;
                    ddlCategory.Visible = true;
                    List<int> programIds = GetProgramIDs();
                    if (!DataHelper.DataSourceIsEmpty(programIds))
                    {
                        productsDetails = CampaignsProductProvider.GetCampaignsProducts()
                                          .WhereEquals("NodeSiteID", CurrentSite.SiteID)
                                          .WhereIn("ProgramID", programIds)
                                          .ToList();
                        if (!DataHelper.DataSourceIsEmpty(productsDetails))
                        {
                            if (programID != default(int))
                            {
                                productsDetails = productsDetails
                                    .Where(x => x.ProgramID == programID)
                                    .ToList();
                            }
                            if (categoryID != default(int))
                            {
                                productsDetails = productsDetails
                                    .Where(x => x.CategoryID == categoryID)
                                    .ToList();
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            EventLogProvider.LogException("Get Product Details", "GetProductsDetails()", ex, CurrentSite.SiteID, ex.Message);
        }
        return productsDetails;
    }

    private void BindBrands()
    {
        try
        {
            var brands = CustomTableItemProvider.GetItems<BrandItem>()
                            .Columns("ItemId,BrandName")
                            .OrderBy("BrandName")
                            .ToList();
            if (!DataHelper.DataSourceIsEmpty(brands))
            {
                ddlBrand.DataSource = brands;
                ddlBrand.DataTextField = "BrandName";
                ddlBrand.DataValueField = "ItemId";
                ddlBrand.DataBind();
                string selectText = ValidationHelper.GetString(ResHelper.GetString("Kadena.CampaignProduct.SelectBrandText"), string.Empty);
                ddlBrand.Items.Insert(0, new ListItem(selectText, "0"));
            }
        }
        catch (Exception ex)
        {
            EventLogProvider.LogException("Bind Brands to Dropdown", "BindBrands()", ex, CurrentSite.SiteID, ex.Message);
        }
    }

    /// <summary>
    /// Get Skudetails
    /// </summary>
    /// <param name="productsDetails"></param>
    /// <returns></returns>
    public List<SKUInfo> GetSkuDetails(List<CampaignsProduct> productsDetails)
    {
        List<SKUInfo> skuDetails = new List<SKUInfo>();
        try
        {
            List<int> skuIds = productsDetails.Select(x => x.NodeSKUID).ToList<int>();
            if (!DataHelper.DataSourceIsEmpty(skuIds))
            {
                skuDetails = SKUInfoProvider.GetSKUs()
                               .WhereIn("SKUID", skuIds)
                               .And()
                               .WhereEquals("SKUEnabled", true)
                               .Columns("SKUProductCustomerReferenceNumber,SKUNumber,SKUName,SKUPrice,SKUEnabled,SKUImagePath,SKUAvailableItems,SKUID,SKUDescription")
                               .ToList();
            }
        }
        catch (Exception ex)
        {
            EventLogProvider.LogException("Get Sku Details", "GetSkuDetails()", ex, CurrentSite.SiteID, ex.Message);
        }
        return skuDetails;
    }

    /// <summary>
    /// Bind the Products data to repeater
    /// </summary>
    /// <param name="programID"></param>
    /// <param name="categoryID"></param>
    public void BindData(int programID = default(int), int categoryID = default(int), string posNumber = null, int brandID = default(int))
    {
        try
        {
            divNoRecords.Visible = false;
            rptProductLists.DataSource = null;
            rptProductLists.DataBind();
            List<CampaignsProduct> productsDetails = GetProductsDetails(programID, categoryID, posNumber, brandID);
            if (!DataHelper.DataSourceIsEmpty(productsDetails))
            {
                List<SKUInfo> skuDetails = GetSkuDetails(productsDetails);
                if (!string.IsNullOrEmpty(posNumber) && !string.IsNullOrWhiteSpace(posNumber) && !DataHelper.DataSourceIsEmpty(skuDetails))
                {
                    skuDetails = skuDetails
                                 .Where(x => x.GetStringValue("SKUProductCustomerReferenceNumber", string.Empty).ToLower().Contains(posNumber.ToLower()))
                                 .ToList();
                }
                if (!DataHelper.DataSourceIsEmpty(skuDetails) && !DataHelper.DataSourceIsEmpty(productsDetails))
                {
                    var productAndSKUDetails = productsDetails
                          .Join(skuDetails, x => x.NodeSKUID, y => y.SKUID, (x, y) => new { x.ProgramID, x.CategoryID, x.QtyPerPack, x.EstimatedPrice, y.SKUNumber, x.Product.SKUProductCustomerReferenceNumber, y.SKUName, y.SKUPrice, y.SKUEnabled, y.SKUImagePath, y.SKUAvailableItems, y.SKUID, y.SKUDescription })
                           .OrderBy(p => p.SKUName)
                          .ToList();
                    rptProductLists.DataSource = productAndSKUDetails;
                    rptProductLists.DataBind();
                }
                else
                {
                    divNoRecords.Visible = true;
                }
            }
            else if (DataHelper.DataSourceIsEmpty(productsDetails) && OpenCampaign == null && ProductType == (int)ProductsType.PreBuy)
            {
                orderControls.Visible = false;
                divNoRecords.Visible = false;
                divNoCampaign.Visible = true;
                rptProductLists.DataSource = null;
                rptProductLists.DataBind();
            }
            else
            {
                divNoRecords.Visible = true;
            }
        }
        catch (Exception ex)
        {
            EventLogProvider.LogException("Bind products to repeater", "BindData()", ex, CurrentSite.SiteID, ex.Message);
        }
    }

    /// <summary>
    /// Get the Program Ids in Open Campaign
    /// </summary>
    /// <returns></returns>
    public List<int> GetProgramIDs()
    {
        List<int> programIds = new List<int>();
        try
        {
            if (OpenCampaign != null)
            {
                programIds = ProgramProvider.GetPrograms()
                               .WhereEquals("CampaignID", OpenCampaign.CampaignID)
                               .Columns("ProgramID")
                               .Select(x => x.ProgramID).ToList<int>();
            }
        }
        catch (Exception ex)
        {
            EventLogProvider.LogException("Get ProgramsIDs from CVampaign", "GetProgramIDs()", ex, CurrentSite.SiteID, ex.Message);
        }
        return programIds;
    }

    /// <summary>
    /// Bind the Program to dropdown
    /// </summary>
    public void BindPrograms()
    {
        try
        {
            if (OpenCampaign != null)
            {
                if (OpenCampaign.CampaignID != default(int))
                {
                    var programs = ProgramProvider.GetPrograms()
                        .WhereEquals("NodeSiteID", CurrentSite.SiteID)
                        .WhereEquals("CampaignID", OpenCampaign.CampaignID)
                        .Columns("ProgramName,ProgramID")
                        .ToList();
                    if (!DataHelper.DataSourceIsEmpty(programs))
                    {
                        ddlProgram.DataSource = programs;
                        ddlProgram.DataTextField = "ProgramName";
                        ddlProgram.DataValueField = "ProgramID";
                        ddlProgram.DataBind();
                        string selectText = ValidationHelper.GetString(ResHelper.GetString("Kadena.CampaignProduct.SelectProgramText"), string.Empty);
                        ddlProgram.Items.Insert(0, new ListItem(selectText, "0"));
                    }
                }
            }
        }
        catch (Exception ex)
        {
            EventLogProvider.LogException("Bind Programs to dropdown", "BindPrograms()", ex, CurrentSite.SiteID, ex.Message);
        }
    }

    /// <summary>
    /// Bind Categories to Dropdown
    /// </summary>
    public void BindCategories()
    {
        try
        {
            var categories = ProductCategoryProvider.GetProductCategories()
                            .WhereEquals("NodeSiteID", CurrentSite.SiteID)
                            .Columns("ProductCategoryID,ProductCategoryTitle")
                            .Select(x => new ProductCategory { ProductCategoryID = x.ProductCategoryID, ProductCategoryTitle = x.ProductCategoryTitle })
                            .ToList();
            if (!DataHelper.DataSourceIsEmpty(categories))
            {
                ddlCategory.DataSource = categories;
                ddlCategory.DataTextField = "ProductCategoryTitle";
                ddlCategory.DataValueField = "ProductCategoryID";
                ddlCategory.DataBind();
                string selectText = ValidationHelper.GetString(ResHelper.GetString("Kadena.CampaignProduct.SelectCategoryText"), string.Empty);
                ddlCategory.Items.Insert(0, new ListItem(selectText, "0"));
            }
        }
        catch (Exception ex)
        {
            EventLogProvider.LogException("Bind Categories to Dropdown", "BindCategories()", ex, CurrentSite.SiteID, ex.Message);
        }
    }

    /// <summary>
    /// Filter products by By selected program
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlProgram_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindData(ValidationHelper.GetInteger(ddlProgram.SelectedValue, default(int)), ValidationHelper.GetInteger(ddlCategory.SelectedValue, default(int)), ValidationHelper.GetString(txtPos.Text, string.Empty), ValidationHelper.GetInteger(ddlBrand.SelectedValue, default(int)));
    }

    /// <summary>
    /// Filter products by selected category
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindData(ValidationHelper.GetInteger(ddlProgram.SelectedValue, default(int)), ValidationHelper.GetInteger(ddlCategory.SelectedValue, default(int)), ValidationHelper.GetString(txtPos.Text, string.Empty), ValidationHelper.GetInteger(ddlBrand.SelectedValue, default(int)));
    }

    /// <summary>
    /// Filter products by brand
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlBrand_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindData(ValidationHelper.GetInteger(ddlProgram.SelectedValue, default(int)), ValidationHelper.GetInteger(ddlCategory.SelectedValue, default(int)), ValidationHelper.GetString(txtPos.Text, string.Empty), ValidationHelper.GetInteger(ddlBrand.SelectedValue, default(int)));
    }

    /// <summary>
    /// Filter products by POS Number
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void txtPos_TextChanged(object sender, EventArgs e)
    {
        BindData(ValidationHelper.GetInteger(ddlProgram.SelectedValue, default(int)), ValidationHelper.GetInteger(ddlCategory.SelectedValue, default(int)), ValidationHelper.GetString(txtPos.Text, string.Empty));
    }

    /// <summary>
    /// Adds items to the cart
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lnkAddToCart_Command(object sender, CommandEventArgs e)
    {
        try
        {
            ProductSKUID = ValidationHelper.GetInteger(e.CommandArgument, default(int));
            hdnClickSKU.Value = ProductSKUID.ToString();
            SKUInfo product = SKUInfoProvider.GetSKUInfo(ProductSKUID);
            if (product != null && ProductType == (int)ProductsType.GeneralInventory && (string.IsNullOrWhiteSpace(product.SKUNumber) || product.SKUNumber.Equals("00000")))
            {
                Response.Cookies["status"].Value = QueryStringStatus.InvalidProduct;
                Response.Cookies["status"].HttpOnly = false;
                return;
            }
            dialog_Add_To_Cart.Attributes.Add("class", "dialog active");
            btnClose.InnerText = CartCloseText;
            lblPopUpHeader.Text = ResHelper.GetString("KDA.AddToCart.Popup.HeaderText");
            var hasBusinessUnit = CheckPersonHasBusinessUnit();
            if (!DataHelper.DataSourceIsEmpty(product))
            {
                switch (ProductType)
                {
                    case (int)ProductsType.GeneralInventory:
                        BindGeneralInventory(product, hasBusinessUnit);
                        break;
                    case (int)ProductsType.PreBuy:
                        BindPreBuy(product, hasBusinessUnit);
                        break;
                }
            }
        }
        catch (Exception ex)
        {
            EventLogProvider.LogException("Add items to cart", "lnkAddToCart_Click()", ex, CurrentSite.SiteID, ex.Message);
        }
    }
    /// <summary>
    /// Binds general inventory data to popup
    /// </summary>
    /// <param name="product"></param>
    /// <param name="hasBusinessUnit"></param>
    private void BindGeneralInventory(SKUInfo product, bool hasBusinessUnit)
    {
        try
        {
            if (product.SKUAvailableItems > 0)
            {
                lblProductName.Text = product.SKUName;
                lblAvailbleItems.Text = $"{product.SKUAvailableItems} {ResHelper.GetString("Kadena.AddToCart.StockAvilable")}";
                lblAvailbleItems.Visible = true;
                BindPopupGridData(hasBusinessUnit);
            }
            else
            {
                lblErrorMsg.Visible = true;
                llbtnAddToCart.Visible = false;
                lblErrorMsg.Text = ResHelper.GetString("Kadena.AddToCart.NoStockAvailableError");
            }
        }
        catch (Exception ex)
        {
            EventLogProvider.LogException("CustomerCartOperations.ascx.cs", "BindGeneralInventory()", ex);
        }
    }
    /// <summary>
    /// Binds prebuy data to  popup
    /// </summary>
    /// <param name="product"></param>
    /// <param name="hasBusinessUnit"></param>
    private void BindPreBuy(SKUInfo product, bool hasBusinessUnit)
    {
        try
        {
            lblProductName.Text = product?.SKUName;
            lblAvailbleItems.Visible = false;
            BindPopupGridData(hasBusinessUnit);
        }
        catch (Exception ex)
        {
            EventLogProvider.LogException("CustomerCartOperations.ascx.cs", "BindPreBuy()", ex);
        }
    }
    /// <summary>
    /// showing pop data based on assiged business units
    /// </summary>
    /// <param name="hasBusinessUnit"></param>
    private void BindPopupGridData(bool hasBusinessUnit)
    {
        try
        {
            if (hasBusinessUnit)
            {
                lblErrorMsg.Visible = false;
                gvCustomersCart.Visible = true;
                llbtnAddToCart.Visible = true;
                BindCustomersList(ProductSKUID);
                llbtnAddToCart.CommandArgument = ProductSKUID.ToString();
            }
            else
            {
                llbtnAddToCart.Visible = false;
                lblErrorMsg.Visible = true;
                lblAvailbleItems.Visible = false;
                lblErrorMsg.Text = ResHelper.GetString("Kadena.AddToCart.BusinessUnitError");
                gvCustomersCart.Visible = false;
            }
        }
        catch (Exception ex)
        {
            EventLogProvider.LogException("CustomerCartOperations.ascx.cs", "BindPopupGridData()", ex);
        }
    }
    /// <summary>
    /// Get all Cusromers / Distributers list based on product ID
    /// </summary>
    /// <param name="productID">Producct skuid</param>
    private void BindCustomersList(int productID)
    {
        try
        {
            List<AddressData> myAddressList = GetMyAddressBookList();
            if (myAddressList.Count > 0)
            {
                var shoppingCarts = DIContainer.Resolve<IShoppingCartProvider>();
                var whereCondition = new WhereCondition().WhereIn("ShoppingCartDistributorID", myAddressList.Select(g => g.AddressID).ToList())
                                                                .WhereEquals("ShoppingCartInventoryType", ProductType);
                if (ProductType == (int)ProductsType.PreBuy && OpenCampaign != null)
                {
                    whereCondition = whereCondition.WhereEquals("ShoppingCartCampaignID", OpenCampaign.CampaignID);
                }
                List<int> shoppingCartIDs = shoppingCarts.GetShoppingCartIDs(whereCondition);
                List<ShoppingCartItemInfo> cartItems = shoppingCarts.GetShoppingCartItemsByCartIDs(shoppingCartIDs);
                gvCustomersCart.DataSource = myAddressList
                    .Distinct()
                    .Select(g =>
                    {
                        var currentCartItems = cartItems
                            .Where(k => k.GetValue("CartItemDistributorID", default(int)) == g.AddressID)
                            .ToList();
                        var cartData = currentCartItems.FirstOrDefault();
                        var cartItem = currentCartItems.Where(x => x.SKUID == productID).FirstOrDefault();
                        if (cartItem != null)
                        {
                            return new
                            {
                                g.AddressID,
                                g.AddressPersonalName,
                                ShoppingCartID = cartItem?.ShoppingCartID ?? default(int),
                                SKUID = cartItem?.SKUID ?? default(int),
                                SKUUnits = cartItem?.CartItemUnits ?? default(int)
                            };
                        }
                        else
                        {
                            return new
                            {
                                g.AddressID,
                                g.AddressPersonalName,
                                ShoppingCartID = cartData?.ShoppingCartID ?? default(int),
                                SKUID = default(int),
                                SKUUnits = default(int)
                            };
                        }

                    })
                    .ToList();
                gvCustomersCart.Columns[1].HeaderText = AddressIDText;
                gvCustomersCart.Columns[2].HeaderText = AddressPersonalNameText;
                gvCustomersCart.DataBind();
            }
            else
            {
                lblError.Text = ResHelper.GetString("Kadena.AddToCart.DistributorError");
                lblError.Visible = true;
                llbtnAddToCart.Visible = false;
                btnClose.InnerText = ResHelper.GetString("KDA.ShoppingCart.Close");
            }
        }
        catch (Exception ex)
        {
            EventLogProvider.LogException("CustomerCartOperations.ascx.cs", "BindCustomersList()", ex);
        }
    }
    /// <summary>
    /// Gets the distributors created by current user
    /// </summary>
    /// <returns></returns>
    private List<AddressData> GetMyAddressBookList()
    {
        List<AddressData> myAddressList = new List<AddressData>();
        int currentCustomerId = DIContainer.Resolve<IKenticoCustomerProvider>().GetCustomerIDByUserID(CurrentUser.UserID);
        if (currentCustomerId != default(int))
        {
            myAddressList = DIContainer.Resolve<IKenticoAddressBookProvider>().GetAddressesList(currentCustomerId);
        }
        return myAddressList;
    }
    /// <summary>
    /// refreshes the page on buuton close
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnClose_ServerClick(object sender, EventArgs e)
    {
        Response.Redirect(Request.RawUrl, false);
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
            lblErrorMsg.Visible = false;
            var productProvider = DIContainer.Resolve<IKenticoProductsProvider>();
            ProductSKUID = ValidationHelper.GetInteger(hdnClickSKU.Value, default(int));
            SKUInfo product = SKUInfoProvider.GetSKUs().WhereEquals("SKUID", ProductSKUID).WhereNull("SKUOptionCategoryID").FirstObject;
            var campProduct = CampaignsProductProvider.GetCampaignsProducts().WhereEquals("NodeSKUID", product?.SKUID).Columns("CampaignsProductID,EstimatedPrice").FirstOrDefault();
            var skuPrice = default(double);
            if (ProductType == (int)ProductsType.GeneralInventory)
            {
                skuPrice = 0;
            }
            else
            {
                skuPrice = campProduct.EstimatedPrice;
            }

            var allocatedQuantityItem = campProduct != null ? productProvider.GetAllocatedProductQuantityForUser(campProduct.CampaignsProductID, CurrentUser.UserID) : null;
            var allocatedQuantity = allocatedQuantityItem!=null? allocatedQuantityItem.GetValue<int>("Quantity", default(int)):default(int);
            bool productHasAllocation = false;
            if (ProductType == (int)ProductsType.GeneralInventory)
            {
                productHasAllocation = campProduct != null ? productProvider.IsProductHasAllocation(campProduct.CampaignsProductID) : false;
            }
            var itemsPlaced = default(int);
            foreach (GridViewRow row in gvCustomersCart.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow && !DataHelper.DataSourceIsEmpty(product))
                {
                    int customerAddressID = Convert.ToInt32(row.Cells[0].Text);
                    TextBox txtQty = (row.Cells[2].FindControl("txtQuanityOrdering") as TextBox);
                    var quantityPlacing = ValidationHelper.GetInteger(txtQty.Text, default(int));
                    var customerShoppingCartID = ValidationHelper.GetInteger(row.Cells[3].Text, default(int));
                    if (ProductType == (int)ProductsType.GeneralInventory)
                    {
                        itemsPlaced += quantityPlacing;
                        if (itemsPlaced > product.SKUAvailableItems)
                        {
                            lblErrorMsg.Text = ResHelper.GetString("Kadena.AddToCart.StockError");
                            lblErrorMsg.Visible = true;
                        }
                        else if (itemsPlaced > allocatedQuantity && productHasAllocation)
                        {
                            lblErrorMsg.Text = ResHelper.GetString("Kadena.AddToCart.AllocatedProductQuantityError");
                            lblErrorMsg.Visible = true;
                        }
                        else
                        {
                            CartProcessOperations(customerShoppingCartID, quantityPlacing, product, customerAddressID, skuPrice);
                        }
                    }
                    else
                    {
                        CartProcessOperations(customerShoppingCartID, quantityPlacing, product, customerAddressID, skuPrice);
                    }
                }
            }
            btnClose.InnerText = ResHelper.GetString("KDA.ShoppingCart.Close");
            lblAvailbleItems.Visible = false;
            if (!lblErrorMsg.Visible)
            {
                lblSuccessMsg.Text = ResHelper.GetString("Kadena.AddToCart.SuccessfullyAdded");
                lblSuccessMsg.Visible = true;
                gvCustomersCart.Visible = false;
                llbtnAddToCart.Visible = false;
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
    private void CartProcessOperations(int cartID, int quantity, SKUInfo productInfo, int addressID, double skuPrice)
    {
        try
        {
            if (!DataHelper.DataSourceIsEmpty(productInfo) && addressID != default(int))
            {
                GetPrebuyData(productInfo.SKUID);
                if (cartID == default(int) && quantity > 0)
                {
                    CreateShoppingCartByCustomer(productInfo, addressID, quantity, skuPrice);
                    BindCustomersList(ProductSKUID);
                }
                else if (cartID > 0 && quantity > 0)
                {
                    Updatingtheunitcountofcartitem(productInfo, cartID, quantity, addressID, skuPrice);
                    BindCustomersList(ProductSKUID);
                }
                else if (cartID > 0 && quantity <= 0)
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
    private void Updatingtheunitcountofcartitem(SKUInfo product, int shoppinCartID, int unitCount, int customerAddressID, double skuPrice)
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
                if (cart.ShoppingCartCurrencyID <= 0)
                {
                    cart.ShoppingCartCurrencyID = CurrencyInfoProvider.GetMainCurrency(CurrentSite.SiteID).CurrencyID;
                    cart.Update();
                }
                var campaingnID = ValidationHelper.GetInteger(cart.GetValue("ShoppingCartCampaignID"), default(int));
                var programID = ValidationHelper.GetInteger(cart.GetValue("ShoppingCartProgramID"), default(int));
                item = cart.CartItems.Where(g => g.SKUID == product.SKUID).FirstOrDefault();
                if (!DataHelper.DataSourceIsEmpty(item))
                {
                    item.CartItemPrice = skuPrice;
                    ShoppingCartItemInfoProvider.UpdateShoppingCartItemUnits(item, unitCount);
                    cart.InvalidateCalculations();
                }
                else
                {
                    ShoppingCartItemParameters parameters = new ShoppingCartItemParameters(product.SKUID, unitCount);
                    parameters.CustomParameters.Add("CartItemCustomerID", customerAddressID);
                    ShoppingCartItemInfo cartItem = cart.SetShoppingCartItem(parameters);
                    cartItem.SetValue("CartItemPrice", skuPrice);
                    cartItem.SetValue("CartItemDistributorID", customerAddressID);
                    cartItem.SetValue("CartItemCampaignID", cartItem.SetValue("CartItemCampaignID", campaingnID));
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
    private void CreateShoppingCartByCustomer(SKUInfo product, int customerAddressID, int productQty, double skuPrice)
    {
        try
        {
            var customerAddress = AddressInfoProvider.GetAddressInfo(customerAddressID);
            if (!DataHelper.DataSourceIsEmpty(product))
            {
                ShoppingCartInfo cart = new ShoppingCartInfo();
                cart.ShoppingCartSiteID = CurrentSite.SiteID;
                cart.ShoppingCartCustomerID = customerAddressID;
                cart.ShoppingCartCurrencyID = CurrencyInfoProvider.GetMainCurrency(CurrentSite.SiteID).CurrencyID;
                cart.SetValue("ShoppingCartCampaignID", ProductCampaignID);
                cart.SetValue("ShoppingCartProgramID", ProductProgramID);
                cart.SetValue("ShoppingCartDistributorID", customerAddressID);
                cart.SetValue("ShoppingCartInventoryType", ProductType);
                cart.User = CurrentUser;
                cart.ShoppingCartShippingAddress = customerAddress;
                cart.ShoppingCartShippingOptionID = ProductShippingID;
                ShoppingCartInfoProvider.SetShoppingCartInfo(cart);
                ShoppingCartItemParameters parameters = new ShoppingCartItemParameters(product.SKUID, productQty);
                parameters.CustomParameters.Add("CartItemCustomerID", customerAddressID);
                ShoppingCartItemInfo cartItem = cart.SetShoppingCartItem(parameters);
                cartItem.SetValue("CartItemPrice", skuPrice);
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

    /// <summary>
    /// Gets the campaign and programid of the product
    /// </summary>
    /// <param name="skuID"></param>
    private void GetPrebuyData(int skuID)
    {
        try
        {
            var productDocument = DocumentHelper.GetDocuments("KDA.CampaignsProduct").WhereEquals("NodeSKUID", skuID).Columns("NodeSKUID,ProgramID").FirstOrDefault();
            if (!DataHelper.DataSourceIsEmpty(productDocument))
            {
                ProductProgramID = ValidationHelper.GetInteger(productDocument?.GetValue("ProgramID"), default(int));
                ProductShippingID = ShippingID;
                var program = ProgramProvider.GetPrograms()
                                       .WhereEquals("ProgramID", ProductProgramID)
                                       .FirstOrDefault();
                if (!DataHelper.DataSourceIsEmpty(program))
                {
                    ProductCampaignID = program.CampaignID;
                }
            }
        }
        catch (Exception ex)
        {
            EventLogProvider.LogException("CustomerCartOperations.ascx.cs", "GetPrebuyData()", ex);
        }
    }
    #endregion "Methods"
}

public enum ProductsType
{
    GeneralInventory = 1,
    PreBuy
}