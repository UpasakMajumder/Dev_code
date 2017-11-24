using System;
using System.Data;
using System.Collections;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using CMS.PortalEngine.Web.UI;
using CMS.Helpers;
using CMS.DocumentEngine.Web.UI;
using CMS.DataEngine;

public partial class CMSWebParts_Kadena_Address_AddressFilter : CMSAbstractBaseFilterControl
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
        if (!string.IsNullOrEmpty(txtSearchAddress.Text))
        {
            string filterText = SqlHelper.EscapeLikeText(SqlHelper.EscapeQuotes(txtSearchAddress.Text));
            where += "AddressPersonalName like '%" + filterText + "%' or AddressTypeName like '%" + filterText + "%' or AddressName like '%" + filterText + "%' or CompanyName like '%" + filterText + "%' or Email like '%" + filterText + "%' or AddressPhone like '%" + filterText + "%'";
        }
        if (where != null)
        {
            this.WhereCondition = where;
        }
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
    /// filter the list by given text
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void txtSearchAddress_TextChanged(object sender, EventArgs e)
    {
        SetFilter();
    }
}



