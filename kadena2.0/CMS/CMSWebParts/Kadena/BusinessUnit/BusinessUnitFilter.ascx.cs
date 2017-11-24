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
        if (this.StopProcessing)
            this.Visible = false;
    }

    /// <summary>
    /// Generates a WHERE condition and ORDER BY clause based on the current filtering selection.
    /// </summary>
    private void SetFilter()
    {
        string where = null;
        if (!string.IsNullOrEmpty(txtSearchBusinessUnit.Text))
        {
            string filterText = SqlHelper.EscapeLikeText(SqlHelper.EscapeQuotes(txtSearchBusinessUnit.Text));
            where += $"BusinessUnitNumber like '% {filterText}%' or BusinessUnitName like'% {filterText} %'";
        }
        if (where != null)
            this.WhereCondition = where;
        this.RaiseOnFilterChanged();
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
    /// Filter the data based on the text entered in the textbox
    /// </summary>
    protected void txtSearchBusinessUnit_TextChanged(object sender, EventArgs e)
    {
        SetFilter();
    }
}



