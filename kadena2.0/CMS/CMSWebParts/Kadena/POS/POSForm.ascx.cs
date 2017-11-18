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

public partial class CMSWebParts_Kadena_POSForm : CMSAbstractWebPart
{
    #region "Properties"
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
        if (this.StopProcessing)
        {
            // Do not process
        }
        else
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
    //This method will return the Brand list 
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
    // Method to bind the data to the dropdowns
    private void BindData()
    {
       
        try
        {
            //Binding data to Brand dropdownlist
            ddlBrand.Items.Insert(0, new ListItem(ResHelper.GetString("Kadena.POSFrom.BrandWaterMark"), "0"));
            ObjectQuery<CustomTableItem> Brands = GetBrands();
            int index = 1;
            foreach (CustomTableItem Brand in Brands)
            {
                ddlBrand.Items.Insert(index++, new ListItem(Brand.GetValue("BrandName").ToString(), Brand.GetValue("BrandCode").ToString()));
            }
            // Adds the '(any)' and '(default)' filtering options
            ddlYear.Items.Insert(0, new ListItem(ResHelper.GetString("Kadena.POSFrom.FiscalYearWaterMark"), "0"));
            int currentYear = DateTime.Now.Year;
            for (int NoOfYear = 0; NoOfYear < 3; NoOfYear++)
            {
                string year = (currentYear + NoOfYear).ToString();
                ddlYear.Items.Insert(NoOfYear + 1, new ListItem(year, year));
            }
            //POS Category DropdownList
            ddlCategory.Items.Insert(0, new ListItem(ResHelper.GetString("Kadena.POSFrom.POSCategoryWaterMark"), "0"));
            ddlCategory.Items.Insert(1, new ListItem("DEALER MERCHANDISE / DEALER INCENTIVES", "1"));
            ddlCategory.Items.Insert(2, new ListItem("FLOW", "2"));
            ddlCategory.Items.Insert(3, new ListItem("FALL PROMO", "3"));
            ddlCategory.Items.Insert(4, new ListItem("FLYERS/BROCHURES", "4"));
            ddlCategory.Items.Insert(5, new ListItem("HOLIDAY PROMO ", "5"));
        }
        catch (Exception ex)
        {
            EventLogProvider.LogInformation("CMSWebParts_Kadena_POS_POSForm_BindDataToDropdowns", "BindData", ex.Message);
        }
    }
    //Method to populate the pos form feilds for updating the record
    //private void SetFeilds(int posId)
    //{
    //    // Prepares the code name (class name) of the custom table
    //    string customTableClassName = "KDA.POSNumber";
    //    try
    //    {
    //        // Gets the custom table
    //        DataClassInfo brandTable = DataClassInfoProvider.GetDataClassInfo(customTableClassName);
    //        if (brandTable != null)
    //        {
    //            // Gets all data records from the POS table whose 'ItemId' field value equal to PosId
    //            CustomTableItem customTableData = CustomTableItemProvider.GetItem(posId, customTableClassName);
    //            if(customTableData!=null)
    //            {
    //                ddlBrand.SelectedValue = customTableData.GetValue("BrandID").ToString();
    //                ddlYear.SelectedValue = customTableData.GetValue("Year").ToString();
    //                ddlCategory.SelectedValue = customTableData.GetValue("POSCategoryID").ToString();
    //                txtPOSCode.Text = customTableData.GetValue("POSCode").ToString();
    //                txtPOSNumber.Text = customTableData.GetValue("POSNumber").ToString();
    //            }
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        EventLogProvider.LogInformation("CMSWebParts_Kadena_POS_POSForm", "GetBrands", ex.Message);
    //    }
    //}
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
    //protected void btnEdit_EditPOS(object sender, EventArgs e)
    //{
    //    string customTableClassName = "KDA.POSNumber";
    //    try
    //    {
    //        // Gets the custom table
    //        DataClassInfo brandTable = DataClassInfoProvider.GetDataClassInfo(customTableClassName);
    //        if (brandTable != null)
    //        {
    //            // Gets all data records from the POS table whose 'ItemId' field value equal to PosId
    //            CustomTableItem customTableData = CustomTableItemProvider.GetItem(posId, customTableClassName);
    //            if(customTableData !=null)
    //            {
    //                customTableData.SetValue("BrandID", ValidationHelper.GetString(ddlBrand.SelectedValue, string.Empty));
    //                customTableData.SetValue("Year", ValidationHelper.GetString(ddlYear.SelectedValue, string.Empty));
    //                customTableData.SetValue("POSCategoryID", ValidationHelper.GetString(ddlCategory.SelectedValue, string.Empty));
    //                customTableData.SetValue("POSCode", ValidationHelper.GetString(txtPOSCode.Text, string.Empty));
    //                customTableData.SetValue("POSCategoryName", ValidationHelper.GetString(ddlCategory.SelectedItem.Text, string.Empty));
    //                customTableData.SetValue("POSNumber", ValidationHelper.GetString(ddlCategory.SelectedValue, string.Empty));
    //                customTableData.SetValue("BrandName", ValidationHelper.GetString(ddlBrand.SelectedItem.Text,string.Empty));
    //                customTableData.Update();
    //                lblError.Visible = false;
    //                lblSuccess.Visible = true;
    //            }
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        EventLogProvider.LogInformation("CMSWebParts_Kadena_POS_POSForm", "GetBrands", ex.Message);
    //        lblError.Visible = true;
    //    }
    //}
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
            //var redirectUrl = RequestContext.CurrentURL;

            //if (!String.IsNullOrEmpty(DefaultTargetUrl))
            //{
            //    redirectUrl = ResolveUrl(DefaultTargetUrl);
            //}

            //URLHelper.Redirect(redirectUrl);
        }
        catch (Exception ex)
        {
            EventLogProvider.LogException("CancePOSFormButtonClick", "EXCEPTION", ex);
        }

    }
    #endregion
}



