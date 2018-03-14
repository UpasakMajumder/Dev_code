using System;
using System.Data;
using System.Collections;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using CMS.PortalEngine.Web.UI;
using CMS.Helpers;
using CMS.DocumentEngine;
using CMS.Membership;
using CMS.EventLog;
using CMS.DataEngine;
using CMS.PortalEngine;
using CMS.DocumentEngine.Types.KDA;
using CMS.SiteProvider;
using Kadena.Old_App_Code.Kadena.Constants;
using CMS.CustomTables;
using CMS.CustomTables.Types.KDA;
using System.Linq;
using Kadena.WebAPI.KenticoProviders.Contracts;
using Kadena.Container.Default;

public partial class CMSWebParts_Campaign_CreateCampaign : CMSAbstractWebPart
{
    #region "Variables"

    private string folderpath = "/";
    private int campaignId = 0;
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
        if (!this.StopProcessing)
        {
            BindFiscalYears();
            Name.Attributes.Add("PlaceHolder", ResHelper.GetString("Kadena.CampaignForm.txtNameWatermark"));
            Description.Attributes.Add("PlaceHolder", ResHelper.GetString("Kadena.CampaignForm.txtDesWatermark"));
            txtStartDate.Attributes.Add("PlaceHolder", ResHelper.GetString("Kadena.CampaignForm.txtStartDateWatermark"));
            txtEndDate.Attributes.Add("PlaceHolder", ResHelper.GetString("Kadena.CampaignForm.txtEndDateWatermark"));
            rfvUserNameRequired.ErrorMessage = ResHelper.GetString("Kadena.CampaignForm.NameRequired");
            rvDescription.ErrorMessage = ResHelper.GetString("Kadena.CampaignForm.DesMaxLength");
            rfvStartDate.ErrorMessage = ResHelper.GetString("Kadena.CampaignForm.StartDateRequired");
            compareWithStartdate.ErrorMessage= ResHelper.GetString("Kadena.CampaignForm.EndDateRangeMessage");
            compareDate.ErrorMessage = ResHelper.GetString("Kadena.CampaignForm.StartDaterangeMessage");
            rfvEndDate.ErrorMessage = ResHelper.GetString("Kadena.CampaignForm.EndDateRequired");
            rvName.ErrorMessage = ResHelper.GetString("Kadena.CampaignForm.NameMaxLength");
            rqFiscalYear.ErrorMessage = ResHelper.GetString("Kadena.CampaignForm.FiscalYearErrorMessage");
            ddlStatus.Items.Insert(0, new ListItem(ResHelper.GetString("KDA.Common.Status.Active"), "1"));
            ddlStatus.Items.Insert(1, new ListItem(ResHelper.GetString("KDA.Common.Status.Inactive"), "0"));
            folderpath = DIContainer.Resolve<IKenticoResourceService>().GetSettingsKey("KDA_CampaignFolderPath");  
            if (Request.QueryString["ID"] != null)
            {
                btnSave.Click += btnSave_Edit;
                campaignId = ValidationHelper.GetInteger(Request.QueryString["ID"], 0);
                SetFeild(campaignId);
            }
            else
            {
                btnSave.Click += btnSave_Save;
            }
            if (!IsPostBack)
            {
                string currentDate = DateTime.Today.ToShortDateString();
                compareDate.ValueToCompare = currentDate;
            }
        }
    }
    /// <summary>
    /// Insert Campaign to database
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Save(object sender, EventArgs e)
    {
        string campaignName = Name.Text;
        string campaignDes = Description.Text;
        try
        {
            if (!string.IsNullOrEmpty(campaignName))
            {
                TreeProvider tree = new TreeProvider(MembershipContext.AuthenticatedUser);
                CMS.DocumentEngine.TreeNode parentPage = tree.SelectNodes().Path(folderpath).OnCurrentSite().Culture(DocumentContext.CurrentDocument.DocumentCulture).FirstObject;
                if (parentPage != null)
                {
                    Campaign newCampaign = new Campaign()
                    {
                        Name = campaignName,
                        DocumentName = campaignName,
                        Description = campaignDes,
                        StartDate = ValidationHelper.GetDate(txtStartDate.Text, DateTime.Now.Date),
                        EndDate = ValidationHelper.GetDate(txtEndDate.Text, DateTime.Now.Date),
                        FiscalYear = ddlFiscalYear.SelectedValue,
                        Status = ValidationHelper.GetString(ddlStatus.SelectedValue, "1") == "1" ? true : false,
                        DocumentCulture = CurrentDocument.DocumentCulture,
                        DocumentPageTemplateID = DocumentPageTemplate()
                    };
                    newCampaign.Insert(parentPage);
                    lblSuccessMsg.Visible = true;
                    lblFailureText.Visible = false;
                    Name.Text = "";
                    Description.Text = "";
                    Response.Cookies["status"].Value = QueryStringStatus.Added;
                    Response.Cookies["status"].HttpOnly = false;
                    URLHelper.Redirect($"{CurrentDocument.Parent.DocumentUrlPath}?status={QueryStringStatus.Added}");
                }
                else
                {
                    lblFailureText.Visible = true;
                }
            }
        }
        catch (Exception ex)
        {
            EventLogProvider.LogException("CampaignCreateForm", "EXCEPTION", ex);
        }
    }
    /// <summary>
    /// Back to Campain Listing page
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnCancel_Cancel(object sender, EventArgs e)
    {
        URLHelper.Redirect(CurrentDocument.Parent.DocumentUrlPath);
    }
    /// <summary>
    /// Saving the updated values in database
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Edit(object sender, EventArgs e)
    {
        string campaignName = Name.Text;
        string campaignDes = Description.Text;
        try
        {
            if (!string.IsNullOrEmpty(campaignName))
            {
                var editPage = DIContainer.Resolve<IKenticoCampaignsProvider>().GetCampaign(campaignId);
                if (editPage != null)
                {
                    editPage.DocumentName = campaignName;
                    editPage.DocumentCulture = DocumentContext.CurrentDocument.DocumentCulture;
                    editPage.SetValue("Name", campaignName);
                    editPage.SetValue("Description", campaignDes);
                    editPage.SetValue("StartDate", ValidationHelper.GetDate(txtStartDate.Text, DateTime.Now.Date));
                    editPage.SetValue("EndDate", ValidationHelper.GetDate(txtEndDate.Text, DateTime.Now.Date));
                    editPage.SetValue("FiscalYear", ddlFiscalYear.SelectedValue);
                    editPage.SetValue("Status", ValidationHelper.GetString(ddlStatus.SelectedValue, "1") == "1" ? true : false);
                    editPage.Update();
                    Response.Cookies["status"].Value = QueryStringStatus.Updated;
                    Response.Cookies["status"].HttpOnly = false;
                    URLHelper.Redirect($"{CurrentDocument.Parent.DocumentUrlPath}?status={QueryStringStatus.Updated}");
                }
                else
                {
                    lblFailureText.Visible = true;
                }
            }
        }
        catch (Exception ex)
        {
            EventLogProvider.LogException("CampaignCreateFormEdit", "EXCEPTION", ex);
        }
    }
    /// <summary>
    /// Set the Data 
    /// </summary>
    /// <param name="_campaignId"></param>
    private void SetFeild(int _campaignId)
    {
        try
        {
            var editPage = DIContainer.Resolve<IKenticoCampaignsProvider>().GetCampaign(_campaignId);
            string gAdminRoleName = DIContainer.Resolve<IKenticoResourceService>().GetSettingsKey(CurrentSite.SiteID, "KDA_GlobalAminRoleName");
            if (editPage != null)
            {
                Name.Text = editPage.GetValue("Name").ToString();
                Description.Text = editPage.GetValue("Description").ToString();
                var startDate = editPage.GetValue<DateTime>("StartDate", default(DateTime));
                txtStartDate.Text = (startDate != default(DateTime))? startDate.ToShortDateString():string.Empty;
                var endDate = editPage.GetValue<DateTime>("EndDate", default(DateTime));
                txtEndDate.Text = (endDate != default(DateTime)) ? endDate.ToShortDateString() : string.Empty;
                ddlFiscalYear.SelectedValue = editPage.GetValue("FiscalYear", string.Empty);
                ddlStatus.SelectedValue = ValidationHelper.GetBoolean(editPage.GetValue("Status"), false) == true ? "1" : "0";
                var opencamp = editPage.GetValue("OpenCampaign", false);
                var closecamp = editPage.GetValue("CloseCampaign", false);
                if (opencamp)
                {
                    Name.ReadOnly = true;
                    Description.ReadOnly = true;
                    txtStartDate.Enabled = false;
                    if (!CurrentUser.IsInRole(gAdminRoleName, CurrentSiteName))
                    {
                        txtEndDate.Enabled = false;
                    }
                    else
                    {
                        txtEndDate.Enabled = !closecamp;
                    }
                    ddlStatus.Enabled = false;
                    rfvStartDate.Enabled = false;
                    compareDate.Enabled = false;
                }
            }
        }
        catch (Exception ex)
        {
            EventLogProvider.LogException("CampaignCreateFormEdit", "EXCEPTION", ex);
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
    /// <summary>
    /// Get the Campaign Products Tempalte ID
    /// </summary>
    /// <returns>TemplateID</returns>
    public static int DocumentPageTemplate()
    {
        try
        {
            string templteName = DIContainer.Resolve<IKenticoResourceService>().GetSettingsKey("KDA_CampaignProductsTemplateName");
            var pageTemplateInfo = PageTemplateInfoProvider.GetPageTemplateInfo(templteName);
            if (!DataHelper.DataSourceIsEmpty(pageTemplateInfo))
                return pageTemplateInfo.PageTemplateId;
            else
                return default(int);
        }
        catch (Exception ex)
        {
            EventLogProvider.LogInformation("Get DocumentPage Tempate", "DocumentPageTemplate", ex.Message);
            return default(int); ;
        }
    }
    /// <summary>
    /// Binding fiscal years to dropdown
    /// </summary>
    public void BindFiscalYears()
    {
        try
        {
            var fiscalYears = CustomTableItemProvider.GetItems<FiscalYearManagementItem>().Where(x=>x.FiscalYearEndDate > DateTime.Now.Date).ToList();
            if (!DataHelper.DataSourceIsEmpty(fiscalYears))
            {
                ddlFiscalYear.DataSource = fiscalYears;
                ddlFiscalYear.DataTextField = "Year";
                ddlFiscalYear.DataValueField = "Year";
                ddlFiscalYear.DataBind();
            }
            ddlFiscalYear.Items.Insert(0, new ListItem(ResHelper.GetString("Kadena.Campaign.SelectFiscalYear"), "0"));

        }
        catch (Exception ex)
        {
            EventLogProvider.LogInformation("Binding Fiscal years", "DocumentPageTemplate", ex.Message);
        }

    }
    #endregion
}



