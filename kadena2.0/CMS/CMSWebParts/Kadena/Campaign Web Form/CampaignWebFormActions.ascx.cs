using CMS.DataEngine;
using CMS.DocumentEngine;
using CMS.DocumentEngine.Types.KDA;
using CMS.EventLog;
using CMS.Helpers;
using CMS.Membership;
using CMS.PortalEngine.Web.UI;
using CMS.SiteProvider;
using Kadena.Old_App_Code.Kadena.EmailNotifications;
using System;
using System.Web.UI.WebControls;

public partial class CMSWebParts_Kadena_Campaign_Web_Form_CampaignWebFormActions : CMSAbstractWebPart
{
    #region "Properties"

    /// <summary>
    /// CampaignID
    /// </summary>
    public int CampaignID
    {
        get
        {
            return ValidationHelper.GetInteger(GetValue("CampaignID"), 0);
        }
        set
        {
            SetValue("CampaignID", value);
        }
    }

    /// <summary>
    /// Edit campaign link resource string text
    /// </summary>
    public string EditCampaignLinkText
    {
        get
        {
            return ValidationHelper.GetString(ResHelper.GetString("Kadena.CampaignProduct.EditCampaignLinkText"), string.Empty);
        }
        set
        {
            SetValue("EditCampaignLinkText", value);
        }
    }

    /// <summary>
    /// View products link resource string text
    /// </summary>
    public string ViewProductsLinkText
    {
        get
        {
            return ValidationHelper.GetString(ResHelper.GetString("Kadena.CampaignProduct.ViewProductsLinkText"), string.Empty);
        }
        set
        {
            SetValue("ViewProductsLinkText", value);
        }
    }

    /// <summary>
    /// Update products link resource string text
    /// </summary>
    public string UpdateProductsLinkText
    {
        get
        {
            return ValidationHelper.GetString(ResHelper.GetString("Kadena.CampaignProduct.UpdateProductsLinkText"), string.Empty);
        }
        set
        {
            SetValue("UpdateProductsLinkText", value);
        }
    }

    /// <summary>
    /// Open Campaign link resource string text
    /// </summary>
    public string OpenCampaignLinkText
    {
        get
        {
            return ValidationHelper.GetString(ResHelper.GetString("Kadena.CampaignProduct.OpenCampaignLinkText"), string.Empty);
        }
        set
        {
            SetValue("OpenCampaignLinkText", value);
        }
    }

    /// <summary>
    /// Close campaign link campaign text
    /// </summary>
    public string CloseCampaignLinkText
    {
        get
        {
            return ValidationHelper.GetString(ResHelper.GetString("Kadena.CampaignProduct.CloseCampaignLinkText"), string.Empty);
        }
        set
        {
            SetValue("CloseCampaignLinkText", value);
        }
    }

    /// <summary>
    /// Initiate campaign link resource string text
    /// </summary>
    public string InitiateCampaignLinkText
    {
        get
        {
            return ValidationHelper.GetString(ResHelper.GetString("Kadena.CampaignProduct.InitiateCampaignLinkText"), string.Empty);
        }
        set
        {
            SetValue("InitiateCampaignLinkText", value);
        }
    }
    /// <summary>
    /// Edit Campaign Tooltip resource string
    /// </summary>
    public string EditCampaignToolTipText
    {
        get
        {
            return ResHelper.GetString("Kadena.CampaignProduct.EditCampaignToolTipText");
        }
        set
        {
            SetValue("EditCampaignToolTipText", value);
        }
    }
    /// <summary>
    /// Initiate Campaign Tooltip resource string
    /// </summary>
    public string InitiateCampaignToolTipText
    {
        get
        {
            return ResHelper.GetString("Kadena.CampaignProduct.InitiateCampaignToolTipText");
        }
        set
        {
            SetValue("InitiateCampaignToolTipText", value);
        }
    }
    /// <summary>
    /// View Products Tooltip resource string
    /// </summary>
    public string ViewProductsToolTipText
    {
        get
        {
            return ResHelper.GetString("Kadena.CampaignProduct.ViewProductsToolTipText");
        }
        set
        {
            SetValue("ViewProductsToolTipText", value);
        }
    }
    /// <summary>
    /// Update products Tooltip resource string
    /// </summary>
    public string UpdateProductsToolTipText
    {
        get
        {
            return ResHelper.GetString("Kadena.CampaignProduct.UpdateProductsToolTipText");
        }
        set
        {
            SetValue("UpdateProductsToolTipText", value);
        }
    }
    /// <summary>
    /// Open Campaign Tooltip resource string
    /// </summary>
    public string OpenCampaignToolTipText
    {
        get
        {
            return ResHelper.GetString("Kadena.CampaignProduct.OpenCampaignToolTipText");
        }
        set
        {
            SetValue("OpenCampaignToolTipText", value);
        }
    }
    /// <summary>
    /// Close Campaign Tooltip resource string
    /// </summary>
    public string CloseCampaignToolTipText
    {
        get
        {
            return ResHelper.GetString("Kadena.CampaignProduct.CloseCampaignToolTipText");
        }
        set
        {
            SetValue("CloseCampaignToolTipText", value);
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
    }

    /// <summary>
    /// Reloads the control data.
    /// </summary>
    public override void ReloadData()
    {
        base.ReloadData();
        BindActions();
        BindResourceStrings();
    }

    /// <summary>
    /// Bind resource strings for links
    /// </summary>
    public void BindResourceStrings()
    {
        lnkEdit.Text = EditCampaignLinkText;
        lnkInitiate.Text = InitiateCampaignLinkText;
        lnkUpdateProducts.Text = UpdateProductsLinkText;
        lnkViewProducts.Text = ViewProductsLinkText;
        lnkOpenCampaign.Text = OpenCampaignLinkText;
        lnkCloseCampaign.Text = CloseCampaignLinkText;
    }

    /// <summary>
    /// Binding the links based on admin roles
    /// </summary>
    public void BindActions()
    {
        try
        {
            Campaign campaign = CampaignProvider.GetCampaigns()
                .WhereEquals("NodeSiteID", CurrentSite.SiteID)
                .WhereEquals("CampaignID", CampaignID)
                .FirstObject;
            if (campaign != null)
            {
                bool initiated = campaign.GetBooleanValue("CampaignInitiate", false);
                bool openCampaign = campaign.GetBooleanValue("OpenCampaign", false);
                bool isGlobalAdminNotified = campaign.GetBooleanValue("GlobalAdminNotified", false);
                bool closeCampaign = campaign.GetBooleanValue("CloseCampaign", false);
                string gAdminRoleName = SettingsKeyInfoProvider.GetValue(CurrentSite.SiteName + ".KDA_GlobalAminRoleName");
                string adminRoleName = SettingsKeyInfoProvider.GetValue(CurrentSite.SiteName + ".KDA_AdminRoleName");

                if (CurrentUser.IsInRole(gAdminRoleName, CurrentSiteName))
                {
                    if (!initiated)
                    {
                        lnkEdit.Visible = true;
                        lnkEdit.Enabled = true;
                        lnkInitiate.Enabled = true;
                        lnkInitiate.Visible = true;
                        lnkOpenCampaign.Enabled = false;
                        lnkOpenCampaign.Visible = true;
                        lnkOpenCampaign.CssClass = "disable";
                    }
                    else if (initiated && !isGlobalAdminNotified && !openCampaign && !closeCampaign)
                    {
                        lnkEdit.Enabled = true;
                        lnkEdit.Visible = true;
                        lnkUpdateProducts.Enabled = true;
                        lnkUpdateProducts.Visible = true;
                        lnkOpenCampaign.Enabled = false;
                        lnkOpenCampaign.Visible = true;
                        lnkOpenCampaign.CssClass = "disable";
                    }
                    else if (initiated && isGlobalAdminNotified && !openCampaign && !closeCampaign)
                    {
                        lnkEdit.Visible = true;
                        lnkEdit.Enabled = false;
                        lnkUpdateProducts.Visible = true;
                        lnkUpdateProducts.Enabled = true;
                        lnkOpenCampaign.Visible = true;
                        lnkOpenCampaign.Enabled = true;
                    }
                    if (openCampaign)
                    {
                        lnkEdit.Visible = true;
                        lnkEdit.Enabled = true;
                        lnkViewProducts.Visible = true;
                        lnkViewProducts.Enabled = true;
                        lnkCloseCampaign.Enabled = true;
                        lnkCloseCampaign.Visible = true;
                    }
                    if (closeCampaign)
                    {
                        lnkEdit.Visible = true;
                        lnkEdit.Enabled = true;
                        lnkViewProducts.Visible = true;
                        lnkViewProducts.Enabled = true;
                        lnkCloseCampaign.Visible = true;
                        lnkCloseCampaign.Enabled = false;
                        lnkCloseCampaign.CssClass = "disable";
                    }
                }
                else if (CurrentUser.IsInRole(adminRoleName, CurrentSiteName))
                {
                    if (!initiated)
                    {
                        lnkEdit.Visible = false;
                        lnkEdit.Enabled = true;
                    }
                    else if (initiated && !isGlobalAdminNotified && !openCampaign && !closeCampaign)
                    {
                        lnkEdit.Visible = false;
                        lnkEdit.Enabled = true;
                        lnkUpdateProducts.Visible = true;
                        lnkUpdateProducts.Enabled = true;
                    }
                    else if (initiated && isGlobalAdminNotified && !openCampaign && !closeCampaign)
                    {
                        lnkEdit.Visible = false;
                        lnkEdit.Enabled = false;
                        lnkViewProducts.Visible = true;
                        lnkViewProducts.Enabled = true;
                    }
                    else if (openCampaign || closeCampaign)
                    {
                        lnkViewProducts.Visible = true;
                        lnkViewProducts.Enabled = true;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            EventLogProvider.LogException("CMSWebParts_Kadena_Campaign_Web_Form_CampaignWebFormActions", "BindActions", ex, CurrentSite.SiteID, ex.Message);
        }
    }

    /// <summary>
    /// showing the campain list in edit view
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lnkEdit_Click(object sender, EventArgs e)
    {
        try
        {
            LinkButton btn = (LinkButton)sender;
            int campaignID = ValidationHelper.GetInteger(btn.CommandArgument, 0);
            Guid nodeGUID = ValidationHelper.GetGuid(SettingsKeyInfoProvider.GetValue(SiteContext.CurrentSiteName + ".KDA_CampaignCreatePageUrl"), Guid.Empty);
            {
                if (!nodeGUID.Equals(Guid.Empty))
                {
                    var document = new TreeProvider().SelectSingleNode(nodeGUID, CurrentDocument.DocumentCulture, CurrentSite.SiteName);
                    if (document != null)
                    {
                        Response.Redirect($"{document.DocumentUrlPath}?ID={campaignID}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            EventLogProvider.LogException("CMSWebParts_Kadena_Campaign_Web_Form_CampaignWebFormActions", "lnkEdit_Click", ex, CurrentSite.SiteID, ex.Message);
        }
    }

    /// <summary>
    /// initiate the Campaign
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lnkInitiate_Click(object sender, EventArgs e)
    {
        try
        {
            LinkButton btn = (LinkButton)sender;
            int campaignID = ValidationHelper.GetInteger(btn.CommandArgument, 0);
            Campaign campaign = CampaignProvider.GetCampaigns()
                .WhereEquals("NodeSiteID", CurrentSite.SiteID)
                .WhereEquals("CampaignID", campaignID)
                .FirstObject;
            if (campaign != null)
            {
                campaign.CampaignInitiate = true;
                campaign.Update();
                Response.Redirect(CurrentDocument.DocumentUrlPath);
            }
        }
        catch (Exception ex)
        {
            EventLogProvider.LogException("CMSWebParts_Kadena_Campaign_Web_Form_CampaignWebFormActions", "lnkInitiate_Click", ex, CurrentSite.SiteID, ex.Message);
        }
    }

    /// <summary>
    /// view the current campaign products
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lnkViewProducts_Click(object sender, EventArgs e)
    {
        try
        {
            LinkButton btn = (LinkButton)sender;
            int campaignID = ValidationHelper.GetInteger(btn.CommandArgument, 0);
            Campaign campaign = CampaignProvider.GetCampaigns()
                .WhereEquals("NodeSiteID", CurrentSite.SiteID)
                .WhereEquals("CampaignID", campaignID)
                .FirstObject;
            Response.Redirect(campaign.DocumentUrlPath);
        }
        catch (Exception ex)
        {
            EventLogProvider.LogException("CMSWebParts_Kadena_Campaign_Web_Form_CampaignWebFormActions", "lnkViewProducts_Click", ex, CurrentSite.SiteID, ex.Message);
        }
    }

    /// <summary>
    /// Redirect to products list
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lnkUpdateProducts_Click(object sender, EventArgs e)
    {
        try
        {
            LinkButton btn = (LinkButton)sender;
            int campaignID = ValidationHelper.GetInteger(btn.CommandArgument, 0);
            Campaign campaign = CampaignProvider.GetCampaigns()
                .WhereEquals("NodeSiteID", CurrentSite.SiteID)
                .WhereEquals("CampaignID", campaignID)
                .FirstObject;
            Response.Redirect(campaign.DocumentUrlPath);
        }
        catch (Exception ex)
        {
            EventLogProvider.LogException("CMSWebParts_Kadena_Campaign_Web_Form_CampaignWebFormActions", "lnkUpdateProducts_Click", ex, CurrentSite.SiteID, ex.Message);
        }
    }

    /// <summary>
    /// Open the Campaign
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lnkOpenCampaign_Click(object sender, EventArgs e)
    {
        try
        {
            LinkButton btn = (LinkButton)sender;
            int campaignID = ValidationHelper.GetInteger(btn.CommandArgument, 0);
            Campaign campaign = CampaignProvider.GetCampaigns()
                .WhereEquals("NodeSiteID", CurrentSite.SiteID)
                .WhereEquals("CampaignID", campaignID)
                .FirstObject;
            if (campaign != null)
            {
                campaign.OpenCampaign = true;
                campaign.Update();

                var users = UserInfoProvider.GetUsers();
                if (users != null)
                {
                    foreach (var user in users)
                    {
                        ProductEmailNotifications.CampaignEmail(campaign.DocumentName, user.Email, "CampaignOpenEmail");
                    }
                }
                Response.Redirect(CurrentDocument.DocumentUrlPath);
            }
        }
        catch (Exception ex)
        {
            EventLogProvider.LogException("CMSWebParts_Kadena_Campaign_Web_Form_CampaignWebFormActions", "lnkOpenCampaign_Click", ex, CurrentSite.SiteID, ex.Message);
        }
    }

    /// <summary>
    /// Closing the campaign
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lnkCloseCampaign_Click(object sender, EventArgs e)
    {
        try
        {
            LinkButton btn = (LinkButton)sender;
            int campaignID = ValidationHelper.GetInteger(btn.CommandArgument, 0);
            Campaign campaign = CampaignProvider.GetCampaigns()
                .WhereEquals("NodeSiteID", CurrentSite.SiteID)
                .WhereEquals("CampaignID", campaignID)
                .FirstObject;
            if (campaign != null)
            {
                campaign.CloseCampaign = true;
                campaign.Update();
                var users = UserInfoProvider.GetUsers();
                if (users != null)
                {
                    foreach (var user in users)
                    {
                        ProductEmailNotifications.CampaignEmail(campaign.DocumentName, user.Email, "CampaignCloseEmail");
                    }
                }
                Response.Redirect(CurrentDocument.DocumentUrlPath);
            }
        }
        catch (Exception ex)
        {
            EventLogProvider.LogException("CMSWebParts_Kadena_Campaign_Web_Form_CampaignWebFormActions", "lnkCloseCampaign_Click", ex, CurrentSite.SiteID, ex.Message);
        }
    }

    #endregion "Methods"
}