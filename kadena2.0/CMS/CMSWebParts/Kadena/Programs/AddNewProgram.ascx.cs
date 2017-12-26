using CMS.CustomTables;
using CMS.CustomTables.Types.KDA;
using CMS.DocumentEngine;
using CMS.DocumentEngine.Types.KDA;
using CMS.EventLog;
using CMS.Helpers;
using CMS.Membership;
using CMS.PortalEngine.Web.UI;
using CMS.SiteProvider;
using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class CMSWebParts_Kadena_Programs_AddNewProgram : CMSAbstractWebPart
{
    #region "Properties"

    /// <summary>
    /// Program name localization string
    /// </summary>
    public string ProgramNameText
    {
        get
        {
            return ValidationHelper.GetString(ResHelper.GetString("Kadena.Program.ProgramName"), "");
        }
        set
        {
            SetValue("ProgramNameText", value);
        }
    }

    /// <summary>
    /// Program description localization string
    /// </summary>
    public string ProgramDescriptionText
    {
        get
        {
            return ValidationHelper.GetString(ResHelper.GetString("Kadena.Program.ProgramDescription"), "");
        }
        set
        {
            SetValue("ProgramDescriptionText", value);
        }
    }

    /// <summary>
    /// BrandName localization string
    /// </summary>
    public string BrandNameText
    {
        get
        {
            return ValidationHelper.GetString(ResHelper.GetString("Kadena.Program.BrandName"), "");
        }
        set
        {
            SetValue("BrandNameText", value);
        }
    }

    /// <summary>
    /// Campaign name localization string
    /// </summary>
    public string CampaignNameText
    {
        get
        {
            return ValidationHelper.GetString(ResHelper.GetString("Kadena.Program.CampaignName"), "");
        }
        set
        {
            SetValue("CampaignNameText", value);
        }
    }

    /// <summary>
    /// SaveButton localization string
    /// </summary>
    public string SaveButtonText
    {
        get
        {
            return ValidationHelper.GetString(ResHelper.GetString("Kadena.Program.SaveButton"), "");
        }
        set
        {
            SetValue("SaveButtonText", value);
        }
    }

    /// <summary>
    /// UpdateButton localization string
    /// </summary>
    public string UpdateButtonText
    {
        get
        {
            return ValidationHelper.GetString(ResHelper.GetString("Kadena.Program.UpdateButton"), "");
        }
        set
        {
            SetValue("UpdateButtonText", value);
        }
    }
    /// <summary>
    /// Status localization string
    /// 
    /// </summary>
    public string StatusTest
    {
        get
        {
            return ValidationHelper.GetString(ResHelper.GetString("Kadena.Program.StatusName"), "");
        }
        set
        {
            SetValue("CampaignNameText", value);
        }
    }


    /// <summary>
    /// CancelButton localization string
    /// </summary>
    public string CancelButtonText
    {
        get
        {
            return ValidationHelper.GetString(ResHelper.GetString("Kadena.Program.CancelButton"), "");
        }
        set
        {
            SetValue("CancelButtonText", value);
        }
    }

    /// <summary>
    /// Delivery Date localization string
    /// </summary>
    public string DeliveryDateToDistributors
    {
        get
        {
            return ValidationHelper.GetString(ResHelper.GetString("Kadena.Program.DeliveryDateToDistributors"), "");
        }
        set
        {
            SetValue("DeliveryDateToDistributors", value);
        }
    }

    #endregion "Properties"

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
            lblDateValid.Visible = false;
            lblProgramName.InnerText = ProgramNameText;
            lblProgramDescription.InnerText = ProgramDescriptionText;
            lblBrandName.InnerText = BrandNameText;
            lblCampaignName.InnerText = CampaignNameText;
            btnAddProgram.Text = SaveButtonText;
            lblStatus.InnerText = StatusTest;
            lblProgramDeliveryDate.InnerText = DeliveryDateToDistributors;
            btnCancelProgram.Text = CancelButtonText;
            btnUpdateProgram.Text = UpdateButtonText;
            programNameRequired.ErrorMessage = ResHelper.GetString("Kadena.Programs.ProgramNameRequired");
            cvDesc.ErrorMessage = ResHelper.GetString("Kadena.Programs.ProgramDescError");
            GetBrandName();
            GetCampaign();
            BindStatus();
            int programID = ValidationHelper.GetInteger(Request.QueryString["id"], 0);
            if (programID != 0)
            {
                Program program = ProgramProvider.GetPrograms().WhereEquals("ProgramID", programID).TopN(1).FirstOrDefault();
                if (program != null)
                {
                    txtProgramName.Text = program.ProgramName;
                    txtProgramDescription.Text = program.ProgramDescription;
                    ddlBrand.SelectedValue = program.BrandID.ToString();
                    txtProgramDeliveryDate.Text = program.DeliveryDateToDistributors == default(DateTime) ? string.Empty : program.DeliveryDateToDistributors.ToShortDateString();
                    ddlCampaign.SelectedValue = program.CampaignID.ToString();
                    btnAddProgram.Visible = false;
                    btnUpdateProgram.Visible = true;
                    ViewState["CampaignID"] = program.CampaignID;
                    ViewState["programNodeID"] = program.NodeID;
                }
            }
            else
            {
                btnAddProgram.Visible = true;
                btnUpdateProgram.Visible = false;
            }
        }
    }

    /// <summary>
    /// Get the brand list
    /// </summary>
    /// <param name="brandItemID"></param>
    /// <returns></returns>
    public string GetBrandName()
    {
        string returnValue = string.Empty;
        try
        {
            var brands = CustomTableItemProvider.GetItems(BrandItem.CLASS_NAME)
                .Columns("ItemID,BrandName")
                .WhereEquals("Status", 1)
                .ToList();
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
        catch (Exception ex)
        {
            EventLogProvider.LogException("CMSWebParts_Kadena_Campaign_Web_Form_AddCampaignProducts", "GetBrandName", ex, CurrentSite.SiteID, ex.Message);
        }
        return returnValue;
    }
    /// <summary>
    /// Get the brand list
    /// </summary>
    /// <param name="brandItemID"></param>
    /// <returns></returns>
    public string GetCampaign()
    {
        string returnValue = string.Empty;
        try
        {
            var Campaigns = CampaignProvider.GetCampaigns()
                .Columns("CampaignID,Name")
                .WhereEquals("Status", 1)
                .ToList();
            if (!DataHelper.DataSourceIsEmpty(Campaigns))
            {
                ddlCampaign.DataSource = Campaigns;
                ddlCampaign.DataTextField = "Name";
                ddlCampaign.DataValueField = "CampaignID";
                ddlCampaign.DataBind();
                string selectText = ValidationHelper.GetString(ResHelper.GetString("Kadena.ProgramForm.CampaignWaterMark"), string.Empty);
                ddlCampaign.Items.Insert(0, new ListItem(selectText, "0"));
            }
        }
        catch (Exception ex)
        {
            EventLogProvider.LogException("CMSWebParts_Kadena_Campaign_Web_Form_AddCampaignProducts", "GetCampaignName", ex, CurrentSite.SiteID, ex.Message);
        }
        return returnValue;
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

    /// <summary>
    /// Create New Program
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnAddProgram_Click(object sender, EventArgs e)
    {
        try
        {
            int campaignID = ValidationHelper.GetInteger(ddlCampaign.SelectedValue, 0);
            if (campaignID != 0)
            {
                Campaign campaign = new Campaign();
                TreeProvider tree = new TreeProvider(MembershipContext.AuthenticatedUser);
                Campaign CampaignNode = CampaignProvider.GetCampaigns().WhereEquals("CampaignID", campaignID).FirstOrDefault();
                if (Page.IsValid)
                {
                    if (CampaignNode != null)
                    {
                        if (ValidationHelper.GetDate(txtProgramDeliveryDate.Text, default(DateTime)).Date >= DateTime.Today)
                        {
                            lblDateValid.Visible = false;
                            Program program = new Program()
                            {
                                DocumentName = txtProgramName.Text,
                                DocumentCulture = SiteContext.CurrentSite.DefaultVisitorCulture,
                                ProgramName = txtProgramName.Text,
                                ProgramDescription = txtProgramDescription.Text,
                                BrandID = ValidationHelper.GetInteger(ddlBrand.SelectedValue, 0),
                                CampaignID = ValidationHelper.GetInteger(ddlCampaign.SelectedValue, 0),
                                Status = ValidationHelper.GetBoolean(ddlStatus.SelectedValue, true),
                                DeliveryDateToDistributors = ValidationHelper.GetDate(txtProgramDeliveryDate.Text, default(DateTime)).Date
                            };
                            program.Insert(CampaignNode, true);
                            URLHelper.Redirect(CurrentDocument.Parent.DocumentUrlPath);
                        }
                        else
                        {
                            lblDateValid.Visible = true;
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            EventLogProvider.LogInformation("CMSWebParts_Kadena_Programs_AddNewProgram", "btnAddProgram_Click", ex.Message);
        }
    }

    /// <summary>
    /// Back to Programs list page
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnCancelProgram_Click(object sender, EventArgs e)
    {
        URLHelper.Redirect(CurrentDocument.Parent.DocumentUrlPath);
    }

    /// <summary>
    /// Update Program
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnUpdateProgram_Click(object sender, EventArgs e)
    {
        try
        {
            int campaignID = ValidationHelper.GetInteger(ddlCampaign.SelectedValue, 0);
            TreeProvider tree = new TreeProvider(MembershipContext.AuthenticatedUser);
            if (Page.IsValid)
            {
                if (ViewState["programNodeID"] != null)
                {
                    if (ValidationHelper.GetDate(txtProgramDeliveryDate.Text, default(DateTime)).Date >= DateTime.Today)
                    {
                        lblDateValid.Visible = false;
                        Program program = ProgramProvider.GetProgram(ValidationHelper.GetInteger(ViewState["programNodeID"], 0), CurrentDocument.DocumentCulture, CurrentSiteName);

                        if (program != null)
                        {
                            program.DocumentName = txtProgramName.Text;
                            program.ProgramName = txtProgramName.Text;
                            program.ProgramDescription = txtProgramDescription.Text;
                            program.BrandID = ValidationHelper.GetInteger(ddlBrand.SelectedValue, 0);
                            program.CampaignID = ValidationHelper.GetInteger(ddlCampaign.SelectedValue, 0);
                            program.Status = ValidationHelper.GetBoolean(ddlStatus.SelectedValue, true);
                            program.Update();
                        }
                        if (ViewState["CampaignID"] != null)
                        {
                            if (Convert.ToInt32(ViewState["CampaignID"]) != campaignID)
                            {
                                Campaign targetCampaign = CampaignProvider.GetCampaigns().WhereEquals("CampaignID", campaignID).FirstOrDefault();
                                if (targetCampaign != null && program != null)
                                    DocumentHelper.MoveDocument(program, targetCampaign, tree, true);
                            }
                        }
                        URLHelper.Redirect(CurrentDocument.Parent.DocumentUrlPath);
                    }
                    else
                    {
                        lblDateValid.Visible = true;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            EventLogProvider.LogInformation("CMSWebParts_Kadena_Programs_AddNewProgram", "btnUpdateProgram_Click", ex.Message);
        }
    }

    /// <summary>
    /// Validates description
    /// </summary>
    /// <param name="source"></param>
    /// <param name="args"></param>
    protected void cvDesc_ServerValidate(object source, ServerValidateEventArgs args)
    {
        try
        {
            args.IsValid = string.IsNullOrEmpty(txtProgramDescription.Text.Trim()) ? false : txtProgramDescription.Text.Trim().Length <= 140 && txtProgramDescription.Text.Trim().Length >= 1 ? true : false;
        }
        catch (Exception ex)
        {
            EventLogProvider.LogInformation("CMSWebParts_Kadena_Programs_AddNewProgram", "cvDesc_ServerValidate", ex.Message);
        }
    }
    public void BindStatus()
    {
        ddlStatus.Items.Clear();
        ddlStatus.Items.Insert(0, new ListItem(ResHelper.GetString("KDA.Common.Status.Active"), "1"));
        ddlStatus.Items.Insert(1, new ListItem(ResHelper.GetString("KDA.Common.Status.Inactive"), "0"));
    }
}