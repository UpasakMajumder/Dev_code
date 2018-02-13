using System;
using System.Data;
using System.Collections;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using CMS.PortalEngine.Web.UI;
using CMS.Helpers;
using CMS.SiteProvider;
using CMS.Ecommerce;
using CMS.EventLog;

public partial class CMSWebParts_Kadena_Product_SwapSKUNumberToCRN : CMSAbstractWebPart
{
    #region "Properties"

    

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
            lblSiteName.Text = SiteContext.CurrentSiteName;
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


    protected void btnGetRecords_Click(object sender, EventArgs e)
    {
        try
        {

            var products = SKUInfoProvider.GetSKUs()
                .WhereEquals("SKUSiteID", SiteContext.CurrentSiteID);
            lblTotalCount.Text = products.Count.ToString();
            EventLogProvider.LogEvent(EventType.INFORMATION, "SKU Update", "GETCOUNT", eventDescription: "Get SKU products");
        }
        catch (Exception ex)
        {
            EventLogProvider.LogException("ProductsSKUUpdate", "GetRecords", ex, CurrentSite.SiteID, ex.Message);
        }

    }

    protected void btnUpdateCRN_Click(object sender, EventArgs e)
    {
        try
        {
            int rCount = 0;
            var products = SKUInfoProvider.GetSKUs()
               .WhereEquals("SKUSiteID", SiteContext.CurrentSiteID);
            foreach (SKUInfo modifyProduct in products)
            {
                if (String.IsNullOrEmpty(modifyProduct.GetValue("SKUProductCustomerReferenceNumber",string.Empty)))
                {
                    modifyProduct.SetValue("SKUProductCustomerReferenceNumber", modifyProduct.SKUNumber);
                    SKUInfoProvider.SetSKUInfo(modifyProduct);
                    rCount++;
                }
            }
            EventLogProvider.LogEvent(EventType.INFORMATION, "SKU Update", "UPDATECRN", eventDescription: "Update CRN with SKU");
            lblUpdatedCRN.Text = string.Format("{0} Records moved to CRN", rCount);
        }
        catch (Exception ex)
        {
            EventLogProvider.LogException("ProductsSKUUpdate", "UpdateCRNRecords", ex, CurrentSite.SiteID, ex.Message);
        }
    }

    protected void btnUpdateSKU_Click(object sender, EventArgs e)
    {
        try
        {
            int rCount = 0;
            var products = SKUInfoProvider.GetSKUs()
               .WhereEquals("SKUSiteID", SiteContext.CurrentSiteID);
            foreach (SKUInfo modifyProduct in products)
            {
                modifyProduct.SKUNumber = "00000";
                SKUInfoProvider.SetSKUInfo(modifyProduct);
                rCount++;
            }
            lblSKUUpdate.Text = string.Format("{0} Records updated in SKUNumber field", rCount);
            EventLogProvider.LogEvent(EventType.INFORMATION, "SKU Update", "UPDATESKU", eventDescription: "Update SKU with 00000");
        }
        catch (Exception ex)
        {
            EventLogProvider.LogException("ProductsSKUUpdate", "UpdateSKURecords", ex, CurrentSite.SiteID, ex.Message);
        }
    }

    protected void btnRevert_Click(object sender, EventArgs e)
    {
        try
        {
            int rCount = 0;
            var products = SKUInfoProvider.GetSKUs()
               .WhereEquals("SKUSiteID", SiteContext.CurrentSiteID);
            foreach (SKUInfo modifyProduct in products)
            {
                modifyProduct.SKUNumber = modifyProduct.GetValue("SKUProductCustomerReferenceNumber", string.Empty);
                SKUInfoProvider.SetSKUInfo(modifyProduct);
                rCount++;
            }
            EventLogProvider.LogEvent(EventType.INFORMATION, "SKU Update", "REVERTSKU", eventDescription: "Update back SKU with CRN");
            lblRevert.Text = string.Format("{0} Records reverted to SKUNumber field", rCount);
        }
        catch (Exception ex)
        {
            EventLogProvider.LogException("ProductsSKUUpdate", "Reveret UpdateSKURecords", ex, CurrentSite.SiteID, ex.Message);
        }
    }
}



