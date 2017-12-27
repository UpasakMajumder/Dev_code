using CMS.CustomTables;
using CMS.CustomTables.Types.KDA;
using CMS.DataEngine;
using CMS.DocumentEngine.Types.KDA;
using CMS.Ecommerce;
using CMS.EventLog;
using CMS.Globalization;
using CMS.Helpers;
using CMS.MediaLibrary;
using CMS.PortalEngine.Web.UI;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web.UI.WebControls;
using Kadena.Old_App_Code.Kadena.Enums;

public partial class CMSWebParts_Kadena_Catalog_CreateCatalog : CMSAbstractWebPart
{
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
    public Campaign GetOpenCampaign
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
            SetValue("GetOpenCampaign", value);
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
    private void Bindproducts()
    {
        List<CampaignsProduct> products = null;
        if (TypeOfProduct == (int)ProductsType.PreBuy)
        {
            BindPrograms();
            BindProductTypes();
            List<int> programIds = new List<int>();
            if (GetOpenCampaign != null)
            {
                var programs = ProgramProvider.GetPrograms()
                                    .WhereEquals("CampaignID", GetOpenCampaign.CampaignID)
                                    .Columns("ProgramID")
                                    .ToList();
                if (!DataHelper.DataSourceIsEmpty(programs))
                {
                    foreach (var program in programs)
                    {
                        programIds.Add(program.ProgramID);
                    }
                }
            }
            products = CampaignsProductProvider.GetCampaignsProducts()
                .WhereIn("ProgramID", programIds).ToList();
        }
        else if (TypeOfProduct == (int)ProductsType.GeneralInventory)
        {
            BindBrands();
            BindProductTypes();
            products = CampaignsProductProvider.GetCampaignsProducts()
                .WhereEquals("ProgramID", null).ToList();
        }
        BindingProductsToRepeater(products);
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
            if (GetOpenCampaign.CampaignID != default(int))
            {
                var programs = ProgramProvider.GetPrograms()
                                    .WhereEquals("NodeSiteID", CurrentSite.SiteID)
                                    .WhereEquals("CampaignID", ValidationHelper.GetInteger(GetOpenCampaign.GetValue("CampaignID"), default(int)))
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
            lblNoProducts.Visible = false;
            if (TypeOfProduct == (int)ProductsType.PreBuy)
            {
                var products = CampaignsProductProvider.GetCampaignsProducts().WhereNotEquals("ProgramID", null).ToList();
                if (programID != default(int) && !DataHelper.DataSourceIsEmpty(products))
                {
                    products = products.Where(x => x.ProgramID == programID).ToList();
                }
                if (categoryID != default(int) && !DataHelper.DataSourceIsEmpty(products))
                {
                    products = products.Where(x => x.CategoryID == categoryID).ToList();
                }
                BindingProductsToRepeater(products, posNum);
            }
            else if (TypeOfProduct == (int)ProductsType.GeneralInventory)
            {
                var products = CampaignsProductProvider.GetCampaignsProducts().WhereEquals("ProgramID", null).ToList();
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
            CreateProductPDF();
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
    public void CreateProductPDF()
    {
        try
        {
            if (!string.IsNullOrEmpty(hdncheckedValues.Value))
            {
                lblNoProducts.Visible = false;
                List<string> selectedProducts = hdncheckedValues.Value.Split(',').ToList();
                var skuDetails = SKUInfoProvider.GetSKUs()
                                            .WhereIn("SKUNumber", selectedProducts)
                                            .ToList();
                string htmlTextheader = SettingsKeyInfoProvider.GetValue($@"{CurrentSiteName}.ProductsPDFHeader");
                string programFooterText = SettingsKeyInfoProvider.GetValue($@"{CurrentSiteName}.KDA_ProgramFooterText");
                htmlTextheader = htmlTextheader.Replace("CAMPAIGNNAME", GetOpenCampaign?.Name);
                htmlTextheader = htmlTextheader.Replace("OrderStartDate", GetOpenCampaign.StartDate == default(DateTime) ? string.Empty : GetOpenCampaign.StartDate.ToString("MMM dd, yyyy"));
                htmlTextheader = htmlTextheader.Replace("OrderEndDate", GetOpenCampaign.EndDate == default(DateTime) ? string.Empty : GetOpenCampaign.EndDate.ToString("MMM dd, yyyy"));
                string generalInventory = string.Empty;
                if (TypeOfProduct == (int)ProductsType.GeneralInventory)
                {
                    generalInventory = SettingsKeyInfoProvider.GetValue($@"{CurrentSiteName}.KDA_GeneralInventoryCover");
                }
                List<int> brands = new List<int>();
                string programsContent = string.Empty;
                if (TypeOfProduct == (int)ProductsType.PreBuy)
                {
                    var programs = ProgramProvider.GetPrograms()
                                       .Columns("ProgramName,BrandID,DeliveryDateToDistributors")
                                       .WhereEquals("CampaignID", GetOpenCampaign.CampaignID)
                                       .ToList();
                    foreach (var program in programs)
                    {
                        string programContent = SettingsKeyInfoProvider.GetValue($@"{CurrentSiteName}.ProgramsContent");
                        brands.Add(program.BrandID);
                        programContent = programContent.Replace("ProgramBrandName", program.ProgramName);
                        programContent = programContent.Replace("ProgramDate", program.DeliveryDateToDistributors == default(DateTime) ? string.Empty : program.DeliveryDateToDistributors.ToString("MMM dd, yyyy"));
                        programsContent += programContent;
                        programContent = string.Empty;
                    }
                    programsContent += programFooterText.Replace("PROGRAMFOOTERTEXT", ResHelper.GetString("Kadena.Catalog.ProgramFooterText"));
                }
                else
                {
                    var productItems = CampaignsProductProvider.GetCampaignsProducts()
                                        .WhereEquals("ProgramID", null)
                                        .ToList();
                    var inventoryList = productItems
                                        .Join(skuDetails, x => x.NodeSKUID, y => y.SKUID, (x, y) => new { x.BrandID, y.SKUNumber })
                                        .ToList();
                    foreach (var giProducts in inventoryList)
                    {
                        brands.Add(giProducts.BrandID);
                    }
                }
                string pdfProductsContentWithBrands = string.Empty;
                string closingDiv = SettingsKeyInfoProvider.GetValue("ClosingDIV").ToString();
                if (!DataHelper.DataSourceIsEmpty(selectedProducts))
                {
                    foreach (var brand in brands)
                    {
                        var brandName = GetBrandName(brand);
                        string productBrandHeader = SettingsKeyInfoProvider.GetValue($@"{CurrentSiteName}.PDFBrand");
                        productBrandHeader = productBrandHeader.Replace("BrandName", brandName);
                        List<CampaignsProduct> productItems = new List<CampaignsProduct>();
                        if (TypeOfProduct == (int)ProductsType.PreBuy)
                        {
                            productItems = CampaignsProductProvider.GetCampaignsProducts().ToList();
                        }
                        else if (TypeOfProduct == (int)ProductsType.GeneralInventory)
                        {
                            productItems = CampaignsProductProvider.GetCampaignsProducts().WhereEquals("ProgramID", null).ToList();
                        }
                        var catalogList = productItems
                                        .Join(skuDetails, x => x.NodeSKUID, y => y.SKUID, (x, y) => new { x.ProductName, x.EstimatedPrice, x.BrandID, x.ProgramID, x.QtyPerPack, x.State, y.SKUPrice, y.SKUNumber, y.SKUDescription, y.SKUShortDescription, y.SKUImagePath, y.SKUValidUntil })
                                        .Where(x => x.BrandID == brand)
                                        .ToList();
                        string pdfProductsContent = string.Empty;
                        if (!DataHelper.DataSourceIsEmpty(catalogList))
                        {
                            foreach (var product in catalogList)
                            {
                                var stateInfo = CustomTableItemProvider.GetItems<StatesGroupItem>().WhereEquals("ItemID", product.State).FirstOrDefault();
                                string pdfProductContent = SettingsKeyInfoProvider.GetValue($@"{CurrentSiteName}.PDFInnerHTML");
                                pdfProductContent = pdfProductContent.Replace("IMAGEGUID", GetProductImage(product.SKUImagePath));
                                pdfProductContent = pdfProductContent.Replace("PRODUCTPARTNUMBER", product?.SKUNumber??string.Empty);
                                pdfProductContent = pdfProductContent.Replace("PRODUCTBRANDNAME", GetBrandName(product.BrandID));
                                pdfProductContent = pdfProductContent.Replace("PRODUCTSHORTDESCRIPTION", product?.SKUShortDescription ?? string.Empty);
                                pdfProductContent = pdfProductContent.Replace("PRODUCTDESCRIPTION", product?.SKUDescription ?? string.Empty);
                                pdfProductContent = pdfProductContent.Replace("PRODUCTVALIDSTATES", stateInfo?.States ?? string.Empty);
                                pdfProductContent = pdfProductContent.Replace("PRODUCTCOSTBUNDLE", TypeOfProduct == (int)ProductsType.PreBuy ? ValidationHelper.GetString(product.EstimatedPrice, string.Empty) : ValidationHelper.GetString(product.SKUPrice, string.Empty));
                                pdfProductContent = pdfProductContent.Replace("PRODUCTBUNDLEQUANTITY", product?.QtyPerPack.ToString() ?? string.Empty);
                                pdfProductContent = pdfProductContent.Replace("PRODUCTEXPIRYDATE", product?.SKUValidUntil.ToString() ?? string.Empty);
                                pdfProductsContent += pdfProductContent;
                                pdfProductContent = string.Empty;
                                selectedProducts.Remove(product.SKUNumber);
                            }
                            pdfProductsContentWithBrands += productBrandHeader + pdfProductsContent + closingDiv;
                            productBrandHeader = string.Empty;
                        }
                    }
                }
                string pdfClosingDivs = SettingsKeyInfoProvider.GetValue($@"{CurrentSiteName}.PdfEndingTags");
                string html = pdfProductsContentWithBrands + pdfClosingDivs;
                byte[] pdfByte = default(byte[]);
                if (TypeOfProduct == (int)ProductsType.PreBuy)
                {
                    pdfByte = (new NReco.PdfGenerator.HtmlToPdfConverter()).GeneratePdf(html, htmlTextheader + programsContent + closingDiv);
                }
                else
                {
                    pdfByte = (new NReco.PdfGenerator.HtmlToPdfConverter()).GeneratePdf(html, generalInventory + closingDiv);
                }
                string fileName = string.Empty;
                if (TypeOfProduct == (int)ProductsType.PreBuy)
                {
                    fileName = ValidationHelper.GetString(ResHelper.GetString("KDA.CatalogGI.PrebuyFileName"), string.Empty) + ".pdf";
                }
                else
                {
                    fileName = ValidationHelper.GetString(ResHelper.GetString("KDA.CatalogGI.GeneralInventory"), string.Empty) + ".pdf";
                }
                Response.Clear();
                MemoryStream ms = new MemoryStream(pdfByte);
                Response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition", "attachment;filename=" + fileName);
                Response.Buffer = true;
                ms.WriteTo(Response.OutputStream);
                Response.End();
            }
            else
            {
                Bindproducts();
                lblNoProducts.Visible = true;
            }
        }
        catch (Exception ex)
        {
            EventLogProvider.LogException("creating html", ex.Message, ex);
        }
    }

    /// <summary>
    /// getting brand name based on programID
    /// </summary>
    /// <param name="BrandID"></param>
    /// <returns></returns>
    public string GetBrandName(int BrandID)
    {
        try
        {
            var brand = CustomTableItemProvider.GetItems(BrandItem.CLASS_NAME)
                        .WhereEquals("ItemID", BrandID).Columns("BrandName")
                        .Select(x => new BrandItem { BrandName = x.Field<string>("BrandName") })
                        .FirstOrDefault();
            return brand?.BrandName ?? string.Empty;
        }
        catch (Exception ex)
        {
            EventLogProvider.LogException("GetBrandName", ex.Message, ex);
            return string.Empty;
        }
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
            CreateProductPDF();
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
                                .Columns("SKUNumber,SKUName,SKUPrice,SKUEnabled,SKUImagePath,SKUAvailableItems,SKUID,SKUDescription")
                                .ToList();
                if (!DataHelper.DataSourceIsEmpty(skuDetails))
                {
                    var catalogList = productsList
                                      .Join(skuDetails, x => x.NodeSKUID, y => y.SKUID, (x, y) => new { x.ProductName, x.NodeSKUID, x.QtyPerPack, x.State, x.BrandID, y.SKUNumber, y.SKUDescription, y.SKUShortDescription, y.SKUImagePath, y.SKUValidUntil, x.EstimatedPrice })
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
    /// Get product Image by Image path
    /// </summary>
    /// <param name="imagepath"></param>
    /// <returns></returns>
    public string GetProductImage(object imagepath)
    {
        string returnValue = string.Empty;
        try
        {
            if (TypeOfProduct == (int)ProductType.PreBuy)
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
    /// Filtering data by POS number
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void posNumber_TextChanged(object sender, EventArgs e)
    {
        SetFilter(ValidationHelper.GetInteger(ddlPrograms.SelectedValue, default(int)), ValidationHelper.GetInteger(ddlProductTypes.SelectedValue, default(int)), ValidationHelper.GetInteger(ddlBrands.SelectedValue, default(int)), ValidationHelper.GetString(posNumber.Text, null));
    }

    #endregion "Methods"
}