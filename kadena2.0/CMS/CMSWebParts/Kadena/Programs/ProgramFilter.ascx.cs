using System;
using System.Data;
using System.Collections;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using CMS.PortalEngine.Web.UI;
using CMS.Helpers;
using CMS.DocumentEngine.Web.UI;

public partial class CMSWebParts_Kadena_Programs_ProgramFilter : CMSAbstractBaseFilterControl
{
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
        if (!string.IsNullOrEmpty(txtSearchProgram.Text))
        {
            // Gets the ID of the selected department
            where += "ProgramName like '%" + txtSearchProgram.Text + "%' or ProgramDescription like '%" + txtSearchProgram.Text + "%' or BrandName like '%"+txtSearchProgram.Text+ "%' or Name like '%" + txtSearchProgram.Text + "%'";
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
    protected void txtSearchProgram_TextChanged(object sender, EventArgs e)
    {
        SetFilter();
    }
}



