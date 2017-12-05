using CMS.CustomTables;
using CMS.CustomTables.Types.KDA;
using CMS.DataEngine;
using CMS.DocumentEngine.Types.KDA;
using CMS.EventLog;
using CMS.Helpers;
using CMS.PortalEngine.Web.UI;
using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Web.UI.WebControls;

public partial class CMSWebParts_Kadena_Catalog_CreateCatalog : CMSAbstractWebPart
{
    #region "Properties"

    /// <summary>
    /// Gets or sets the value of product type
    /// </summary>
    public int ProductType
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
    /// Gets or sets the value of
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
    /// Gets or sets the value of
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
    /// Gets or sets the value of
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
    /// Gets or sets the value of
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
            ddlBrands.Visible = ShowBrandFilter;
            ddlProductTypes.Visible = ShowProductCategoryFilter;
            ddlPrograms.Visible = ShowProgramFilter;
            if (!IsPostBack)
            {
                if (ProductType == (int)ProductOfType.CampaignProduct)
                {
                    var products = CampaignProductProvider.GetCampaignProducts().Columns("Image,ShortDescription,ProductName,CampaignProductID,EstimatedPrice").ToList();
                    rptCatalogProducts.DataSource = products;
                    rptCatalogProducts.DataBind();
                    BindPrograms();
                    BindBrands(ddlPrograms.SelectedValue);
                    BindProductTypes();
                }
                else if (ProductType == (int)ProductOfType.InventoryProduct)
                {
                    var products = CampaignProductProvider.GetCampaignProducts().Columns("Image,ShortDescription,ProductName,CampaignProductID,EstimatedPrice").WhereEquals("ProgramID", null).ToList();
                    rptCatalogProducts.DataSource = products;
                    rptCatalogProducts.DataBind();
                    BindBrands();
                }
            }
        }
    }

    /// <summary>
    /// Binding product types
    /// </summary>
    private void BindProductTypes()
    {
        try
        {
            var productCategories = ProductCategoryProvider.GetProductCategories().Columns("ProductCategoryTitle,ProductCategoryID").WhereEquals("NodeSiteID", CurrentSite.SiteID).Select(x => new ProductCategory { ProductCategoryID = x.ProductCategoryID, ProductCategoryTitle = x.ProductCategoryTitle }).ToList().OrderBy(y => y.ProductCategoryTitle);
            ddlProductTypes.DataSource = productCategories;
            ddlProductTypes.DataTextField = "ProductCategoryTitle";
            ddlProductTypes.DataValueField = "ProductCategoryID";
            ddlProductTypes.DataBind();
        }
        catch (Exception ex)
        {
            EventLogProvider.LogException("Binding producttypes based on program", ex.Message, ex);
        }
    }

    /// <summary>
    /// Binding programs based on campaign
    /// </summary>
    private void BindPrograms()
    {
        try
        {
            var campaign = CampaignProvider.GetCampaigns().Columns("CampaignID").WhereEquals("OpenCampaign", true).WhereNotEquals("CloseCampaign", false).WhereEquals("NodeSiteID", CurrentSite.SiteID).FirstOrDefault();
            if (ValidationHelper.GetInteger(campaign.GetValue("CampaignID"), default(int)) != default(int))
            {
                var programs = ProgramProvider.GetPrograms().WhereEquals("NodeSiteID", CurrentSite.SiteID).WhereEquals("CampaignID", ValidationHelper.GetInteger(campaign.GetValue("CampaignID"), default(int))).Columns("ProgramName,ProgramID").Select(x => new Program { ProgramID = x.ProgramID, ProgramName = x.ProgramName }).ToList().OrderBy(y => y.ProgramName);
                if (programs != null)
                {
                    ddlPrograms.DataSource = programs;
                    ddlPrograms.DataTextField = "ProgramName";
                    ddlPrograms.DataValueField = "ProgramID";
                    ddlPrograms.DataBind();
                }
            }
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
            if (!string.IsNullOrEmpty(programID))
            {
                var brandID = ProgramProvider.GetPrograms()
                    .WhereEquals("NodeSiteID", CurrentSite.SiteID)
                    .WhereEquals("ProgramID", ValidationHelper.GetInteger(programID, default(int)))
                    .Columns("BrandID")
                    .Select(x => new Program { BrandID = x.BrandID })
                    .FirstOrDefault();
                var brand = CustomTableItemProvider.GetItems(BrandItem.CLASS_NAME).Columns("BrandName,ItemID").WhereEquals("ItemID", ValidationHelper.GetInteger(brandID.GetValue("BrandID"), default(int))).Select(x => new BrandItem { ItemID = x.Field<int>("ItemID"), BrandName = x.Field<string>("BrandName") }).ToList();
                ddlBrands.DataSource = brand;
                ddlBrands.DataTextField = "BrandName";
                ddlBrands.DataValueField = "ItemID";
                ddlBrands.DataBind();
            }
            else
            {
                var brand = CustomTableItemProvider.GetItems(BrandItem.CLASS_NAME).Columns("BrandName,ItemID").Select(x => new BrandItem { ItemID = x.Field<int>("ItemID"), BrandName = x.Field<string>("BrandName") }).ToList();
                ddlBrands.DataSource = brand;
                ddlBrands.DataTextField = "BrandName";
                ddlBrands.DataValueField = "ItemID";
                ddlBrands.DataBind();
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
    public void SetFilter()
    {
        try
        {
            if (ProductType == (int)ProductOfType.CampaignProduct)
            {
                var program = SqlHelper.EscapeLikeText(SqlHelper.EscapeQuotes(ddlPrograms.SelectedValue)).Trim();
                var brand = SqlHelper.EscapeLikeText(SqlHelper.EscapeQuotes(ddlBrands.SelectedValue)).Trim();
                var product = SqlHelper.EscapeLikeText(SqlHelper.EscapeQuotes(ddlProductTypes.SelectedValue)).Trim();
                if (!string.IsNullOrEmpty(program) && !string.IsNullOrEmpty(brand) && !string.IsNullOrEmpty(product))
                {
                    string WhereCondition = $"ProgramID={program} AND BrandID ={brand} and CategoryID ={product}";
                    var products = CampaignProductProvider.GetCampaignProducts().Where(WhereCondition).Columns("Image,ShortDescription,ProductName,EstimatedPrice").ToList();
                    rptCatalogProducts.DataSource = products;
                    rptCatalogProducts.DataBind();
                }
            }
            else
            {
                var brand = SqlHelper.EscapeLikeText(SqlHelper.EscapeQuotes(ddlBrands.SelectedValue)).Trim();
                var product = SqlHelper.EscapeLikeText(SqlHelper.EscapeQuotes(ddlProductTypes.SelectedValue)).Trim();
                if (!string.IsNullOrEmpty(brand) && !string.IsNullOrEmpty(product))
                {
                    string WhereCondition = $"BrandID ={brand} and CategoryID ={product}";
                    var products = CampaignProductProvider.GetCampaignProducts().Where(WhereCondition).Columns("Image,ShortDescription,ProductName,EstimatedPrice").ToList();
                    rptCatalogProducts.DataSource = products;
                    rptCatalogProducts.DataBind();
                }
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
        SetFilter();
    }

    /// <summary>
    /// event for Brands drop down change
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlBrands_SelectedIndexChanged(object sender, EventArgs e)
    {
        SetFilter();
    }

    /// <summary>
    /// event for programs drop down change
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlPrograms_SelectedIndexChanged(object sender, EventArgs e)
    {
        SetFilter();
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
            var items = !string.IsNullOrEmpty(hdncheckedValues.Value) ? hdncheckedValues.Value.Split(',').ToList() : null;
            var productItems = CampaignProductProvider.GetCampaignProducts().WhereIn("CampaignProductID", items).ToList();
            string htmlTextheader = SettingsKeyInfoProvider.GetValue($@"{CurrentSiteName}.ProductPdfHeader");
            htmlTextheader = htmlTextheader.Replace("CAMPAIGNNAME", "Test CamPaign");
            htmlTextheader = htmlTextheader.Replace("OrderStartDate", "");
            htmlTextheader = htmlTextheader.Replace("OrderEndDate", "");
            var programs = ProgramProvider.GetPrograms().Columns("ProgramName,BrandID").ToList();
            string programContent = SettingsKeyInfoProvider.GetValue($@"{CurrentSiteName}.ProgramsContent");
            string programsContent = string.Empty;
            foreach (var program in programs)
            {
                programContent = programContent.Replace("ProgramBrandName", GetBrandName(program.BrandID));
                programContent = programContent.Replace("ProgramDate", "getprogramdate");
                programsContent += programContent;
            }
            string closingDiv = SettingsKeyInfoProvider.GetValue("ClosingDIV").ToString();
            string pdfProductContentHeader = SettingsKeyInfoProvider.GetValue($@"{CurrentSiteName}.PDFProductContentHeader");
            pdfProductContentHeader = pdfProductContentHeader.Replace("BRANDNAME", "");
            string pdfProductsContent = string.Empty;
            foreach (var product in productItems)
            {
                string pdfProductContent = SettingsKeyInfoProvider.GetValue($@"{CurrentSiteName}.ProductInnerContent");
                pdfProductContent = pdfProductContent.Replace("IMAGEGUID", product?.Image.ToString() ?? string.Empty);
                pdfProductContent = pdfProductContent.Replace("PRODUCTPARTNUMBER", "Test");
                pdfProductContent = pdfProductContent.Replace("PRODUCTBRANDNAME", "getbrandname based on programbrandId");
                pdfProductContent = pdfProductContent.Replace("PRODUCTSHORTDESCRIPTION", product.ShortDescription);
                pdfProductContent = pdfProductContent.Replace("PRODUCTDESCRIPTION", product.LongDescription);
                pdfProductContent = pdfProductContent.Replace("PRODUCTVALIDSTATES", "get valid states");
                pdfProductContent = pdfProductContent.Replace("PRODUCTCOSTBUNDLE", "cost bundle");
                pdfProductContent = pdfProductContent.Replace("PRODUCTBUNDLEQUANTITY", product.QtyPerPack);
                pdfProductContent = pdfProductContent.Replace("PRODUCTEXPIRYDATE", product?.ExpirationDate.ToString() ?? string.Empty);
                pdfProductsContent += pdfProductContent;
                pdfProductContent = string.Empty;
            }
            string pdfClosingDivs = SettingsKeyInfoProvider.GetValue($@"{CurrentSiteName}.PdfEndingTags");
            string html = pdfProductContentHeader + pdfProductsContent + pdfClosingDivs;
            var pdfBytes = (new NReco.PdfGenerator.HtmlToPdfConverter()).GeneratePdf(html, htmlTextheader + programsContent + closingDiv);
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
            var brand = CustomTableItemProvider.GetItems(BrandItem.CLASS_NAME).WhereEquals("ItemID", BrandID).Columns("BrandName").Select(x => new BrandItem { BrandName = x.Field<string>("BrandName") }).FirstOrDefault();
            return brand?.BrandName ?? string.Empty;
        }
        catch (Exception ex)
        {
            EventLogProvider.LogException("GetBrandName", ex.Message, ex);
            return string.Empty;
        }
    }

    /// <summary>
    /// Enum for products types
    /// </summary>
    private enum ProductOfType
    { CampaignProduct = 1, InventoryProduct = 2 };

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

    #endregion "Methods"
}