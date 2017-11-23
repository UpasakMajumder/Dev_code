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
        if (this.StopProcessing)
        {
            // Do not process

        }
        else
        {
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
            //assigning resource string to watermark text
            Name.Attributes.Add("PlaceHolder", ResHelper.GetString("Kadena.CampaignForm.txtNameWatermark"));
            Description.Attributes.Add("PlaceHolder", ResHelper.GetString("Kadena.CampaignForm.txtDesWatermark"));
            rfvUserNameRequired.ErrorMessage = ResHelper.GetString("Kadena.CampaignForm.NameRequired");
            rvDescription.ErrorMessage = ResHelper.GetString("Kadena.CampaignForm.DesMaxLength");
            rvName.ErrorMessage = ResHelper.GetString("Kadena.CampaignForm.NameMaxLength");
            //setting for folder path
            folderpath = SettingsKeyInfoProvider.GetValue("KDA_CampaignFolderPath", CurrentSiteName);
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
            URLHelper.Redirect(CurrentDocument.Parent.DocumentUrlPath);
    }
    protected void btnSave_Edit(object sender, EventArgs e)
    {
        string campaignName = Name.Text;
        string campaignDes = Description.Text;
        try
        {
            if (!string.IsNullOrEmpty(campaignName))
            {
                TreeProvider tree = new TreeProvider(MembershipContext.AuthenticatedUser);
                CMS.DocumentEngine.TreeNode editPage = tree.SelectNodes("KDA.Campaign").OnCurrentSite().Where("CampaignID", QueryOperator.Equals, campaignId);
                if (editPage != null)
                {
                    // Sets the properties of the new page
                    editPage.DocumentName = campaignName;
                    editPage.DocumentCulture = "en-us";
                    editPage.SetValue("Name", campaignName);
                    editPage.SetValue("Description", campaignDes);

                    // update the  campaign
                    editPage.Update();

                    URLHelper.Redirect(CurrentDocument.Parent.DocumentUrlPath);
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

    ///
    /// 
    private void SetFeild(int _campaignId)
    {
        try
        {
            TreeProvider tree = new TreeProvider(MembershipContext.AuthenticatedUser);
            CMS.DocumentEngine.TreeNode editPage = tree.SelectNodes("KDA.Campaign").OnCurrentSite().Where("CampaignID", QueryOperator.Equals, _campaignId);
            if (editPage != null)
            {
                // get the properties of the page

                Name.Text = editPage.GetValue("Name").ToString();
                Description.Text = editPage.GetValue("Description").ToString();

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

    #endregion
}



