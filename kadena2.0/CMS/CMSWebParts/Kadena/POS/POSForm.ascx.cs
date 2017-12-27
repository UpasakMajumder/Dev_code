using System;
using System.Web.UI.WebControls;
using CMS.PortalEngine.Web.UI;
using CMS.Helpers;
using CMS.CustomTables;
using CMS.EventLog;
using CMS.CustomTables.Types.KDA;
using System.Linq;

public partial class CMSWebParts_Kadena_POSForm : CMSAbstractWebPart
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
    /// Method to bind the data to all the dropdowns
    /// </summary>
    private void BindData()
    {
        try
        {
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
                CustomTableItem posData = CustomTableItemProvider.GetItems<POSNumberItem>().WhereEquals("POSNumber", posNumber).FirstOrDefault();
                if (posData == null)
                {
                    POSNumberItem objPosNumber = new POSNumberItem
                    {
                        BrandID = ValidationHelper.GetInteger(ddlBrand.SelectedValue, default(int)),
                        Year = ValidationHelper.GetInteger(ddlYear.SelectedValue, default(int)),
                        POSCategoryName = ValidationHelper.GetString(ddlCategory.SelectedItem.Text, string.Empty),
                        POSCode = ValidationHelper.GetInteger(txtPOSCode.Text, default(int)),
                        POSCategoryID = ValidationHelper.GetInteger(ddlCategory.SelectedValue, default(int)),
                        BrandName = ValidationHelper.GetString(ddlBrand.SelectedItem.Text, string.Empty),
                        POSNumber = ValidationHelper.GetInteger(posNumber, default(int)),
                    };
                    objPosNumber.Insert();
                    Response.Redirect(CurrentDocument.Parent.DocumentUrlPath, false);
                }
                else
                {
                    lblDuplicate.Visible = true;
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
        Response.Redirect(CurrentDocument.Parent.DocumentUrlPath);
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
        var brands = CustomTableItemProvider.GetItems(BrandItem.CLASS_NAME).Columns("BrandCode,BrandName").WhereEquals("Status",1).OrderBy("BrandName").ToList();
        if (!DataHelper.DataSourceIsEmpty(brands))
        {
            ddlBrand.DataSource = brands;
            ddlBrand.DataTextField = "BrandName";
            ddlBrand.DataValueField = "BrandCode";
            ddlBrand.DataBind();
            string selectText = ValidationHelper.GetString(ResHelper.GetString("Kadena.InvProductForm.BrandWaterMark"), string.Empty);
            ddlBrand.Items.Insert(0, new ListItem(selectText, "0"));
        }
    }
    #endregion
}
