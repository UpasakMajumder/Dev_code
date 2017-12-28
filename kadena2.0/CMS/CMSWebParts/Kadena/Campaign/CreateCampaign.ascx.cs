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
                CMS.DocumentEngine.TreeNode parentPage = tree.SelectNodes().Path(folderpath).OnCurrentSite().Culture(DocumentContext.CurrentDocument.DocumentCulture).FirstObject;
                if (parentPage != null)
                {
                    Campaign newCampaign = new Campaign()
                    {
                        Name = campaignName,
                        DocumentName = campaignName,
                        Description = campaignDes,
                        DocumentCulture = CurrentDocument.DocumentCulture,
                        DocumentPageTemplateID = DocumentPageTemplate()
                    };
                    newCampaign.Insert(parentPage);
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
                    editPage.DocumentName = campaignName;
                    editPage.DocumentCulture = DocumentContext.CurrentDocument.DocumentCulture;
                    editPage.SetValue("Name", campaignName);
                    editPage.SetValue("Description", campaignDes);
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
    /// <summary>
    /// Set the Data 
    /// </summary>
    /// <param name="_campaignId"></param>
    private void SetFeild(int _campaignId)
    {
        try
        {
            TreeProvider tree = new TreeProvider(MembershipContext.AuthenticatedUser);
            CMS.DocumentEngine.TreeNode editPage = tree.SelectNodes("KDA.Campaign").OnCurrentSite().Where("CampaignID", QueryOperator.Equals, _campaignId);
            if (editPage != null)
            {
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
    /// <summary>
    /// Get the Campaign Products Tempalte ID
    /// </summary>
    /// <returns>TemplateID</returns>
    public static int DocumentPageTemplate()
    {
        try
        {
            string templteName = SettingsKeyInfoProvider.GetValue(SiteContext.CurrentSite.SiteName + ".KDA_CampaignProductsTemplateName");
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

    #endregion
}



