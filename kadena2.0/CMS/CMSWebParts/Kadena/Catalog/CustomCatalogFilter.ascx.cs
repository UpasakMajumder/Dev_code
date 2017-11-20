using System;
using System.Data;
using System.Collections;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using CMS.PortalEngine.Web.UI;
using CMS.Helpers;
using CMS.DocumentEngine.Web.UI;
using CMS.DocumentEngine;
using System.Linq;
using CMS.SiteProvider;
using CMS.DocumentEngine.Types.KDA;
using CMS.CustomTables;
using CMS.CustomTables.Types.KDA;
using CMS.Membership;
using CMS.DataEngine;
using CMS.EventLog;

public partial class CMSWebParts_Kadena_Catalog_CustomCatalogFilter : CMSAbstractBaseFilterControl
{

    #region "Properties"



    #endregion


    /// <summary>
    /// Sets up the inner child controls.
    /// </summary>
    private void SetupControl()
    {
        // Hides the filter if StopProcessing is enabled
        if (this.StopProcessing)
        {
            this.Visible = false;
        }

        // Initializes only if the current request is NOT a postback
        else if (!RequestHelper.IsPostBack())
        {
            if (AuthenticationHelper.IsAuthenticated())
            {
                //ddlPrograms.Items.Insert(0, "Select Program");
                //ddlBrands.Items.Insert(0, "Select Brand Name");
                //ddlProductTypes.Items.Insert(0, "Select Category Name");
                BindPrograms();
                BindBrands();
                BindProductTypes();
            }
            else
            {
                Response.Redirect("~/Access-Denied");
            }

        }
    }




    /// <summary>
    /// Generates a WHERE condition and ORDER BY clause based on the current filtering selection.
    /// </summary>
    private void SetFilter()
    {
        string where = null;
        string order = null;

        var program = SqlHelper.EscapeLikeText(SqlHelper.EscapeQuotes(ddlPrograms.SelectedValue));
        var brand = SqlHelper.EscapeLikeText(SqlHelper.EscapeQuotes(ddlBrands.SelectedValue));
        var product = SqlHelper.EscapeLikeText(SqlHelper.EscapeQuotes(ddlProductTypes.SelectedValue));
        if (!string.IsNullOrEmpty(program) && !string.IsNullOrEmpty(brand) && !string.IsNullOrEmpty(product))
        {
            where += "ProgramID = " + program + " AND BrandID = " + brand + " and CategoryID = " + product;
        }
        if (where != null)
        {
            // Sets the Where condition
            this.WhereCondition = where;
        }

        if (order != null)
        {
            // Sets the OrderBy clause
            this.OrderBy = order;
        }

        // Raises the filter changed event
        this.RaiseOnFilterChanged();
    }


    /// <summary>
    /// Init event handler.
    /// </summary>
    protected override void OnInit(EventArgs e)
    {
        // Creates the child controls
        SetupControl();
        base.OnInit(e);
    }

    /// <summary>
    /// PreRender event handler
    /// </summary>
    protected override void OnPreRender(EventArgs e)
    {
        // Checks if the current request is a postback
        if (RequestHelper.IsPostBack())
        {
            // Applies the filter to the displayed data
            SetFilter();
        }

        base.OnPreRender(e);
    }
    /// <summary>
    /// Binding programs based on campaign
    /// </summary>
    private void BindPrograms()
    {

        try
        {
            int capaignNodeID = ValidationHelper.GetInteger(Request.QueryString["camp"], default(int));
            if (capaignNodeID != default(int))
            {
                TreeProvider tree = new TreeProvider(MembershipContext.AuthenticatedUser);
                var campaign = DocumentHelper.GetDocument(capaignNodeID, CurrentSite.DefaultVisitorCulture, tree);
                if (campaign != null)
                {
                    int campaignID = campaign.GetIntegerValue("CampaignID", default(int));
                    if (campaignID != default(int))
                    {
                        var programs = ProgramProvider.GetPrograms().WhereEquals("NodeSiteID", CurrentSite.SiteID).WhereEquals("CampaignID", campaignID).Columns("ProgramName,BrandID").Select(x => new Program { BrandID = x.BrandID, ProgramName = x.ProgramName }).ToList();
                        if (programs != null)
                        {
                            ddlPrograms.DataSource = programs;
                            ddlPrograms.DataTextField = "ProgramName";
                            ddlPrograms.DataValueField = "BrandID";
                            ddlPrograms.DataBind();
                            ddlPrograms.Items.Insert(0, "Select program");

                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            EventLogProvider.LogInformation("CMSWebParts_Kadena_Campaign_Web_Form_AddCampaignProducts", "BindPrograms", ex.Message);
        }

    }
    /// <summary>
    /// Binding brands based on program
    /// </summary>
    /// <param name="brandID"></param>
    private void BindBrands(String brandID = "1")
    {
        var brand = CustomTableItemProvider.GetItems(BrandItem.CLASS_NAME).Columns("BrandName,ItemID").WhereEquals("ItemID", brandID).TopN(1).Select(x => new BrandItem { ItemID = x.Field<int>("ItemID"), BrandName = x.Field<string>("BrandName") }).ToList();
        ddlBrands.DataSource = brand;
        ddlBrands.DataBind();
        ddlBrands.DataTextField = "BrandName";
        ddlBrands.DataValueField = "ItemID";
        ddlBrands.DataBind();

    }
    /// <summary>
    /// Binding product types
    /// </summary>
    private void BindProductTypes()
    {
        var productCategories = ProductCategoryProvider.GetProductCategories().Columns("ProductCategoryTitle,ProductCategoryID").WhereEquals("NodeSiteID", CurrentSite.SiteID).Select(x => new ProductCategory { ProductCategoryID = x.ProductCategoryID, ProductCategoryTitle = x.ProductCategoryTitle }).ToList();
        ddlProductTypes.DataSource = productCategories;
        ddlProductTypes.DataBind();
        ddlProductTypes.DataTextField = "ProductCategoryTitle";
        ddlProductTypes.DataValueField = "ProductCategoryID";
        ddlProductTypes.DataBind();

    }

    protected void ddlPrograms_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindBrands(ddlPrograms.SelectedValue);
        SetFilter();
    }

    protected void ddlBrands_SelectedIndexChanged(object sender, EventArgs e)
    {
        SetFilter();
    }

    protected void ddlProductTypes_SelectedIndexChanged(object sender, EventArgs e)
    {
        SetFilter();
    }
}



