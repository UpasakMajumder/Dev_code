using System;
using System.Data;
using System.Collections;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Linq;

using CMS.PortalEngine.Web.UI;
using CMS.Helpers;
using CMS.Membership;
using CMS.DocumentEngine.Types.KDA;

public partial class CMSWebParts_Kadena_Campaign_Web_Form_CampaignWebFormActions : CMSAbstractWebPart
{
    #region "Properties"

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
            Campaign campaign = CampaignProvider.GetCampaigns().WhereEquals("NodeSiteID", CurrentSite.SiteID).WhereEquals("CampaignID", CampaignID).FirstObject;
            //bool initiated = campaign.GetBooleanValue("CampaignInitiate", false);
            //bool openCampaign = campaign.GetBooleanValue("OpenCampaign", false);
            //bool isGlobalAdminNotified = campaign.GetBooleanValue("GlobalAdminNotified", false);
            //bool allowUpdates = campaign.GetBooleanValue("AllowUpdates", false);
            //bool closeCampaign = campaign.GetBooleanValue("CloseCampaign", false);

            //if (CurrentUser.IsInRole("TWEGlobalAdmin", CurrentSiteName))
            //{
            //    if (initiated)
            //        lnkInitiate.Visible = false;
            //    if (!openCampaign && initiated)
            //    {
            //        lnkUpdateProducts.Visible = true;

            //    }
            //}
            //else if (CurrentUser.IsInRole("TWEGlobalAdmin", CurrentSiteName))
            //{

            //}
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

    protected void lnkEdit_Click(object sender, EventArgs e)
    {
        Response.Redirect("");
    }
    protected void lnkInitiate_Click(object sender, EventArgs e)
    {
        LinkButton btn = (LinkButton)sender;
        int campaignID = ValidationHelper.GetInteger(btn.CommandArgument,0);
        Campaign campaign = CampaignProvider.GetCampaigns().WhereEquals("NodeSiteID", CurrentSite.SiteID).WhereEquals("CampaignID", campaignID).FirstObject;
        if (campaign != null)
        {
            campaign.CampaignInitiate = true;
            campaign.Update();
        }

    }

    protected void lnkViewProducts_Click(object sender, EventArgs e)
    {
        Response.Redirect("");
    }

    protected void lnkUpdateProducts_Click(object sender, EventArgs e)
    {
        Response.Redirect("");
    }

    protected void lnkOpenCampaign_Click(object sender, EventArgs e)
    {
        LinkButton btn = (LinkButton)sender;
        int campaignID = ValidationHelper.GetInteger(btn.CommandArgument, 0);
        Campaign campaign = CampaignProvider.GetCampaigns().WhereEquals("NodeSiteID", CurrentSite.SiteID).WhereEquals("CampaignID", campaignID).FirstObject;
        if (campaign != null)
        {
            campaign.OpenCampaign = true;
            campaign.Update();
        }
    }

    protected void lnkCloseCampaign_Click(object sender, EventArgs e)
    {
        LinkButton btn = (LinkButton)sender;
        int campaignID = ValidationHelper.GetInteger(btn.CommandArgument, 0);
        Campaign campaign = CampaignProvider.GetCampaigns().WhereEquals("NodeSiteID", CurrentSite.SiteID).WhereEquals("CampaignID", campaignID).FirstObject;
        if (campaign != null)
        {
            campaign.CloseCampaign = true;
            campaign.Update();
        }
    }
}



