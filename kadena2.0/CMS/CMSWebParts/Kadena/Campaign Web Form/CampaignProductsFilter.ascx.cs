using System;
using System.Data;
using System.Collections;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Linq;

using CMS.PortalEngine.Web.UI;
using CMS.Helpers;
using CMS.DocumentEngine.Types.KDA;
using CMS.DataEngine;
using CMS.DocumentEngine.Web.UI;
using CMS.EventLog;
using CMS.SiteProvider;

public partial class CMSWebParts_Kadena_Campaign_Web_Form_CampaignProductsFilter : CMSAbstractBaseFilterControl
{
    #region "Properties"



    #endregion


    /// <summary>
    /// Content loaded event handler.
    /// </summary>
    private void SetupControl()
    {
        if (this.StopProcessing)
        {
            this.Visible = false;
        }
        else if (!RequestHelper.IsPostBack())
        {
            BindPrograms();
            BindCategories();
        }
    }
    /// <summary>
    /// Generates a WHERE condition and ORDER BY clause based on the current filtering selection.
    /// </summary>
    private void SetFilter()
    {
        try
        {
            string where = null;
            if (!string.IsNullOrEmpty(txtSearchProducts.Text))
            {
                where += "ProductName like '%" + SqlHelper.EscapeLikeText(SqlHelper.EscapeQuotes(txtSearchProducts.Text)) + "%'";
            }
            if (ValidationHelper.GetInteger(ddlPrograms.SelectedValue, 0) != 0)
            {
                int programID = ValidationHelper.GetInteger(ddlPrograms.SelectedValue, 0);
                where += where != null ? "and ProgramID=" + programID : "ProgramID=" + programID;
            }
            if (ValidationHelper.GetInteger(ddlProductcategory.SelectedValue, 0) != 0)
            {
                int categoryID = ValidationHelper.GetInteger(ddlProductcategory.SelectedValue, 0);
                where += where != null ? "and CategoryID=" + categoryID : "CategoryID=" + categoryID;
            }
            if (where != null)
                this.WhereCondition = where;
            this.RaiseOnFilterChanged();

            var nodeGuid = CurrentDocument.NodeGUID;
            Campaign campaign = CampaignProvider.GetCampaign(nodeGuid, CurrentSite.DefaultVisitorCulture, CurrentSite.SiteName);
            if (campaign != null)
            {
                bool gAdminNotified = campaign.GlobalAdminNotified;
                if (CurrentUser.IsInRole("TWEGlobalAdmin", SiteContext.CurrentSiteName))
                {
                    if (gAdminNotified)
                    {
                        btnNotifyAdmin.Visible = false;
                        btnAllowUpates.Visible = true;
                    }
                }
                else if (CurrentUser.IsInRole("TWEGlobalAdmin", SiteContext.CurrentSiteName))
                {
                    if (gAdminNotified)
                    {
                        btnNotifyAdmin.Visible = false;
                        btnAllowUpates.Visible = false;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            EventLogProvider.LogInformation("CMSWebParts_Kadena_Campaign_Web_Form_CampaignProductsFilter", "SetFilter", ex.Message);
        }
    }
    /// <summary>
    /// Init event handler.
    /// </summary>
    protected override void OnInit(EventArgs e)
    {
        SetupControl();



        base.OnInit(e);
    }

    /// <summary>
    /// PreRender event handler
    /// </summary>
    protected override void OnPreRender(EventArgs e)
    {
        if (RequestHelper.IsPostBack())
            SetFilter();
        base.OnPreRender(e);
    }
    /// <summary>
    /// search data by given text
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void txtSearchProducts_TextChanged(object sender, EventArgs e)
    {
        SetFilter();
    }
    /// <summary>
    /// Bind the prograns to dropdown
    /// </summary>
    public void BindPrograms()
    {
        try
        {
            int campaignID = CurrentDocument.GetIntegerValue("CampaignID", default(int));
            if (campaignID != default(int))
            {
                var programs = ProgramProvider.GetPrograms().WhereEquals("NodeSiteID", CurrentSite.SiteID).WhereEquals("CampaignID", campaignID).Columns("ProgramID,ProgramName").ToList();
                if (programs != null)
                {
                    ddlPrograms.DataSource = programs;
                    ddlPrograms.DataTextField = "ProgramName";
                    ddlPrograms.DataValueField = "ProgramID";
                    ddlPrograms.DataBind();
                    ddlPrograms.Items.Insert(0, "--Select Program--");
                }
            }
        }
        catch (Exception ex)
        {
            EventLogProvider.LogInformation("CMSWebParts_Kadena_Campaign_Web_Form_CampaignProductsFilter", "BindPrograms", ex.Message);
        }
    }
    /// <summary>
    /// Bind Categories to dropdown
    /// </summary>
    public void BindCategories()
    {
        try
        {
            var categories = ProductCategoryProvider.GetProductCategories().WhereEquals("NodeSiteID", CurrentSite.SiteID).Columns("ProductCategoryID,ProductCategoryTitle").Select(x => new ProductCategory { ProductCategoryID = x.ProductCategoryID, ProductCategoryTitle = x.ProductCategoryTitle }).ToList();
            if (categories != null)
            {
                ddlProductcategory.DataSource = categories;
                ddlProductcategory.DataTextField = "ProductCategoryTitle";
                ddlProductcategory.DataValueField = "ProductCategoryID";
                ddlProductcategory.DataBind();
                ddlProductcategory.Items.Insert(0, "--Select Category--");
            }
        }
        catch (Exception ex)
        {
            EventLogProvider.LogInformation("CMSWebParts_Kadena_Campaign_Web_Form_CampaignProductsFilter", "BindCategories", ex.Message);
        }
    }
    /// <summary>
    /// filter data by category id
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlProductcategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        SetFilter();
    }
    /// <summary>
    /// filter data by brogram id
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlPrograms_SelectedIndexChanged(object sender, EventArgs e)
    {
        SetFilter();
    }

    protected void btnNotifyAdmin_Click(object sender, EventArgs e)
    {
        var nodeGuid = CurrentDocument.NodeGUID;
        Campaign campaign = CampaignProvider.GetCampaign(nodeGuid, CurrentSite.DefaultVisitorCulture, CurrentSite.SiteName);
        if (campaign != null)
        {
            campaign.GlobalAdminNotified = true;
            campaign.Update();
        }
    }

    protected void btnAllowUpates_Click(object sender, EventArgs e)
    {
        var nodeGuid = CurrentDocument.NodeGUID;
        Campaign campaign = CampaignProvider.GetCampaign(nodeGuid, CurrentSite.DefaultVisitorCulture, CurrentSite.SiteName);
        if (campaign != null)
        {
            campaign.GlobalAdminNotified = false;
            campaign.Update();
        }
    }

    protected void btnNewProduct_Click(object sender, EventArgs e)
    {

    }
}



