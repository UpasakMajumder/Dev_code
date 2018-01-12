using CMS.CustomTables;
using CMS.CustomTables.Types.KDA;
using CMS.EventLog;
using CMS.Helpers;
using CMS.PortalEngine.Web.UI;
using Kadena.Old_App_Code.Kadena.Constants;
using System;
using System.Linq;
using System.Web.UI.WebControls;

public partial class CMSWebParts_Kadena_BusinessUnit_BusinessUnit : CMSAbstractWebPart
{
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
        if (!this.StopProcessing)
        {
            BindResourceStrings();
            BindBusinessUnitStatus();
            var itemID = Request.QueryString["itemID"] != null ? ValidationHelper.GetInteger(Request.QueryString["itemID"], default(int)) : default(int);
            if (itemID != default(int))
            {
                BindBusinessUnitData(itemID);
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

    #endregion "Methods"

    #region "Custom methods"

    /// <summary>
    /// Binds all the labels and error messages
    /// </summary>
    private void BindResourceStrings()
    {
        try
        {
            lblBUNumber.InnerText = ResHelper.GetString("Kadena.BusinessUnit.NumberText");
            lblBUName.InnerText = ResHelper.GetString("Kadena.BusinessUnit.NameText");
            lblBUStatus.InnerText = ResHelper.GetString("Kadena.BusinessUnit.StatusText");
            rfBUNumber.ErrorMessage = ResHelper.GetString("Kadena.BusinessUnit.BUNumberRequired");
            rfBUName.ErrorMessage = ResHelper.GetString("Kadena.BusinessUnit.BUNameRequired");
            revBUNumber.ErrorMessage = ResHelper.GetString("Kadena.BusinessUnit.BUNumberRange");
            cvBUNumber.ErrorMessage = ResHelper.GetString("Kadena.BusinessUnit.BUNumberUnique");
            btnCancel.Text = ResHelper.GetString("Kadena.BusinessUnit.CancelText");
            btnSave.Text = Request.QueryString["itemID"] != null ? ResHelper.GetString("Kadena.BusinessUnit.UpdateText") : ResHelper.GetString("Kadena.BusinessUnit.SaveText");
        }
        catch (Exception ex)
        {
            EventLogProvider.LogException("BusinessUnit.ascx.cs", "BindResourceStrings()", ex);
        }
    }

    /// <summary>
    /// binds the data to controls
    /// </summary>
    private void BindBusinessUnitData(int itemID)
    {
        try
        {
            var buData = CustomTableItemProvider.GetItems<BusinessUnitItem>().WhereEquals("ItemID", itemID).FirstOrDefault();
            if (!DataHelper.DataSourceIsEmpty(buData))
            {
                txtBUNumber.Text = ValidationHelper.GetString(buData.BusinessUnitNumber, string.Empty);
                txtBUName.Text = ValidationHelper.GetString(buData.BusinessUnitName, string.Empty);
                ddlStatus.SelectedValue = buData.Status ? "1" : "0";
            }
        }
        catch (Exception ex)
        {
            EventLogProvider.LogException("BusinessUnit.ascx.cs", "BindBusinessUnitData()", ex);
        }
    }

    /// <summary>
    /// binding the status of business units
    /// </summary>
    private void BindBusinessUnitStatus()
    {
        ddlStatus.Items.Add(new ListItem(ResHelper.GetString("KDA.Common.Status.Active"), "1"));
        ddlStatus.Items.Add(new ListItem(ResHelper.GetString("KDA.Common.Status.Inactive"), "0"));
    }

    /// <summary>
    /// Inserts business unit
    /// </summary>
    public void InsertBusinessUnit()
    {
        try
        {
            BusinessUnitItem objBusinessUnit = new BusinessUnitItem();
            objBusinessUnit.BusinessUnitNumber = ValidationHelper.GetLong(txtBUNumber.Text, default(int)); ;
            objBusinessUnit.BusinessUnitName = ValidationHelper.GetString(txtBUName.Text, string.Empty);
            objBusinessUnit.Status = ValidationHelper.GetBoolean(ddlStatus.SelectedValue, true);
            objBusinessUnit.SiteID = CurrentSite.SiteID;
            objBusinessUnit.Insert();
        }
        catch (Exception ex)
        {
            EventLogProvider.LogException("BusinessUnit.ascx.cs", "InsertBusinessUnit()", ex);
        }
    }

    /// <summary>
    /// updates the business unit data
    /// </summary>
    /// <param name="itemID"></param>
    private void UpdateBusinessUnit(int itemID)
    {
        try
        {
            var buData = CustomTableItemProvider.GetItem<BusinessUnitItem>(itemID);
            if (!DataHelper.DataSourceIsEmpty(buData))
            {
                buData.BusinessUnitNumber = ValidationHelper.GetLong(txtBUNumber.Text, default(int));
                buData.BusinessUnitName = ValidationHelper.GetString(txtBUName.Text, string.Empty);
                buData.Status = ValidationHelper.GetBoolean(ddlStatus.SelectedValue, true);
                buData.SiteID = CurrentSite.SiteID;
                buData.Update();
            }
        }
        catch (Exception ex)
        {
            EventLogProvider.LogException("BusinessUnit.ascx.cs", "UpdateBusinessUnit()", ex);
        }
    }

    #endregion "Custom methods"

    /// <summary>
    /// Inserting and updating the business unit
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            if (Page.IsValid)
            {
                int itemID = QueryHelper.GetInteger("itemID", 0);
                if (itemID > 0)
                {
                    UpdateBusinessUnit(itemID);
                    URLHelper.Redirect($"{CurrentDocument.Parent.DocumentUrlPath}?status={QueryStringStatus.Updated}");
                }
                else
                {
                    InsertBusinessUnit();
                    URLHelper.Redirect($"{CurrentDocument.Parent.DocumentUrlPath}?status={QueryStringStatus.Added}");
                }
            }
        }
        catch (Exception ex)
        {
            EventLogProvider.LogException("BusinessUnit.ascx.cs", "InsertBusinessUnit()", ex);
        }
    }

    /// <summary>
    /// redirects to listing page
    /// </summary>
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        URLHelper.Redirect(CurrentDocument.Parent.DocumentUrlPath);
    }

    /// <summary>
    /// custom validator for checking the uniqueness of the busines unit number
    /// </summary>
    protected void cvBUNumber_ServerValidate(object source, System.Web.UI.WebControls.ServerValidateEventArgs args)
    {
        try
        {
            BusinessUnitItem objBusinessUnit = new BusinessUnitItem();
            var buNumber = ValidationHelper.GetLong(txtBUNumber.Text, default(int));
            int itemID = QueryHelper.GetInteger("itemID", 0);
            if (itemID > 0)
            {
                var buData = CustomTableItemProvider.GetItems<BusinessUnitItem>()
                                                    .WhereEquals("BusinessUnitNumber", buNumber)
                                                    .And()
                                                    .WhereNotEquals("ItemID", itemID)
                                                    .Columns("BusinessUnitNumber")
                                                    .FirstOrDefault();
                args.IsValid = DataHelper.DataSourceIsEmpty(buData);
            }
            else
            {
                var buData = CustomTableItemProvider.GetItems<BusinessUnitItem>()
                                                    .WhereEquals("BusinessUnitNumber", buNumber)
                                                    .Columns("BusinessUnitNumber")
                                                    .FirstOrDefault();
                args.IsValid = DataHelper.DataSourceIsEmpty(buData);
            }
        }
        catch (Exception ex)
        {
            EventLogProvider.LogException("BusinessUnit.ascx.cs", "cvBUNumber_ServerValidate()", ex);
        }
    }
}