using CMS.CustomTables;
using CMS.CustomTables.Types.KDA;
using CMS.DataEngine;
using CMS.DocumentEngine.Types.KDA;
using CMS.Ecommerce;
using CMS.EventLog;
using CMS.Helpers;
using CMS.PortalEngine.Web.UI;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using System.Text;
using Kadena.Models.CustomCatalog;
using Kadena.Old_App_Code.Kadena.PDFHelpers;
using Kadena.Models.SiteSettings;
using Kadena.WebAPI.KenticoProviders.Contracts;
using Kadena.Container.Default;
using Kadena.Models.Brand;
using Kadena.Models.Common;
using Kadena.BusinessLogic.Contracts;

public partial class CMSWebParts_Kadena_Catalog_CreateCatalog : CMSAbstractWebPart
{
    public IKenticoBrandsProvider BrandsProvider { get; protected set; } = DIContainer.Resolve<IKenticoBrandsProvider>();

    private Lazy<Dictionary<int, Brand>> Brands
        => new Lazy<Dictionary<int, Brand>>(() => BrandsProvider.GetBrands().ToDictionary(b => b.ItemID, b => b));

    #region "Properties"

    /// <summary>
    /// Select all text
    /// </summary>
    public string SelectAllText
    {
        get
        {
            return ValidationHelper.GetString(ResHelper.GetString("Kadena.Catalog.SelectAllText"), string.Empty);
        }
        set
        {
            SetValue("SelectAllText", value);
        }
    }

    /// <summary>
    /// Select all text
    /// </summary>
    public string NoProductSelected
    {
        get
        {
            return ValidationHelper.GetString(ResHelper.GetString("Kadena.Catalog.NoProductSelected"), string.Empty);
        }
        set
        {
            SetValue("NoProductSelected", value);
        }
    }

    /// <summary>
    /// Select all text
    /// </summary>
    public string NoDataFoundText
    {
        get
        {
            return ValidationHelper.GetString(ResHelper.GetString("Kadena.Catalog.NoDataFoundText"), string.Empty);
        }
        set
        {
            SetValue("NoDataFoundText", value);
        }
    }

    /// <summary>
    /// Select all text
    /// </summary>
    public string NoCampaignOpen
    {
        get
        {
            return ValidationHelper.GetString(ResHelper.GetString("Kadena.Catalog.NoCampaignOpen"), string.Empty);
        }
        set
        {
            SetValue("NoCampaignOpen", value);
        }
    }

    /// <summary>
    /// Gets or sets the value of product type
    /// </summary>
    public int TypeOfProduct
    {
        get
        {
            return ValidationHelper.GetInteger(GetValue("ProductType"), 0);
        }
        set
        {
            SetValue("ProductType", value);
        }
    }

    /// <summary>
    /// Gets or sets the value of Program filter
    /// </summary>
    public bool ShowProgramFilter
    {
        get
        {
            return ValidationHelper.GetBoolean(GetValue("ShowProgramFilter"), true);
        }
        set
        {
            SetValue("ShowProgramFilter", value);
        }
    }

    /// <summary>
    /// Gets or sets the value of Brand
    /// </summary>
    public bool ShowBrandFilter
    {
        get
        {
            return ValidationHelper.GetBoolean(GetValue("ShowBrandFilter"), true);
        }
        set
        {
            SetValue("ShowBrandFilter", value);
        }
    }

    /// <summary>
    /// Gets or sets the value of Product category
    /// </summary>
    public bool ShowProductCategoryFilter
    {
        get
        {
            return ValidationHelper.GetBoolean(GetValue("ShowProductCategoryFilter"), true);
        }
        set
        {
            SetValue("ShowProductCategoryFilter", value);
        }
    }

    /// <summary>
    /// Gets or sets the value of POS
    /// </summary>
    public bool ShowPosFilter
    {
        get
        {
            return ValidationHelper.GetBoolean(GetValue("ShowPosFilter"), true);
        }
        set
        {
            SetValue("ShowPosFilter", value);
        }
    }

    /// <summary>
    /// Get current open campaign
    /// </summary>
    public Campaign OpenCampaign
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

    /// <summary>
    /// Search placeholder text
    /// </summary>
    public string PosSearchPlaceholder
    {
        get
        {
            return ValidationHelper.GetString(ResHelper.GetString("Kadena.Catalog.PosSearchPlaceholderText"), string.Empty);
        }
        set
        {
            SetValue("PosSearchPlaceholder", value);
        }
    }

    /// <summary>
    /// POS number text
    /// </summary>
    public string POSNumberText
    {
        get
        {
            return ValidationHelper.GetString(ResHelper.GetString("Kadena.Catalog.POSNumberText"), string.Empty);
        }
        set
        {
            SetValue("POSNumberText", value);
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
            this.Visible = false;
        }
        else
        {
            catalogControls.Visible = true;
            lblNoProducts.Visible = false;
            ddlBrands.Visible = ShowBrandFilter;
            ddlProductTypes.Visible = ShowProductCategoryFilter;
            ddlPrograms.Visible = ShowProgramFilter;
            if (ShowPosFilter)
            {
                searchDiv.Visible = true;
                posNumber.Visible = ShowPosFilter;
            }
            else
            {
                searchDiv.Visible = false;
                posNumber.Visible = ShowPosFilter;
            }
            if (!IsPostBack)
            {
                posNumber.Attributes.Add("placeholder", PosSearchPlaceholder);
                Bindproducts();
            }
        }
    }

    /// <summary>
    /// Binding products on page load
    /// </summary>
    private void Bindproducts()
    {
        List<CampaignsProduct> products = null;
        if (TypeOfProduct == (int)ProductsType.PreBuy && OpenCampaign != null)
        {
            BindBrands();
            BindProductTypes();
            if (OpenCampaign != null)
            {
                products = CampaignsProductProvider.GetCampaignsProducts()
                           .WhereIn("ProgramID", GetProgramIDs()).WhereEquals("NodeSiteID", CurrentSite.SiteID).ToList();
            }
            BindingProductsToRepeater(products);
        }
        else if (TypeOfProduct == (int)ProductsType.PreBuy && OpenCampaign == null)
        {
            catalogControls.Visible = false;
            lblNoProducts.Visible = false;
            campaignIsNotOpen.Visible = true;
        }
        if (TypeOfProduct == (int)ProductsType.GeneralInventory)
        {
            BindBrands();
            BindProductTypes();
            products = CampaignsProductProvider.GetCampaignsProducts().WhereEquals("NodeSiteID", CurrentSite.SiteID)
                .Where(new WhereCondition().WhereEquals("ProgramID", null).Or().WhereEquals("ProgramID", 0)).ToList();
            BindingProductsToRepeater(products);
        }
    }

    /// <summary>
    /// Getting programs based on open campaign
    /// </summary>
    /// <returns></returns>
    public List<int> GetProgramIDs()
    {
        try
        {
            return ProgramProvider.GetPrograms()
                .WhereEquals(nameof(Program.CampaignID), OpenCampaign.CampaignID)
                .Columns(nameof(Program.ProgramID))
                .Select(p => p.ProgramID)
                .ToList();
        }
        catch (Exception ex)
        {
            EventLogProvider.LogException("get program based on open campaign", ex.Message, ex);
        }
        return default(List<int>);
    }

    /// <summary>
    /// Binding product types
    /// </summary>
    private void BindProductTypes()
    {
        try
        {
            var productCategories = ProductCategoryProvider.GetProductCategories()
                                        .Columns("ProductCategoryTitle,ProductCategoryID")
                                        .WhereEquals("NodeSiteID", CurrentSite.SiteID)
                                        .Select(x => new ProductCategory { ProductCategoryID = x.ProductCategoryID, ProductCategoryTitle = x.ProductCategoryTitle })
                                        .ToList()
                                        .OrderBy(y => y.ProductCategoryTitle);
            ddlProductTypes.DataSource = productCategories;
            ddlProductTypes.DataTextField = "ProductCategoryTitle";
            ddlProductTypes.DataValueField = "ProductCategoryID";
            ddlProductTypes.DataBind();
            ddlProductTypes.Items.Insert(0, new ListItem(ResHelper.GetString("Kadena.Catalog.SelectProductTypeText"), "0"));
        }
        catch (Exception ex)
        {
            EventLogProvider.LogException("Binding producttypes based on program", ex.Message, ex);
        }
    }

    /// <summary>
    /// Binding programs based on campaign
    /// </summary>
    public void BindPrograms()
    {
        try
        {
            if ((OpenCampaign?.CampaignID ?? default(int)) != default(int))
            {
                var programs = ProgramProvider.GetPrograms()
                                    .WhereEquals("NodeSiteID", CurrentSite.SiteID)
                                    .WhereEquals("CampaignID", ValidationHelper.GetInteger(OpenCampaign.GetValue("CampaignID"), default(int)))
                                    .Columns("ProgramName,ProgramID").Select(x => new Program { ProgramID = x.ProgramID, ProgramName = x.ProgramName })
                                    .ToList()
                                    .OrderBy(y => y.ProgramName);
                if (programs != null)
                {
                    ddlPrograms.DataSource = programs;
                    ddlPrograms.DataTextField = "ProgramName";
                    ddlPrograms.DataValueField = "ProgramID";
                    string selectText = ValidationHelper.GetString(ResHelper.GetString("Kadena.Catalog.SelectProgramText"), string.Empty);
                    ddlPrograms.DataBind();
                }
            }
            ddlPrograms.Items.Insert(0, new ListItem(ResHelper.GetString("Kadena.Catalog.SelectProgramText"), "0"));
        }
        catch (Exception ex)
        {
            EventLogProvider.LogException("CMSWebParts_Kadena_CustomCatalogFilter BindPrograms", ex.Message, ex);
        }
    }

    /// <summary>
    /// Binding brands based on program
    /// </summary>
    /// <param name="programID"></param>
    private void BindBrands(String programID = "0")
    {
        try
        {
            {
                var brand = CustomTableItemProvider.GetItems(BrandItem.CLASS_NAME)
                                .Columns("BrandName,ItemID")
                                .Select(x => new BrandItem { ItemID = x.Field<int>("ItemID"), BrandName = x.Field<string>("BrandName") })
                                .OrderBy(x => x.BrandName)
                                .ToList();
                ddlBrands.DataSource = brand;
                ddlBrands.DataTextField = "BrandName";
                ddlBrands.DataValueField = "ItemID";
                ddlBrands.DataBind();
                ddlBrands.Items.Insert(0, new ListItem(ResHelper.GetString("Kadena.Catalog.SelectBrandText"), "0"));
            }
        }
        catch (Exception ex)
        {
            EventLogProvider.LogException("Binding brands based on program", ex.Message, ex);
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
    /// Filtering products based on filters
    /// </summary>
    public void SetFilter(int programID = default(int), int categoryID = default(int), int brandID = default(int), string posNum = null)
    {
        try
        {
            rptCatalogProducts.DataSource = null;
            hdncheckedValues.Value = string.Empty;
            rptCatalogProducts.DataBind();
            lblNoProducts.Visible = false;
            noProductSelected.Visible = false;
            if (TypeOfProduct == (int)ProductsType.PreBuy && OpenCampaign != null)
            {
                var products = CampaignsProductProvider.GetCampaignsProducts().WhereNotEquals("ProgramID", null).WhereEquals("NodeSiteID", CurrentSite.SiteID).WhereIn("ProgramID", GetProgramIDs()).ToList();
                if (brandID != default(int) && !DataHelper.DataSourceIsEmpty(products))
                {
                    products = products.Where(x => x.BrandID == brandID).ToList();
                }
                if (categoryID != default(int) && !DataHelper.DataSourceIsEmpty(products))
                {
                    products = products.Where(x => x.CategoryID == categoryID).ToList();
                }
                BindingProductsToRepeater(products, posNum);
            }
            if (TypeOfProduct == (int)ProductsType.GeneralInventory)
            {
                var products = CampaignsProductProvider.GetCampaignsProducts().WhereEquals("NodeSiteID", CurrentSite.SiteID).Where(new WhereCondition().WhereEquals("ProgramID", null).Or().WhereEquals("ProgramID", 0)).ToList();
                if (brandID != default(int) && !DataHelper.DataSourceIsEmpty(products))
                {
                    products = products.Where(x => x.BrandID == brandID).ToList();
                }
                if (categoryID != default(int) && !DataHelper.DataSourceIsEmpty(products))
                {
                    products = products.Where(x => x.CategoryID == categoryID).ToList();
                }
                BindingProductsToRepeater(products, posNum);
            }
        }
        catch (Exception ex)
        {
            EventLogProvider.LogException("Setting where condition to filter", ex.Message, ex);
        }
    }

    /// <summary>
    /// event for Product type drop down change
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlProductTypes_SelectedIndexChanged(object sender, EventArgs e)
    {
        SetFilter(ValidationHelper.GetInteger(ddlPrograms.SelectedValue, default(int)), ValidationHelper.GetInteger(ddlProductTypes.SelectedValue, default(int)), ValidationHelper.GetInteger(ddlBrands.SelectedValue, default(int)), ValidationHelper.GetString(posNumber.Text, null));
    }

    /// <summary>
    /// event for Brands drop down change
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlBrands_SelectedIndexChanged(object sender, EventArgs e)
    {
        SetFilter(ValidationHelper.GetInteger(ddlPrograms.SelectedValue, default(int)), ValidationHelper.GetInteger(ddlProductTypes.SelectedValue, default(int)), ValidationHelper.GetInteger(ddlBrands.SelectedValue, default(int)), ValidationHelper.GetString(posNumber.Text, null));
    }

    /// <summary>
    /// event for programs drop down change
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlPrograms_SelectedIndexChanged(object sender, EventArgs e)
    {
        SetFilter(ValidationHelper.GetInteger(ddlPrograms.SelectedValue, default(int)), ValidationHelper.GetInteger(ddlProductTypes.SelectedValue, default(int)), ValidationHelper.GetInteger(ddlBrands.SelectedValue, default(int)), ValidationHelper.GetString(posNumber.Text, null));
    }

    /// <summary>
    /// Creating PDF for all the catalog.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void llbSaveSelection_Click(object sender, EventArgs e)
    {
        try
        {
            CreateProductPDF(hdncheckedValues.Value);
        }
        catch (Exception ex)
        {
            EventLogProvider.LogException("saving pdf file", ex.Message, ex);
        }
    }

    /// <summary>
    /// Creating products PDF from Html
    /// </summary>
    /// <returns></returns>
    public void CreateProductPDF(string selectedValues)
    {
        try
        {
            if (!string.IsNullOrEmpty(selectedValues))
            {
                lblNoProducts.Visible = false;
                var selectedProducts = selectedValues.Split(',').ToList();

                var productContentHtml = string.Empty;
                var closingDiv = SettingsKeyInfoProvider.GetValue(Settings.ClosingDIV, CurrentSite.SiteID).ToString();
                var programListCoverHtml = string.Empty;

                if (!DataHelper.DataSourceIsEmpty(selectedProducts))
                {
                    var selectedSkus = SKUInfoProvider
                        .GetSKUs()
                        .WhereIn(nameof(SKUInfo.SKUID), selectedProducts)
                        .ToList();

                    var programs = ProgramProvider
                        .GetPrograms()
                        .Columns(nameof(Program.ProgramID), nameof(Program.ProgramName), nameof(Program.BrandID), nameof(Program.DeliveryDateToDistributors))
                        .WhereEquals(nameof(Program.CampaignID), OpenCampaign?.CampaignID ?? default(int))
                        .ToList();

                    var products = new List<CampaignsProduct>();

                    if (TypeOfProduct == (int)ProductsType.PreBuy && OpenCampaign != null)
                    {
                        products = CampaignsProductProvider
                            .GetCampaignsProducts()
                            .WhereNotNull(nameof(CampaignsProduct.ProgramID))
                            .WhereEquals(nameof(CampaignsProduct.NodeSiteID), CurrentSite.SiteID)
                            .WhereIn(nameof(CampaignsProduct.ProgramID), GetProgramIDs())
                            .ToList();
                    }
                    else
                    {
                        products = CampaignsProductProvider
                            .GetCampaignsProducts()
                            .WhereEquals(nameof(CampaignsProduct.NodeSiteID), CurrentSite.SiteID)
                            .Where(new WhereCondition().WhereNull(nameof(CampaignsProduct.ProgramID))
                                .Or().WhereEquals(nameof(CampaignsProduct.ProgramID), 0))
                            .ToList();
                    }

                    var catalogList = products
                        .Join(selectedSkus,
                            cp => cp.NodeSKUID,
                            sku => sku.SKUID,
                            (cp, sku) => new
                            {
                                cp.ProductName,
                                cp.EstimatedPrice,
                                cp.BrandID,
                                cp.ProgramID,
                                QtyPerPack = sku.GetIntegerValue("SKUNumberOfItemsInPackage", 1),
                                cp.State,
                                sku.SKUPrice,
                                sku.SKUNumber,
                                cp.Product.SKUProductCustomerReferenceNumber,
                                sku.SKUDescription,
                                sku.SKUShortDescription,
                                cp.ProductImage,
                                sku.SKUValidUntil
                            })
                        .ToList();

                    var brandData = CustomTableItemProvider
                        .GetItems<BrandItem>()
                        .Distinct()
                        .WhereIn(nameof(BrandItem.ItemID), catalogList.Select(i => i.BrandID).ToList())
                        .Columns(nameof(BrandItem.ItemID), nameof(BrandItem.BrandName))
                        .OrderBy(nameof(BrandItem.BrandName))
                        .ToList();

                    foreach (var brand in brandData)
                    {
                        var brandCatalogList = catalogList.Where(x => x.BrandID == brand.ItemID);
                        if (TypeOfProduct == (int)ProductsType.PreBuy)
                        {
                            var programIds = brandCatalogList.Select(p => p.ProgramID).Distinct();
                            foreach (var programId in programIds)
                            {
                                var program = programs.Where(x => x.ProgramID == programId).FirstOrDefault();
                                programListCoverHtml += SettingsKeyInfoProvider.GetValue(Settings.ProgramsContent, CurrentSite.SiteID)
                                    .Replace("^ProgramName^", program?.ProgramName)
                                    .Replace("^ProgramBrandName^", brand.BrandName)
                                    .Replace("ProgramDate", program.DeliveryDateToDistributors == default(DateTime) ?
                                        string.Empty : program.DeliveryDateToDistributors.ToString("MMM dd, yyyy"));
                            }
                        }
                        var productListContentHtml = string.Empty;
                        foreach (var product in brandCatalogList)
                        {
                            var stateInfo = CustomTableItemProvider.GetItem<StatesGroupItem>(product.State);
                            productListContentHtml += SettingsKeyInfoProvider.GetValue(Settings.PDFInnerHTML, CurrentSite.SiteID)
                                .Replace("IMAGEGUID", CartPDFHelper.GetThumbnailImageAbsolutePath(product.ProductImage))
                                .Replace("PRODUCTPARTNUMBER", product.SKUProductCustomerReferenceNumber ?? string.Empty)
                                .Replace("PRODUCTBRANDNAME", GetBrandName(product.BrandID))
                                .Replace("PRODUCTSHORTDESCRIPTION", product.ProductName ?? string.Empty)
                                .Replace("PRODUCTDESCRIPTION", product.SKUDescription ?? string.Empty)
                                .Replace("PRODUCTVALIDSTATES", stateInfo?.States.Replace(",", ", ") ?? string.Empty)
                                .Replace("PRODUCTCOSTBUNDLE", TypeOfProduct == (int)ProductsType.PreBuy ?
                                    ($"{CurrencyInfoProvider.GetFormattedPrice(ValidationHelper.GetDouble(product.EstimatedPrice, default(double)), CurrentSite.SiteID, true)}")
                                    : ($"{CurrencyInfoProvider.GetFormattedPrice(ValidationHelper.GetDouble(product.SKUPrice, default(double)), CurrentSite.SiteID, true)}"))
                                .Replace("PRODUCTBUNDLEQUANTITY", product.QtyPerPack.ToString() ?? string.Empty)
                                .Replace("PRODUCTEXPIRYDATE", product.SKUValidUntil != default(DateTime) ? product.SKUValidUntil.ToString("MMM dd, yyyy") : string.Empty);
                        }

                        var productListHeaderHtml = SettingsKeyInfoProvider.GetValue(Settings.PDFBrand, CurrentSite.SiteID);
                        if (TypeOfProduct == (int)ProductsType.PreBuy)
                        {
                            productListHeaderHtml = productListHeaderHtml
                                .Replace("^PROGRAMNAME^", programs.Where(x => x.BrandID == brand.ItemID).Select(y => y.ProgramName).FirstOrDefault())
                                .Replace("^BrandName^", brand.BrandName);
                        }
                        else if (TypeOfProduct == (int)ProductsType.GeneralInventory)
                        {
                            productListHeaderHtml = productListHeaderHtml
                                .Replace("^BrandName^", brand.BrandName)
                                .Replace("^PROGRAMNAME^", string.Empty);
                        }
                        productContentHtml += $"{productListHeaderHtml}{productListContentHtml}{closingDiv}";
                    }
                }
                var pdfClosingDivs = SettingsKeyInfoProvider.GetValue(Settings.PdfEndingTags, CurrentSite.SiteID);
                var contentHtml = $"{productContentHtml}{pdfClosingDivs}";
                var coverHtml = string.Empty;
                var fileName = string.Empty;
                if (TypeOfProduct == (int)ProductsType.PreBuy)
                {
                    var campaignHeaderHtml = SettingsKeyInfoProvider.GetValue(Settings.ProductsPDFHeader, CurrentSite.SiteID);
                    if (OpenCampaign != null)
                    {
                        campaignHeaderHtml = campaignHeaderHtml
                            .Replace("CAMPAIGNNAME", OpenCampaign?.Name)
                            .Replace("OrderStartDate",
                                OpenCampaign.StartDate == default(DateTime) ? string.Empty : OpenCampaign.StartDate.ToString("MMM dd, yyyy"))
                            .Replace("OrderEndDate",
                                OpenCampaign.EndDate == default(DateTime) ? string.Empty : OpenCampaign.EndDate.ToString("MMM dd, yyyy"));
                    }
                    fileName = ValidationHelper.GetString(ResHelper.GetString("KDA.CatalogGI.PrebuyFileName"), string.Empty) + ".pdf";
                    var programListFooterHtml = SettingsKeyInfoProvider
                                .GetValue(Settings.KDA_ProgramFooterText, CurrentSite.SiteID)
                                .Replace("PROGRAMFOOTERTEXT", ResHelper.GetString("Kadena.Catalog.ProgramFooterText"));

                    coverHtml = $"{campaignHeaderHtml}{programListCoverHtml}{programListFooterHtml}{closingDiv}";
                }
                else
                {
                    var generalInventoryHeaderHtml = SettingsKeyInfoProvider.GetValue(Settings.KDA_GeneralInventoryCover, CurrentSite.SiteID);
                    coverHtml = $"{generalInventoryHeaderHtml}{closingDiv}";
                    fileName = ValidationHelper.GetString(ResHelper.GetString("KDA.CatalogGI.GeneralInventory"), string.Empty) + ".pdf";
                }

                var service = DIContainer.Resolve<IByteConverter>();
                var pdfBytes = service.GetBytes(contentHtml, coverHtml);
                RespondWithFile(fileName, pdfBytes);
            }
            else
            {
                Bindproducts();
                noProductSelected.Visible = true;
            }
        }
        catch (Exception ex)
        {
            EventLogProvider.LogException("creating html", ex.Message, ex);
        }
    }

    private void RespondWithFile(string fileName, byte[] pdfByte)
    {
        using (var ms = new MemoryStream(pdfByte))
        {
            Response.Clear();
            Response.ContentType = ContentTypes.Pdf;
            Response.AddHeader("content-disposition", "attachment;filename=" + fileName);
            Response.Buffer = true;
            ms.WriteTo(Response.OutputStream);
            Response.End();
        }
    }

    /// <summary>
    /// getting brand name 
    /// </summary>
    /// <param name="brandID"></param>
    /// <returns></returns>
    public string GetBrandName(int brandID)
    {
        if (Brands.Value.TryGetValue(brandID, out var brand))
        {
            return brand.BrandName;
        }
        return string.Empty;
    }

    /// <summary>
    /// saving full catalog to PDF
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void llbSaveFull_Click(object sender, EventArgs e)
    {
        try
        {
            if (TypeOfProduct == (int)ProductsType.PreBuy)
                GeneratePBFullPDF();
            else if (TypeOfProduct == (int)ProductsType.GeneralInventory)
                GenerateGIFullPDF();
        }
        catch (Exception ex)
        {
            EventLogProvider.LogException("printing all the products in catalog", ex.Message, ex);
        }
    }

    /// <summary>
    /// Binding products to repeater
    /// </summary>
    /// <param name="productsList"></param>
    public void BindingProductsToRepeater(List<CampaignsProduct> productsList, string posNum = null)
    {
        try
        {
            List<int> skuIds = new List<int>();
            if (!DataHelper.DataSourceIsEmpty(productsList))
            {
                foreach (var product in productsList)
                {
                    skuIds.Add(product.NodeSKUID);
                }
            }
            if (!DataHelper.DataSourceIsEmpty(skuIds))
            {
                var skuDetails = SKUInfoProvider.GetSKUs()
                                .WhereIn("SKUID", skuIds)
                                .And()
                                .WhereEquals("SKUEnabled", true)
                                .Columns("SKUProductCustomerReferenceNumber,SKUNumber,SKUName,SKUPrice,SKUEnabled,SKUAvailableItems,SKUID,SKUDescription")
                                .ToList();
                if (!DataHelper.DataSourceIsEmpty(skuDetails))
                {
                    var catalogList = productsList
                                      .Join(skuDetails,
                                            x => x.NodeSKUID,
                                            y => y.SKUID,
                                            (cp, sku) => new
                                            {
                                                cp.ProductName,
                                                cp.NodeSKUID,
                                                QtyPerPack = sku.GetIntegerValue("SKUNumberOfItemsInPackage", 1),
                                                cp.State,
                                                cp.BrandID,
                                                sku.SKUNumber,
                                                cp.Product.SKUProductCustomerReferenceNumber,
                                                sku.SKUDescription,
                                                sku.SKUShortDescription,
                                                cp.ProductImage,
                                                sku.SKUValidUntil,
                                                cp.EstimatedPrice
                                            })
                                      .OrderBy(p => p.ProductName)
                                      .ToList();
                    if (!DataHelper.DataSourceIsEmpty(catalogList) && posNum != null)
                    {
                        catalogList = catalogList.Where(x => x.SKUNumber.Contains(posNum)).ToList();
                    }
                    if (!DataHelper.DataSourceIsEmpty(catalogList))
                    {
                        noData.Visible = false;
                        rptCatalogProducts.DataSource = catalogList;
                        rptCatalogProducts.DataBind();
                    }
                    else
                    {
                        noData.Visible = true;
                    }
                }
                else
                {
                    noData.Visible = true;
                }
            }
            else
            {
                noData.Visible = true;
            }
        }
        catch (Exception ex)
        {
            EventLogProvider.LogException("Binding products to repeater", ex.Message, ex);
        }
    }

    /// <summary>
    /// getting brand name based on programID
    /// </summary>
    /// <param name="BrandID"></param>
    /// <returns></returns>
    public string GetProgramName(int brandID)
    {
        try
        {
            var programs = ProgramProvider.GetPrograms()
                                    .WhereEquals("BrandID", brandID)
                                    .Columns("ProgramName").FirstOrDefault();
            return programs?.ProgramName ?? string.Empty;
        }
        catch (Exception ex)
        {
            EventLogProvider.LogException("GetBrandName", ex.Message, ex);
            return string.Empty;
        }
    }

    /// <summary>
    /// Filtering data by POS number
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void posNumber_TextChanged(object sender, EventArgs e)
    {
        SetFilter(ValidationHelper.GetInteger(ddlPrograms.SelectedValue, default(int)), ValidationHelper.GetInteger(ddlProductTypes.SelectedValue, default(int)), ValidationHelper.GetInteger(ddlBrands.SelectedValue, default(int)), ValidationHelper.GetString(posNumber.Text, null));
    }

    /// <summary>
    /// Generates full pdf for pre buy products
    /// </summary>
    private void GeneratePBFullPDF()
    {
        try
        {
            lblNoProducts.Visible = false;
            var fileName = ValidationHelper.GetString(ResHelper.GetString("KDA.CatalogGI.PrebuyFileName"), string.Empty) + ".pdf";
            var service = DIContainer.Resolve<IPreBuyCatalogService>();
            var pdfBytes = service.Generate(OpenCampaign?.CampaignID ?? 0);
            RespondWithFile(fileName, pdfBytes);
        }
        catch (Exception ex)
        {
            EventLogProvider.LogException("creating pdf for general inventory full catalog", ex.Message, ex);
        }
    }

    /// <summary>
    /// Generates full pdf for inventory products
    /// </summary>
    private void GenerateGIFullPDF()
    {
        try
        {
            var productData = CampaignsProductProvider.GetCampaignsProducts()
                 .WhereEquals("NodeSiteID", CurrentSite.SiteID)
                 .Where(new WhereCondition().WhereNull("ProgramID").Or().WhereEquals("ProgramID", 0))
                 .ToList();
            lblNoProducts.Visible = false;
            var skuDetails = SKUInfoProvider.GetSKUs()
                                        .WhereIn("SKUID", productData.Select(s => s.SKU?.SKUID ?? -1).ToList())
                                        .ToList();
            string generalInventory = string.Empty;
            generalInventory = SettingsKeyInfoProvider.GetValue($@"{CurrentSiteName}.{Settings.KDA_GeneralInventoryCover}");
            List<int> brands = new List<int>();
            var inventoryList = productData
                                .Join(skuDetails, x => x.NodeSKUID, y => y.SKUID, (x, y) => new { x.BrandID, y.SKUNumber, x.Product.SKUProductCustomerReferenceNumber })
                                .ToList();
            foreach (var giProducts in inventoryList)
            {
                brands.Add(giProducts.BrandID);
            }
            string pdfProductsContentWithBrands = string.Empty;
            string closingDiv = SettingsKeyInfoProvider.GetValue(Settings.ClosingDIV).ToString();
            if (!DataHelper.DataSourceIsEmpty(inventoryList))
            {
                foreach (var brand in brands.Distinct())
                {
                    string productBrandHeader = SettingsKeyInfoProvider.GetValue($@"{CurrentSiteName}.{Settings.PDFBrand}");
                    productBrandHeader = productBrandHeader.Replace("^BrandName^", GetBrandName(brand));
                    productBrandHeader = productBrandHeader.Replace("^PROGRAMNAME^", string.Empty);
                    var catalogList = productData
                                    .Join(skuDetails,
                                          cp => cp.NodeSKUID,
                                          sku => sku.SKUID,
                                          (cp, sku) => new
                                          {
                                              cp.ProductName,
                                              cp.EstimatedPrice,
                                              cp.BrandID,
                                              cp.ProgramID,
                                              QtyPerPack = sku.GetIntegerValue("SKUNumberOfItemsInPackage", 1),
                                              cp.State,
                                              sku.SKUPrice,
                                              sku.SKUNumber,
                                              cp.Product.SKUProductCustomerReferenceNumber,
                                              sku.SKUDescription,
                                              sku.SKUShortDescription,
                                              cp.ProductImage,
                                              sku.SKUValidUntil
                                          })
                                    .Where(x => x.BrandID == brand)
                                    .ToList();
                    string pdfProductsContent = string.Empty;
                    if (!DataHelper.DataSourceIsEmpty(catalogList))
                    {
                        foreach (var product in catalogList)
                        {
                            var stateInfo = CustomTableItemProvider.GetItems<StatesGroupItem>().WhereEquals("ItemID", product.State).FirstOrDefault();
                            string pdfProductContent = SettingsKeyInfoProvider.GetValue($@"{CurrentSiteName}.{Settings.PDFInnerHTML}");
                            pdfProductContent = pdfProductContent.Replace("IMAGEGUID", CartPDFHelper.GetThumbnailImageAbsolutePath(product.ProductImage));
                            pdfProductContent = pdfProductContent.Replace("PRODUCTPARTNUMBER", product?.SKUProductCustomerReferenceNumber ?? string.Empty);
                            pdfProductContent = pdfProductContent.Replace("PRODUCTBRANDNAME", GetBrandName(product.BrandID));
                            pdfProductContent = pdfProductContent.Replace("PRODUCTSHORTDESCRIPTION", product?.ProductName ?? string.Empty);
                            pdfProductContent = pdfProductContent.Replace("PRODUCTDESCRIPTION", product?.SKUDescription ?? string.Empty);
                            pdfProductContent = pdfProductContent.Replace("PRODUCTVALIDSTATES", stateInfo?.States.Replace(",", ", ") ?? string.Empty);
                            pdfProductContent = pdfProductContent.Replace("PRODUCTCOSTBUNDLE", TypeOfProduct == (int)ProductsType.PreBuy ? ($"{CurrencyInfoProvider.GetFormattedPrice(ValidationHelper.GetDouble(product.EstimatedPrice, default(double)), CurrentSite.SiteID, true)}") : ($"{CurrencyInfoProvider.GetFormattedPrice(ValidationHelper.GetDouble(product.SKUPrice, default(double)), CurrentSite.SiteID, true)}"));
                            pdfProductContent = pdfProductContent.Replace("PRODUCTBUNDLEQUANTITY", product?.QtyPerPack.ToString() ?? string.Empty);
                            pdfProductContent = pdfProductContent.Replace("PRODUCTEXPIRYDATE", product?.SKUValidUntil != default(DateTime) ? product?.SKUValidUntil.ToString("MMM dd, yyyy") : string.Empty ?? string.Empty);
                            pdfProductsContent += pdfProductContent;
                            pdfProductContent = string.Empty;
                        }
                        pdfProductsContentWithBrands += productBrandHeader + pdfProductsContent + closingDiv;
                        productBrandHeader = string.Empty;
                    }
                }
            }
            string pdfClosingDivs = SettingsKeyInfoProvider.GetValue($@"{CurrentSiteName}.{Settings.PdfEndingTags}");
            string html = pdfProductsContentWithBrands + pdfClosingDivs;
            byte[] pdfByte = default(byte[]);
            var service = DIContainer.Resolve<IByteConverter>();
            pdfByte = service.GetBytes(html, generalInventory + closingDiv);
            string fileName = string.Empty;
            fileName = ValidationHelper.GetString(ResHelper.GetString("KDA.CatalogGI.GeneralInventory"), string.Empty) + ".pdf";
            Response.Clear();
            MemoryStream ms = new MemoryStream(pdfByte);
            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", "attachment;filename=" + fileName);
            Response.Buffer = true;
            ms.WriteTo(Response.OutputStream);
            Response.End();
        }
        catch (Exception ex)
        {
            EventLogProvider.LogException("creating pdf for general inventory full catalog", ex.Message, ex);
        }
    }

    #endregion "Methods"

    protected void llbExportFull_Click(object sender, EventArgs e)
    {
        List<CampaignsProduct> products = new List<CampaignsProduct>();
        List<PrebuyProduct> exportList = new List<PrebuyProduct>();
        string fileName = "Kadena.Catalog.ExcelExportPrebuy";
        if (TypeOfProduct == (int)ProductsType.PreBuy)
        {
            products = CampaignsProductProvider.GetCampaignsProducts().WhereNotEquals("ProgramID", null).WhereEquals("NodeSiteID", CurrentSite.SiteID).WhereIn("ProgramID", GetProgramIDs()).ToList();
            if (!DataHelper.DataSourceIsEmpty(products))
            {
                products.ForEach(p =>
            {
                exportList.Add(new PrebuyProduct()
                {
                    ProductId = p.CampaignsProductID,
                    ProductName = p.ProductName,
                    ShortDescription = p.DocumentSKUShortDescription,
                    BundleQuantity = p.SKU?.GetIntegerValue("SKUNumberOfItemsInPackage", 1) ?? 1,
                    ProductCost = CurrencyInfoProvider.GetFormattedPrice(ValidationHelper.GetDouble(p.EstimatedPrice, default(double)), CurrentSite.SiteID, true),
                    ProgramName = GetProgramFormId(p.ProgramID),
                    BrandName = GetBrandName(p.BrandID),
                    PosNumber = GetPosNumber(p.SKU?.SKUID ?? 0),
                    States = GetStateInfo(p.State)
                });
            });
            }
        }
        else if (TypeOfProduct == (int)ProductsType.GeneralInventory)
        {
            fileName = "Kadena.Catalog.ExcelExportInventory";
            products = CampaignsProductProvider.GetCampaignsProducts().WhereEquals("NodeSiteID", CurrentSite.SiteID)
                            .Where(new WhereCondition().WhereEquals("ProgramID", null).Or().WhereEquals("ProgramID", 0)).ToList();
            if (!DataHelper.DataSourceIsEmpty(products))
            {
                products.ForEach(p =>
            {
                exportList.Add(new PrebuyProduct()
                {
                    ProductId = p.CampaignsProductID,
                    ProductName = p.ProductName,
                    ShortDescription = p.DocumentSKUShortDescription,
                    BundleQuantity = p.SKU?.GetIntegerValue("SKUNumberOfItemsInPackage", 1) ?? 1,
                    ProductCost = CurrencyInfoProvider.GetFormattedPrice(ValidationHelper.GetDouble(p.SKU?.SKUPrice, default(double)), CurrentSite.SiteID, true),
                    BrandName = GetBrandName(p.BrandID),
                    PosNumber = GetPosNumber(p.SKU?.SKUID ?? 0),
                    States = GetStateInfo(p.State)
                });
            });
            }
        }
        DownloadExcel(exportList, fileName);
    }


    public void DownloadExcel<T>(List<T> exportList, string fileName)
    {
        DataGrid dg = new DataGrid();
        dg.AllowPaging = false;
        dg.DataSource = exportList;
        dg.DataBind();
        HttpContext.Current.Response.Clear();
        HttpContext.Current.Response.Buffer = true;
        HttpContext.Current.Response.ContentEncoding = Encoding.UTF8;
        HttpContext.Current.Response.Charset = "";
        HttpContext.Current.Response.AddHeader("Content-Disposition",
          "attachment; filename=" + ResHelper.GetString(fileName) + ".xls");
        HttpContext.Current.Response.ContentType =
          "application/vnd.ms-excel";
        StringWriter stringWriter = new StringWriter();
        System.Web.UI.HtmlTextWriter htmlTextWriter =
          new System.Web.UI.HtmlTextWriter(stringWriter);
        dg.RenderControl(htmlTextWriter);
        HttpContext.Current.Response.Write(stringWriter.ToString());
        HttpContext.Current.Response.End();
    }

    public string GetProgramFormId(int programId)
    {
        if (programId > 0)
        {
            var programData = ProgramProvider.GetPrograms().WhereEquals("ProgramID", programId).FirstOrDefault();
            if (!DataHelper.DataSourceIsEmpty(programData)) return programData.ProgramName;
            return string.Empty;
        }
        return string.Empty;
    }

    public string GetPosNumber(int skuId)
    {
        if (skuId > 0)
        {
            var skuData = SKUInfoProvider.GetSKUs().WhereEquals("SKUID", skuId).FirstOrDefault();
            if (!DataHelper.DataSourceIsEmpty(skuData)) return skuData.GetValue("SKUProductCustomerReferenceNumber", string.Empty);
        }
        return string.Empty;
    }

    public string GetStateInfo(int stateId)
    {
        var stateData = CustomTableItemProvider.GetItems<StatesGroupItem>().WhereEquals("ItemID", stateId).FirstOrDefault();
        return stateData?.States.Replace(",", ", ") ?? string.Empty;
    }

    protected void llbExportSelection_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(hdncheckedValues.Value))
        {
            Bindproducts();
            noProductSelected.Visible = true;
            return;
        }
        string fileName = "Kadena.Catalog.ExcelExportPrebuy";
        List<string> selectedProducts = hdncheckedValues.Value.Split(',').ToList();
        List<PrebuyProduct> exportList = new List<PrebuyProduct>();
        var skuDetails = SKUInfoProvider.GetSKUs().WhereIn("SKUID", selectedProducts).ToList();
        if (TypeOfProduct == (int)ProductsType.GeneralInventory)
        {
            fileName = "Kadena.Catalog.ExcelExportInventory";
            skuDetails.ForEach(p =>
        {
            var productData = CampaignsProductProvider.GetCampaignsProducts().WhereEquals("NodeSKUID", p.SKUID).FirstOrDefault();
            exportList.Add(new PrebuyProduct()
            {
                ProductId = productData.CampaignsProductID,
                ProductName = productData.ProductName,
                ShortDescription = p.SKUShortDescription,
                BundleQuantity = productData.SKU.GetIntegerValue("SKUNumberOfItemsInPackage", 1),
                ProductCost = CurrencyInfoProvider.GetFormattedPrice(ValidationHelper.GetDouble(p.SKUPrice, default(double)), CurrentSite.SiteID, true),
                BrandName = GetBrandName(productData.BrandID),
                PosNumber = GetPosNumber(productData.SKU.SKUID),
                States = GetStateInfo(productData.State)
            });
        });
        }
        if (TypeOfProduct == (int)ProductsType.PreBuy)
        {
            skuDetails.ForEach(p =>
            {
                var productData = CampaignsProductProvider.GetCampaignsProducts().WhereEquals("NodeSKUID", p.SKUID).FirstOrDefault();
                exportList.Add(new PrebuyProduct()
                {
                    ProductId = productData.CampaignsProductID,
                    ProductName = productData.ProductName,
                    ShortDescription = p.SKUShortDescription,
                    BundleQuantity = productData.SKU.GetIntegerValue("SKUNumberOfItemsInPackage", 1),
                    ProductCost = CurrencyInfoProvider.GetFormattedPrice(ValidationHelper.GetDouble(productData.EstimatedPrice, default(double)), CurrentSite.SiteID, true),
                    BrandName = GetBrandName(productData.BrandID),
                    PosNumber = GetPosNumber(productData.SKU.SKUID),
                    States = GetStateInfo(productData.State),
                    ProgramName = GetProgramFormId(productData.ProgramID)
                });
            });
        }
        DownloadExcel(exportList, fileName);
    }
}