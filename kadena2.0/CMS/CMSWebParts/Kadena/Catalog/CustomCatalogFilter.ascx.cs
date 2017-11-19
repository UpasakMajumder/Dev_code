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
            where += "ProgramID = " + program + "AND BrandCode = " + brand + "and ProductType like '%" + product + "%'";
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
    //protected void txtSearchAddress_TextChanged(object sender, EventArgs e)
    //{
    //    SetFilter();
    //}

    private void BindPrograms()
    {
        var programs = ProgramProvider.GetPrograms().Select(x => new Program { ProgramID = x.ProgramID, ProgramName = x.ProgramName }).ToList(); //DocumentHelper.GetDocuments("").OnSite(SiteContext.).Published().Select(m => m.Field<string>("Title"));
        ddlPrograms.DataSource = programs;
        ddlPrograms.DataBind();
        ddlPrograms.DataTextField = "ProgramName";
        ddlPrograms.DataValueField = "ProgramID";
        ddlPrograms.DataBind();
    }
    private void BindBrands(String brandID = "1")
    {
        var brand = CustomTableItemProvider.GetItems(BrandItem.CLASS_NAME).WhereEquals("ItemID", brandID).TopN(1).Select(x => new BrandItem { BrandCode = x.Field<int>("BrandCode"), BrandName = x.Field<string>("BrandName") }).ToList();
        ddlBrands.DataSource = brand;
        ddlBrands.DataBind();
        ddlBrands.DataTextField = "BrandName";
        ddlBrands.DataValueField = "BrandCode";
        ddlBrands.DataBind();

    }

    private void BindProductTypes()
    {
        var productCategories = ProductCategoryProvider.GetProductCategories().Select(x => new ProductCategory { ProductCategoryID = x.ProductCategoryID, ProductCategoryTitle = x.ProductCategoryTitle }).ToList();
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



