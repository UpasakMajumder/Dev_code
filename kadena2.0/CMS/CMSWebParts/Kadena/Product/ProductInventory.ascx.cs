using CMS.CustomTables;
using CMS.CustomTables.Types.KDA;
using CMS.DataEngine;
using CMS.DocumentEngine.Types.KDA;
using CMS.Ecommerce;
using CMS.EventLog;
using CMS.Helpers;
using CMS.MediaLibrary;
using CMS.PortalEngine.Web.UI;
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
                txtPos.Attributes.Add("placeholder", PosSearchPlaceholder);
                BindPrograms();
                BindCategories();
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
            string folderName = SettingsKeyInfoProvider.GetValue(CurrentSite.SiteName + ".KDA_ImagesFolderName");
            folderName = !string.IsNullOrEmpty(folderName) ? folderName.Replace(" ", "") : "CampaignProducts";
            if (imagepath != null && folderName != null)
            {
                returnValue = MediaFileURLProvider.GetMediaFileUrl(CurrentSiteName, folderName, ValidationHelper.GetString(imagepath, string.Empty));
            }
        }
        catch (Exception ex)
        {
            EventLogProvider.LogException("Get Product Image", "GetProductImage", ex, CurrentSite.SiteID, ex.Message);
        }
        return returnValue;
    }

    /// <summary>
    /// Bind the Products data to repeater
    /// </summary>
    /// <param name="programID"></param>
    /// <param name="categoryID"></param>
    public void BindData(int programID = default(int), int categoryID = default(int), string posNumber = null)
    {
        try
        {
            List<CampaignsProduct> productsDetails = new List<CampaignsProduct>();
            if (ProductType != default(int))
            {
                if (ProductType == (int)ProductsType.GeneralInventory)
                {
                    ddlCategory.Visible = true;
                    productsDetails = CampaignsProductProvider.GetCampaignsProducts()
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
                    }
                }
                else
                {
                    ddlProgram.Visible = true;
                    ddlCategory.Visible = true;
                    List<int> programIds = GetProgramIDs();
                    if (!DataHelper.DataSourceIsEmpty(programIds))
                    {
                        productsDetails = CampaignsProductProvider.GetCampaignsProducts()
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
                List<int> skuIds = new List<int>();
                if (!DataHelper.DataSourceIsEmpty(productsDetails))
                {
                    foreach (var product in productsDetails)
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
                    if (!string.IsNullOrEmpty(posNumber) && !string.IsNullOrWhiteSpace(posNumber) && !DataHelper.DataSourceIsEmpty(skuDetails))
                    {
                        skuDetails = skuDetails
                            .Where(x => x.SKUNumber.Contains(posNumber))
                            .ToList();
                    }
                    if (!DataHelper.DataSourceIsEmpty(skuDetails) && !DataHelper.DataSourceIsEmpty(productsDetails))
                    {
                        var productAndSKUDetails = productsDetails
                              .Join(skuDetails, x => x.NodeSKUID, y => y.SKUID, (x, y) => new { x.ProgramID, x.CategoryID,x.QtyPerPack, y.SKUNumber, y.SKUName, y.SKUPrice, y.SKUEnabled, y.SKUImagePath, y.SKUAvailableItems, y.SKUID, y.SKUDescription }).ToList();
                        rptProductList.DataSource = productAndSKUDetails;
                        rptProductList.DataBind();
                    }
                }
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
            Campaign campaign = CampaignProvider.GetCampaigns()
                              .Columns("CampaignID")
                              .Where(x => x.OpenCampaign == true && x.CloseCampaign == false)
                              .FirstOrDefault();
            if (campaign != null)
            {
                var programs = ProgramProvider.GetPrograms()
                       .WhereEquals("CampaignID", campaign.CampaignID)
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
            Campaign campaign = CampaignProvider.GetCampaigns()
                             .Columns("CampaignID")
                             .WhereEquals("NodeSiteID", CurrentSite.SiteID)
                             .Where(x => x.OpenCampaign == true && x.CloseCampaign == false)
                             .FirstOrDefault();
            if (campaign != null)
            {
                if (campaign.CampaignID != default(int))
                {
                    var programs = ProgramProvider.GetPrograms()
                        .WhereEquals("NodeSiteID", CurrentSite.SiteID)
                        .WhereEquals("CampaignID", campaign.CampaignID)
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
        BindData(ValidationHelper.GetInteger(ddlProgram.SelectedValue, default(int)), ValidationHelper.GetInteger(ddlCategory.SelectedValue, default(int)), ValidationHelper.GetString(txtPos.Text, string.Empty));
    }

    /// <summary>
    /// Filter products by selected category
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindData(ValidationHelper.GetInteger(ddlProgram.SelectedValue, default(int)), ValidationHelper.GetInteger(ddlCategory.SelectedValue, default(int)), ValidationHelper.GetString(txtPos.Text, string.Empty));
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

    #endregion "Methods"

}

public enum ProductsType
{
    GeneralInventory = 1,
    PreBuy
}