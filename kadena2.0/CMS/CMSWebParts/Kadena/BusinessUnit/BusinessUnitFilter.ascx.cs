using CMS.DataEngine;
using CMS.DocumentEngine.Web.UI;
using CMS.Helpers;
using System;

public partial class CMSWebParts_Kadena_BusinessUnit_BusinessUnitFilter : CMSAbstractBaseFilterControl
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

        // Generates a WHERE condition based on the selected product department
        if (!string.IsNullOrEmpty(txtSearchBusinessUnit.Text))
        {
            string filterText = SqlHelper.EscapeLikeText(SqlHelper.EscapeQuotes(txtSearchBusinessUnit.Text));

            where += "BusinessUnitNumber like '%" + filterText + "%' or BusinessUnitName like'%" + filterText + "%'";

        }
        if (where != null)
        {
            // Sets the Where condition
            this.WhereCondition = where;
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

    protected void txtSearchBusinessUnit_TextChanged(object sender, EventArgs e)
    {
        SetFilter();
    }
}



