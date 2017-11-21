using System;
using System.Data;
using System.Collections;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using CMS.PortalEngine.Web.UI;
using CMS.Helpers;
using CMS.DocumentEngine.Types.KDA;
using CMS.DataEngine;
using CMS.SiteProvider;
using CMS.EventLog;

public partial class CMSWebParts_Kadena_Campaign_Web_Form_ProductEditButton : CMSAbstractWebPart
{
    #region "Properties"
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

        }
    }


    /// <summary>
    /// Reloads the control data.
    /// </summary>
    public override void ReloadData()
    {
        base.ReloadData();

        SetupControl();
        BindEditButton();
    }
    /// <summary>
    /// Bind the edit button based on roles.
    /// </summary>
    public void BindEditButton()
    {
        try
        {
            //Campaign campaign = CampaignProvider.GetCampaigns().WhereEquals("NodeSiteID", CurrentSite.SiteID).WhereEquals("CampaignID",).FirstObject;
            Campaign campaign = CampaignProvider.GetCampaign(CurrentDocument.NodeGUID, CurrentSite.DefaultVisitorCulture, CurrentSiteName);
            if (campaign != null)
            {
                bool initiated = campaign.GetBooleanValue("CampaignInitiate", false);
                bool openCampaign = campaign.GetBooleanValue("OpenCampaign", false);
                bool isGlobalAdminNotified = campaign.GetBooleanValue("GlobalAdminNotified", false);
                bool allowUpdates = campaign.GetBooleanValue("AllowUpdates", false);
                bool closeCampaign = campaign.GetBooleanValue("CloseCampaign", false);

                string gAdminRoleName = SettingsKeyInfoProvider.GetValue(CurrentSite.SiteName + ".KDA_GlobalAminRoleName");
                string adminRoleName = SettingsKeyInfoProvider.GetValue(CurrentSite.SiteName + ".KDA_AdminRoleName");
                lnkEdit.Text= ValidationHelper.GetString(ResHelper.GetString("Kadena.CampaignProduct.EditProductLink"), string.Empty);

                if (CurrentUser.IsInRole(gAdminRoleName, CurrentSiteName))
                {
                    if (!openCampaign && !closeCampaign)
                    {
                        lnkEdit.Visible = true;
                        lnkEdit.Enabled = true;
                    }
                    else if (openCampaign || closeCampaign)
                    {
                        lnkEdit.Visible = true;
                        lnkEdit.Enabled = false;
                        lnkEdit.CssClass = "disable";
                    }
                }
                if (CurrentUser.IsInRole(adminRoleName, CurrentSiteName))
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
            EventLogProvider.LogInformation("CMSWebParts_Kadena_Campaign_Web_Form_CampaignProductsFilter", "BindEditButton", ex.Message);
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
                string url = SettingsKeyInfoProvider.GetValue(SiteContext.CurrentSiteName + ".KDA_ProductsPath");
                int campainNodeID = CurrentDocument.NodeID;
                Response.Redirect(url + "?camp=" + campainNodeID + "&id=" + productID);
            }
        }
        catch (Exception ex)
        {
            EventLogProvider.LogInformation("CMSWebParts_Kadena_Campaign_Web_Form_CampaignProductsFilter", "lnkEdit_Click", ex.Message);
        }
    }
}

#endregion

