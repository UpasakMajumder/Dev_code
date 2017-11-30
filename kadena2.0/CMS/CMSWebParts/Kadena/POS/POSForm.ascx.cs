using System;
using System.Data;
using System.Collections;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CMS.PortalEngine.Web.UI;
using CMS.Helpers;
using CMS.DataEngine;
using CMS.CustomTables;
using CMS.EventLog;
using CMS.DocumentEngine;
using System.Collections.Generic;
using CMS.Membership;
using CMS.CustomTables.Types.KDA;
using System.Linq;

public partial class CMSWebParts_Kadena_POSForm : CMSAbstractWebPart
{
    #region "Variables"
    int posId = 0;
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
    public string DefaultTargetUrl
    {
        get
        {
            return ValidationHelper.GetString(GetValue("DefaultTargetUrl"), Request.UrlReferrer.ToString());
        }
        set
        {
            SetValue("DefaultTargetUrl", value);
        }
    }

    /// <summary>
    /// Initializes the control properties.
    /// </summary>
    protected void SetupControl()
    {
        if (!this.StopProcessing)
        {
            BindData();
            btnSave.Click += btnSave_SavePOS;
            btnCancel.Click += btnCancel_Cancel;
            rfvBrand.ErrorMessage = ResHelper.GetString("Kadena.POSFrom.BrandRequired");
            rfvYear.ErrorMessage = ResHelper.GetString("Kadena.POSFrom.YearRequired");
            rfvPOSCode.ErrorMessage = ResHelper.GetString("Kadena.POSFrom.POSCodeRequired");
            rfvCatgory.ErrorMessage = ResHelper.GetString("Kadena.POSFrom.POSCategroyRequired");
            revPOSCodeLength.ErrorMessage = ResHelper.GetString("Kadena.POSFrom.POSMaxLengthMsg");
            revPOSCode.ErrorMessage = ResHelper.GetString("Kadena.POSFrom.POSNumberOnlyMsg");
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
    #region
    /// <summary>
    /// This method will return the Brand list 
    /// </summary>
    /// <returns>List of Brands</returns>
    private static ObjectQuery<CustomTableItem> GetBrands()
    {
        // Prepares the code name (class name) of the custom table
        ObjectQuery<CustomTableItem> items = new ObjectQuery<CustomTableItem>();
        string customTableClassName = "KDA.Brand";
        try
        {
            // Gets the custom table
            DataClassInfo brandTable = DataClassInfoProvider.GetDataClassInfo(customTableClassName);
            if (brandTable != null)
            {
                // Gets a custom table records 
                items = CustomTableItemProvider.GetItems(customTableClassName).OrderBy("BrandName");

            }
        }
        catch (Exception ex)
        {
            EventLogProvider.LogInformation("CMSWebParts_Kadena_POS_POSForm", "GetBrands", ex.Message);
        }

        return items;
    }
    /// <summary>
    /// Method to bind the data to all the dropdowns
    /// </summary>
    private void BindData()
    {
        try
        {

            // Adds the '(any)' and '(default)' filtering options
            ddlYear.Items.Insert(0, new ListItem(ResHelper.GetString("Kadena.POSFrom.FiscalYearWaterMark"), "0"));
            int currentYear = DateTime.Now.Year;
            for (int NoOfYear = 0; NoOfYear < 3; NoOfYear++)
            {
                string year = (currentYear + NoOfYear).ToString();
                ddlYear.Items.Insert(NoOfYear + 1, new ListItem(year, year));
            }
            BindPOSCategories();
            BindBrands();

        }
        catch (Exception ex)
        {
            EventLogProvider.LogInformation("CMSWebParts_Kadena_POS_POSForm_BindDataToDropdowns", "BindData", ex.Message);
        }
    }
    #endregion
    #region Events
    /// <summary>
    /// On button click it will save the POS Number to custom table
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_SavePOS(object sender, EventArgs e)
    {
        try
        {
            string posNumber = ddlBrand.SelectedValue + ddlYear.SelectedValue.Substring(2) + txtPOSCode.Text;
            if (!string.IsNullOrEmpty(txtPOSCode.Text) && ddlBrand.SelectedIndex > 0 && ddlYear.SelectedIndex > 0)
            {
                string customTableClassName = "KDA.POSNumber";
                DataClassInfo customTable = DataClassInfoProvider.GetDataClassInfo(customTableClassName);
                if (customTable != null)
                {
                    //Validation for POSNumber
                    CustomTableItem customTableData = CustomTableItemProvider.GetItems(customTableClassName).WhereEquals("POSNumber", posNumber);
                    if (customTableData == null)
                    {
                        // Creates a new custom table item
                        CustomTableItem newCustomTableItem = CustomTableItem.New(customTableClassName);
                        // Sets the values for the fields of the custom table (ItemText in this case) 
                        newCustomTableItem.SetValue("BrandID", ValidationHelper.GetString(ddlBrand.SelectedValue, ""));
                        newCustomTableItem.SetValue("Year", ValidationHelper.GetString(ddlYear.SelectedValue, ""));
                        newCustomTableItem.SetValue("POSCode", ValidationHelper.GetString(txtPOSCode.Text, ""));
                        newCustomTableItem.SetValue("POSCategoryID", ValidationHelper.GetString(ddlCategory.SelectedValue, ""));
                        newCustomTableItem.SetValue("POSCategoryName", ValidationHelper.GetString(ddlCategory.SelectedItem.Text, ""));
                        newCustomTableItem.SetValue("BrandName", ValidationHelper.GetString(ddlBrand.SelectedItem.Text, ""));
                        newCustomTableItem.SetValue("POSNumber", posNumber);
                        // Save the new custom table record into the database
                        newCustomTableItem.Insert();
                        lblError.Visible = false;
                        lblSuccess.Visible = true;
                        lblDuplicate.Visible = false;
                    }
                    else
                    {
                        lblDuplicate.Visible = true;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            EventLogProvider.LogException("SavePOSFromButtonClick", "EXCEPTION", ex);
            lblError.Visible = true;
            lblSuccess.Visible = false;
        }

    }
    protected void btnCancel_Cancel(object sender, EventArgs e)
    {
        try
        {
            ddlBrand.SelectedIndex = 0;
            ddlCategory.SelectedIndex = 0;
            ddlYear.SelectedIndex = 0;
            txtPOSCode.Text = "";
            txtPOSNumber.Text = "";
            lblSuccess.Visible = false;
            lblError.Visible = false;

        }
        catch (Exception ex)
        {
            EventLogProvider.LogException("CancePOSFormButtonClick", "EXCEPTION", ex);
        }

    }
    private void BindPOSCategories()
    {
        var posCategories = CustomTableItemProvider.GetItems(POSCategoryItem.CLASS_NAME).Columns("PosCategoryName,ItemID").ToList();
        if (!DataHelper.DataSourceIsEmpty(posCategories))
        {
            ddlCategory.DataSource = posCategories;
            ddlCategory.DataTextField = "PosCategoryName";
            ddlCategory.DataValueField = "ItemID";
            ddlCategory.DataBind();
            string selectText = ValidationHelper.GetString(ResHelper.GetString("Kadena.POSFrom.POSCategoryWaterMark"), string.Empty);
            ddlCategory.Items.Insert(0, new ListItem(selectText, "0"));
        }
    }
    private void BindBrands()
    {
        var brands = CustomTableItemProvider.GetItems(BrandItem.CLASS_NAME).Columns("ItemID,BrandName").ToList();
        if (!DataHelper.DataSourceIsEmpty(brands))
        {
            ddlBrand.DataSource = brands;
            ddlBrand.DataTextField = "BrandName";
            ddlBrand.DataValueField = "ItemID";
            ddlBrand.DataBind();
            string selectText = ValidationHelper.GetString(ResHelper.GetString("Kadena.InvProductForm.BrandWaterMark"), string.Empty);
            ddlBrand.Items.Insert(0, new ListItem(selectText, "0"));
        }
    }
    #endregion
}
