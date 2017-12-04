using System;
using System.Data;
using System.Collections;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using CMS.PortalEngine.Web.UI;
using CMS.Helpers;
using CMS.DocumentEngine.Types.KDA;
using System.Linq;
using CMS.Ecommerce;
using System.Collections.Generic;
using CMS.DataEngine;
using CMS.EventLog;

public partial class CMSWebParts_Kadena_Product_ProductInventory : CMSAbstractWebPart
{
    #region "Properties"
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


    #endregion


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
    public void BindData(int programID = default(int), int categoryID = default(int))
    {
        List<CampaignsProduct> productsDetails = new List<CampaignsProduct>();
        if (ProductType != default(int))
        {
            if (ProductType == (int)ProductsType.InventoryProduct)
            {
                ddlCategory.Visible = true;
                productsDetails = CampaignsProductProvider.GetCampaignsProducts()
                                  .WhereEquals("ProgramID", null)
                                  .ToList();
                if (categoryID != default(int))
                {
                    productsDetails = productsDetails.Where(x => x.CategoryID == categoryID).ToList();
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
                    if (programID != default(int))
                    {
                        productsDetails = productsDetails.Where(x => x.ProgramID == programID).ToList();
                    }
                    if (categoryID != default(int))
                    {
                        productsDetails = productsDetails.Where(x => x.CategoryID == categoryID).ToList();
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
                if (!DataHelper.DataSourceIsEmpty(skuDetails))
                {
                    var productAndSKUDetails = productsDetails
                          .Join(skuDetails, x => x.NodeSKUID, y => y.SKUID, (x, y) => new { x.ProgramID, x.CategoryID, y.SKUNumber, y.SKUName, y.SKUPrice, y.SKUEnabled, y.SKUImagePath, y.SKUAvailableItems, y.SKUID, y.SKUDescription }).ToList();
                    rptProductList.DataSource = productAndSKUDetails;
                    rptProductList.DataBind();
                }
            }
        }
    }
    public List<int> GetProgramIDs()
    {
        Campaign campaign = CampaignProvider.GetCampaigns()
                          .Columns("CampaignID")
                          .Where(x => x.OpenCampaign == true && x.CloseCampaign == false)
                          .FirstOrDefault();
        List<int> programIds = new List<int>();
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
        return programIds;
    }
    public void BindPrograms()
    {
        Campaign campaign = CampaignProvider.GetCampaigns()
                         .Columns("CampaignID")
                         .WhereEquals("NodeSiteID",CurrentSite.SiteID)
                         .Where(x => x.OpenCampaign == true && x.CloseCampaign == false)
                         .FirstOrDefault();
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
            EventLogProvider.LogException("Bind Category", "BindCategories", ex, CurrentSite.SiteID, ex.Message);
        }
    }


    #endregion

    protected void ddlProgram_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindData(ValidationHelper.GetInteger(ddlProgram.SelectedValue, default(int)), ValidationHelper.GetInteger(ddlCategory.SelectedValue, default(int)));
    }

    protected void ddlCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindData(ValidationHelper.GetInteger(ddlProgram.SelectedValue, default(int)), ValidationHelper.GetInteger(ddlCategory.SelectedValue, default(int)));
    }
}
public enum ProductsType
{
    InventoryProduct = 1, PreBuyProduct = 2
}
