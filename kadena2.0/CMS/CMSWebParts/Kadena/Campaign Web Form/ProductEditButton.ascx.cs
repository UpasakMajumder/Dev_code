using CMS.DataEngine;
using CMS.DocumentEngine;
using CMS.DocumentEngine.Types.KDA;
using CMS.EventLog;
using CMS.Helpers;
using CMS.PortalEngine.Web.UI;
using CMS.SiteProvider;
using Kadena.Container.Default;
using Kadena.Models.Program;
using Kadena.WebAPI.KenticoProviders.Contracts;
using System;
using System.Web.UI.WebControls;

public partial class CMSWebParts_Kadena_Campaign_Web_Form_ProductEditButton : CMSAbstractWebPart
{
    #region "Properties"

    /// <summary>
    /// Get the ProductId from the web part
    /// </summary>
    public int ProductID
    {
        get
        {
            return ValidationHelper.GetInteger(GetValue("ProductID"), 0);
        }
        set
        {
            SetValue("ProductID", value);
        }
    }

    public int ProgramID
    {
        get
        {
            return ValidationHelper.GetInteger(GetValue("ProgramID"), 0);
        }
        set
        {
            SetValue("ProgramID", value);
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
        BindEditButton();
    }

    /// <summary>
    /// Bind the edit button based on roles.
    /// </summary>
    public void BindEditButton()
    {
        try
        {
            Campaign campaign = CampaignProvider.GetCampaign(CurrentDocument.NodeGUID, CurrentDocument.DocumentCulture, CurrentSiteName);
            if (campaign != null)
            {
                CampaignProgram program = DIContainer.Resolve<IKenticoProgramsProvider>().GetProgram(ProgramID);
                if (program == null)
                {
                    return;
                }
                bool initiated = campaign.GetBooleanValue("CampaignInitiate", false);
                bool openCampaign = campaign.GetBooleanValue("OpenCampaign", false);
                bool isGlobalAdminNotified = program.GlobalAdminNotified;
                bool closeCampaign = campaign.GetBooleanValue("CloseCampaign", false);

                string globalAdminRoleName = SettingsKeyInfoProvider.GetValue(CurrentSite.SiteName + ".KDA_GlobalAminRoleName");
                string adminRoleName = SettingsKeyInfoProvider.GetValue(CurrentSite.SiteName + ".KDA_AdminRoleName");

                lnkEdit.Text = ValidationHelper.GetString(ResHelper.GetString("Kadena.CampaignProduct.EditProductLink"), string.Empty);
                if (CurrentUser.IsInRole(globalAdminRoleName, CurrentSiteName))
                {
                    if (!openCampaign && !closeCampaign)
                    {
                        lnkEdit.Visible = true;
                        lnkEdit.Enabled = true;
                        BindEditURL();
                    }
                    else if (openCampaign && !closeCampaign)
                    {
                        lnkEdit.Visible = true;
                        lnkEdit.Enabled = true;
                        BindEditURL();
                    }
                    else
                    {
                        lnkEdit.Visible = true;
                        lnkEdit.Enabled = false;
                        lnkEdit.CssClass = "disable";
                    }
                }
                else if (CurrentUser.IsInRole(adminRoleName, CurrentSiteName))
                {
                    if (!isGlobalAdminNotified && !openCampaign && !closeCampaign)
                    {
                        lnkEdit.Visible = true;
                        lnkEdit.Enabled = true;
                        BindEditURL();
                    }
                    else if (isGlobalAdminNotified || openCampaign || !closeCampaign)
                    {
                        lnkEdit.Visible = true;
                        lnkEdit.Enabled = false;
                        lnkEdit.CssClass = "disable";
                    }
                }
            }
        }
        catch (Exception ex)
        {
            EventLogProvider.LogException("CMSWebParts_Kadena_Campaign_Web_Form_ProductEditButton", "BindEditButton", ex, CurrentSite.SiteID, ex.Message);
        }
    }

    private void BindEditURL()
    {
        if (ProductID != 0)
        {
            Guid nodeGUID = ValidationHelper.GetGuid(SettingsKeyInfoProvider.GetValue(SiteContext.CurrentSiteName + ".KDA_ProductsPath"), Guid.Empty);
            {
                if (!nodeGUID.Equals(Guid.Empty))
                {
                    var document = new TreeProvider().SelectSingleNode(nodeGUID, CurrentDocument.DocumentCulture, CurrentSite.SiteName);
                    if (document != null)
                    {
                        lnkEdit.Attributes.Add("href", $"{document.DocumentUrlPath}?camp={ CurrentDocument.NodeID }&id={ ProductID}&category={Request.QueryString["category"]}&program={Request.QueryString["program"]}&searchProducts={Request.QueryString["searchProducts"]}");
                    }
                }
            }
        }
    }

}

#endregion "Methods"