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
            // Assign localization text to labels
            lblProgramName.InnerText = ProgramNameText;
            lblProgramDescription.InnerText = ProgramDescriptionText;
            lblBrandName.InnerText = BrandNameText;
            lblCampaignName.InnerText = CampaignNameText;
            btnAddProgram.Text = SaveButtonText;
            btnCancelProgram.Text = CancelButtonText;
            btnUpdateProgram.Text = UpdateButtonText;
            programNameRequired.ErrorMessage = ResHelper.GetString("Kadena.Programs.ProgramNameRequired");
            cvDesc.ErrorMessage = ResHelper.GetString("Kadena.Programs.ProgramDescError");
            //get program details by program id.
            int programID = ValidationHelper.GetInteger(Request.QueryString["id"], 0);
            if (programID != 0)
            {
                Program program = ProgramProvider.GetPrograms().WhereEquals("ProgramID", programID).TopN(1).FirstOrDefault();
                if (program != null)
                {
                    txtProgramName.Text = program.ProgramName;
                    txtProgramDescription.Text = program.ProgramDescription;
                    ddlBrand.Value = program.BrandID.ToString();
                    ddlCampaign.Value = program.CampaignID;
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
            int campaignID = ValidationHelper.GetInteger(ddlCampaign.Value, 0);
            if (campaignID != 0)
            {
                Campaign campaign = new Campaign();
                TreeProvider tree = new TreeProvider(MembershipContext.AuthenticatedUser);
                Campaign CampaignNode = CampaignProvider.GetCampaigns().WhereEquals("CampaignID", campaignID).FirstOrDefault();
                if (Page.IsValid)
                {
                    if (CampaignNode != null)
                    {
                        Program program = new Program();
                        program.DocumentName = txtProgramName.Text;
                        program.DocumentCulture = SiteContext.CurrentSite.DefaultVisitorCulture;
                        program.ProgramName = txtProgramName.Text;
                        program.ProgramDescription = txtProgramDescription.Text;
                        program.BrandID = ValidationHelper.GetInteger(ddlBrand.Value, 0);
                        program.CampaignID = ValidationHelper.GetInteger(ddlCampaign.Value, 0);
                        program.Insert(CampaignNode, true);

                        URLHelper.Redirect(CurrentDocument.Parent.DocumentUrlPath);
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
            int campaignID = ValidationHelper.GetInteger(ddlCampaign.Value, 0);
            TreeProvider tree = new TreeProvider(MembershipContext.AuthenticatedUser);
            if (Page.IsValid)
            {
                if (ViewState["programNodeID"] != null)
                {
                    Program program = ProgramProvider.GetProgram(ValidationHelper.GetInteger(ViewState["programNodeID"], 0), CurrentDocument.DocumentCulture, CurrentSiteName);

                    if (program != null)
                    {
                        program.DocumentName = txtProgramName.Text;
                        program.ProgramName = txtProgramName.Text;
                        program.ProgramDescription = txtProgramDescription.Text;
                        program.BrandID = ValidationHelper.GetInteger(ddlBrand.Value, 0);
                        program.CampaignID = ValidationHelper.GetInteger(ddlCampaign.Value, 0);
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
}