using System;
using System.Data;
using System.Collections;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using CMS.PortalEngine.Web.UI;
using CMS.Helpers;
using CMS.DataEngine;
using CMS.DocumentEngine;
using CMS.Membership;
using System.Linq;
using CMS.SiteProvider;
using CMS.EventLog;
using CMS.DocumentEngine.Types.KDA;

public partial class CMSWebParts_Kadena_Programs_AddNewProgram : CMSAbstractWebPart
{
    #region "Properties"
    /// <summary>
    /// Progarm listing page url
    /// </summary>
    public string ProgramListURL
    {
        get
        {
            return ValidationHelper.GetString(GetValue("ProgramListURL"), "");
        }
        set
        {
            SetValue("ProgramListURL", value);
        }
    }
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
            // Assign localization text to labels
            lblProgramName.InnerText = ProgramNameText;
            lblProgramDescription.InnerText = ProgramDescriptionText;
            lblBrandName.InnerText = BrandNameText;
            lblCampaignName.InnerText = CampaignNameText;
            btnAddProgram.Text = SaveButtonText;
            btnCancelProgram.Text = CancelButtonText;
            btnUpdateProgram.Text = UpdateButtonText;

            //get program details by program id.
            int programID = ValidationHelper.GetInteger(Request.QueryString["id"], 0);
            if (programID != 0)
            {
                programHeader.InnerText = "Edit Program";
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

    #endregion
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
                var cmpaignDoc = CampaignProvider.GetCampaigns().WhereEquals("CampaignID", campaignID).FirstOrDefault();
                if (cmpaignDoc != null)
                {
                    int campaignNodeID = cmpaignDoc.NodeID;
                    var document = TreeHelper.SelectSingleNode(campaignNodeID);
                    CMS.DocumentEngine.TreeNode createNode = tree.SelectSingleNode(SiteContext.CurrentSiteName, document.NodeAliasPath, CurrentSite.DefaultVisitorCulture);
                    if (document != null)
                    {
                        Program program = new Program();
                        program.DocumentName = txtProgramName.Text;
                        program.DocumentCulture = SiteContext.CurrentSite.DefaultVisitorCulture;
                        program.ProgramName = txtProgramName.Text;
                        program.ProgramDescription = txtProgramDescription.Text;
                        program.BrandID = ValidationHelper.GetInteger(ddlBrand.Value, 0);
                        program.CampaignID = ValidationHelper.GetInteger(ddlCampaign.Value, 0);
                        program.Insert(createNode, true);
                        Response.Redirect(ProgramListURL);
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
        Response.Redirect(ProgramListURL);
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
                        var targetCampaign = CampaignProvider.GetCampaigns().WhereEquals("CampaignID", campaignID).FirstOrDefault();
                        if (targetCampaign != null)
                        {
                            int targetCampaignNodeID = targetCampaign.NodeID;
                            var tagetDocument = TreeHelper.SelectSingleNode(targetCampaignNodeID);
                            CMS.DocumentEngine.TreeNode targetPage = tree.SelectSingleNode(SiteContext.CurrentSiteName, tagetDocument.NodeAliasPath, SiteContext.CurrentSite.DefaultVisitorCulture);
                            if ((program != null) && (targetPage != null))
                            {
                                DocumentHelper.MoveDocument(program, targetPage, tree, true);
                            }
                        }

                    }
                }
                Response.Redirect(ProgramListURL);
            }
        }
        catch (Exception ex)
        {
            EventLogProvider.LogInformation("CMSWebParts_Kadena_Programs_AddNewProgram", "btnUpdateProgram_Click", ex.Message);
        }
    }
}



