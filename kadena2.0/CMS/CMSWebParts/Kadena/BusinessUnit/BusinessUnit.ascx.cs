using System;

using CMS.PortalEngine.Web.UI;
using CMS.CustomTables.Types.KDA;
using CMS.Helpers;
using CMS.EventLog;
using CMS.CustomTables;
using System.Linq;
using System.Web.UI.WebControls;

public partial class CMSWebParts_Kadena_BusinessUnit_BusinessUnit : CMSAbstractWebPart
{
    #region "Properties"

    public string BUListNavigationURL
    {
        get
        {
            return  ValidationHelper.GetString(GetValue("BUListNavigationURL"),string.Empty);
        }
        set
        {
            SetValue("BUListNavigationURL", value);
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
            BindResourceStrings();
            BindBusinessUnitStatus();
            var itemID = Request.QueryString["itemID"] != null ? ValidationHelper.GetInteger(Request.QueryString["itemID"],default(int)) : default(int);
            if (itemID != default(int))
            {
                btnCancel.Text = ResHelper.GetString("Kadena.BusinessUnit.CancelText","Cancel");
                btnSave.Text= ResHelper.GetString("Kadena.BusinessUnit.UpdateText", "Cancel");
                BindBusinessUnitData(itemID);
            }
            btnSave.Text = ResHelper.GetString("Kadena.BusinessUnit.SaveText", "Cancel");

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

    #endregion

    #region "Custom methods"

    private void BindResourceStrings()
    {
        try
        {
            //Bind lables
            lblBUNumber.InnerText = ResHelper.GetString("Kadena.BusinessUnit.NumberText", "Business Unit Number");
            lblBUName.InnerText = ResHelper.GetString("Kadena.BusinessUnit.NameText", "Business Unit Name");
            lblBUStatus.InnerText = ResHelper.GetString("Kadena.BusinessUnit.StatusText", "Business Unit Number");

            //Bind error messages
            rfBUNumber.ErrorMessage = ResHelper.GetString("Kadena.BusinessUnit.BUNumberRequired", "Please enter Business Unit Number");
            rfBUName.ErrorMessage = ResHelper.GetString("Kadena.BusinessUnit.BUNameRequired", "Please enter Business Unit Name");
            revBUNumber.ErrorMessage = ResHelper.GetString("Kadena.BusinessUnit.BUNumberRange", "Business Unit Number must be in between 8 and 10 characters");
            cvBUNumber.ErrorMessage = ResHelper.GetString("Kadena.BusinessUnit.BUNumberUnique", "Business unit Number must be unique");
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
            var buData = CustomTableItemProvider.GetItems<BusinessUnitItem>().WhereEquals("ItemID", itemID).TopN(1).FirstOrDefault();
            if (!DataHelper.DataSourceIsEmpty(buData))
            {
                txtBUNumber.Text = ValidationHelper.GetString(buData.BusinessUnitNumber,string.Empty);
                txtBUName.Text = ValidationHelper.GetString(buData.BusinessUnitName,string.Empty);
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
        ddlStatus.Items.Add(new ListItem("Deactivate","0"));
        ddlStatus.Items.Add(new ListItem("Activate", "1"));
    }

    /// <summary>
    /// Inserts business unit
    /// </summary>
    public void InsertBusinessUnit()
    {
        try
        {
            BusinessUnitItem objBusinessUnit = new BusinessUnitItem();
            objBusinessUnit.BusinessUnitNumber = ValidationHelper.GetInteger(txtBUNumber.Text, default(int));
            objBusinessUnit.BusinessUnitName = ValidationHelper.GetString(txtBUName.Text, string.Empty);
            objBusinessUnit.Status = ddlStatus.SelectedValue == "0" ? false : true;
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
            var buData = CustomTableItemProvider.GetItems<BusinessUnitItem>().WhereEquals("ItemID", itemID).TopN(1).FirstOrDefault();
            if (!DataHelper.DataSourceIsEmpty(buData))
            {
                buData.BusinessUnitNumber = ValidationHelper.GetInteger(txtBUNumber.Text, default(int));
                buData.BusinessUnitName = ValidationHelper.GetString(txtBUName.Text, string.Empty);
                buData.Status = ddlStatus.SelectedValue == "0" ? false : true;
                buData.SiteID = CurrentSite.SiteID;
                buData.Update();
            }
        }
        catch (Exception ex)
        {
            EventLogProvider.LogException("BusinessUnit.ascx.cs", "UpdateBusinessUnit()", ex);
        }
    }

    #endregion
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
               
                var itemID = Request.QueryString["itemID"] != null ? ValidationHelper.GetInteger(Request.QueryString["itemID"], default(int)) : default(int);
                if (itemID != default(int))
                    UpdateBusinessUnit(itemID);
                
                else
                    InsertBusinessUnit();

                Response.Redirect(BUListNavigationURL);
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
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect(BUListNavigationURL);
    }

    /// <summary>
    /// custom validator for checking the uniqueness of the busines unit number
    /// </summary>
    /// <param name="source"></param>
    /// <param name="args"></param>
    protected void cvBUNumber_ServerValidate(object source, System.Web.UI.WebControls.ServerValidateEventArgs args)
    {
        try
        {
            BusinessUnitItem objBusinessUnit = new BusinessUnitItem();
            var buNumber = ValidationHelper.GetInteger(txtBUNumber.Text, default(int));
            var itemID = Request.QueryString["itemID"] != null ? ValidationHelper.GetInteger(Request.QueryString["itemID"], default(int)) : default(int);
            if (itemID != default(int))
            {
                var buData = CustomTableItemProvider.GetItems<BusinessUnitItem>().WhereEquals("BusinessUnitNumber", buNumber).And().WhereNotEquals("ItemID",itemID).TopN(1).FirstOrDefault();
                if (!DataHelper.DataSourceIsEmpty(buData)) args.IsValid = false;
                else
                    args.IsValid = true;
            }
            else
            {
                var buData = CustomTableItemProvider.GetItems<BusinessUnitItem>().WhereEquals("BusinessUnitNumber", buNumber).TopN(1).FirstOrDefault();
                if (!DataHelper.DataSourceIsEmpty(buData)) args.IsValid = false;
                else
                    args.IsValid = true;
            }

        }
        catch (Exception ex)
        {
            EventLogProvider.LogException("BusinessUnit.ascx.cs", "cvBUNumber_ServerValidate()", ex);
        }

    }
}



