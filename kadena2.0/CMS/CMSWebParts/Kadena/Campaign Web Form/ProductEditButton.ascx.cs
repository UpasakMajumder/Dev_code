using CMS.DataEngine;
using CMS.DocumentEngine;
using CMS.DocumentEngine.Types.KDA;
using CMS.EventLog;
using CMS.Helpers;
using CMS.PortalEngine.Web.UI;
using CMS.SiteProvider;
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
                bool initiated = campaign.GetBooleanValue("CampaignInitiate", false);
                bool openCampaign = campaign.GetBooleanValue("OpenCampaign", false);
                bool isGlobalAdminNotified = campaign.GetBooleanValue("GlobalAdminNotified", false);
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

    /// <summary>
    /// Edit Product
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lnkEdit_Click(object sender, EventArgs e)
    {
        try
        {
            LinkButton btn = (LinkButton)sender;
            int productID = ValidationHelper.GetInteger(btn.CommandArgument, 0);
            if (productID != 0)
            {
                Guid nodeGUID = ValidationHelper.GetGuid(SettingsKeyInfoProvider.GetValue(SiteContext.CurrentSiteName + ".KDA_ProductsPath"), Guid.Empty);
                {
                    if (!nodeGUID.Equals(Guid.Empty))
                    {
                        var document = new TreeProvider().SelectSingleNode(nodeGUID, CurrentDocument.DocumentCulture, CurrentSite.SiteName);
                        if (document != null)
                        {
                            Response.Redirect($"{document.DocumentUrlPath}?camp={ CurrentDocument.NodeID }&id={ productID}");
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            EventLogProvider.LogException("CMSWebParts_Kadena_Campaign_Web_Form_ProductEditButton", "lnkEdit_Click", ex, CurrentSite.SiteID, ex.Message);
        }
    }
}

#endregion "Methods"