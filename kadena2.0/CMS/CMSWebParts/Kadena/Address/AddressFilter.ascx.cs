using System;
using System.Data;
using System.Collections;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using CMS.PortalEngine.Web.UI;
using CMS.Helpers;
using CMS.DocumentEngine.Web.UI;

public partial class CMSWebParts_Kadena_Address_AddressFilter : CMSAbstractBaseFilterControl
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

        }
    }

    /// <summary>
    /// Generates a WHERE condition and ORDER BY clause based on the current filtering selection.
    /// </summary>
    private void SetFilter()
    {
        string where = null;
        string order = null;

        // Generates a WHERE condition based on the selected product department
        if (!string.IsNullOrEmpty(txtSearchBrand.Text))
        {
            // Gets the ID of the selected department
            where += "AddressPersonalName like '%" + txtSearchBrand.Text + "%' or AddressTypeName like '%" + txtSearchBrand.Text + "%' or AddressName like '%" + txtSearchBrand.Text + "%' or CompanyName like '%" + txtSearchBrand.Text + "%' or Email like '%" + txtSearchBrand.Text + "%' or AddressPhone like '%" + txtSearchBrand.Text + "%'";
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

    protected void txtSearchBrand_TextChanged(object sender, EventArgs e)
    {
        SetFilter();
    }

}



