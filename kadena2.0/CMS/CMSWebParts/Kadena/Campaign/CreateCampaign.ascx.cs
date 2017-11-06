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

public partial class CMSWebParts_Campaign_CreateCampaign : CMSAbstractWebPart
{
    #region "Variables"

    private string mDefaultTargetUrl = "";
    private string folderpath="/";
    #endregion
    #region "Properties"

    public string DefaultTargetUrl
    {
        get
        {
            return ValidationHelper.GetString(GetValue("DefaultTargetUrl"), mDefaultTargetUrl);
        }
        set
        {
            SetValue("DefaultTargetUrl", value);
            mDefaultTargetUrl = value;
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
            //assigning resource string to watermark text
            Name.WatermarkText = ResHelper.GetString("Kadena.CampaignForm.txtNameWatermark");
            Description.WatermarkText = ResHelper.GetString("Kadena.CampaignForm.txtDesWatermark");
            rfvUserNameRequired.ErrorMessage = ResHelper.GetString("Kadena.CampaignForm.NameRequired");
            rvDescription.ErrorMessage = ResHelper.GetString("Kadena.CampaignForm.DesMaxLength");
            rvName.ErrorMessage = ResHelper.GetString("Kadena.CampaignForm.NameMaxLength");
            //setting for folder path
            folderpath = SettingsKeyInfoProvider.GetValue("KDA_FolderPath");
        }
    }

    protected void btnSave_Save(object sender, EventArgs e)
    {
        string campaignName = Name.Text;
        string campaignDes = Description.Text;
        try
        {
            if (!string.IsNullOrEmpty(campaignName))
            {
                TreeProvider tree = new TreeProvider(MembershipContext.AuthenticatedUser);
                CMS.DocumentEngine.TreeNode parentPage = tree.SelectNodes().Path(folderpath).OnCurrentSite().Culture("en-us").FirstObject;
                if (parentPage != null)
                {
                    // Creates a new page of the "CMS.MenuItem" page type
                    CMS.DocumentEngine.TreeNode newPage = CMS.DocumentEngine.TreeNode.New("KDA.Campaign", tree);

                    // Sets the properties of the new page
                    newPage.DocumentName = campaignName;
                    newPage.DocumentCulture = "en-us";
                    newPage.SetValue("Name", campaignName);
                    newPage.SetValue("Description", campaignDes);

                    // Inserts the new page as a child of the parent page
                    newPage.Insert(parentPage);
                    lblSuccessMsg.Visible = true;
                    lblFailureText.Visible = false;
                    Name.Text = "";
                    Description.Text = "";
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
    protected void btnCancel_Cancel(object sender, EventArgs e)
    {
        try
        {
            var redirectUrl = RequestContext.CurrentURL;

            if (!String.IsNullOrEmpty(DefaultTargetUrl))
            {
                redirectUrl = ResolveUrl(DefaultTargetUrl);
            }

            URLHelper.Redirect(redirectUrl);
        }
        catch (Exception ex)
        {
            EventLogProvider.LogException("CampaignCreateFormCancel", "EXCEPTION", ex);
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
}



