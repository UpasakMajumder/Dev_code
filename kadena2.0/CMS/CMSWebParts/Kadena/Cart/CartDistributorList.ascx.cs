using CMS.PortalEngine.Web.UI;
using CMS.Helpers;
using Kadena.WebAPI.KenticoProviders.Contracts;
using Kadena.Container.Default;
using CMS.Ecommerce;
using System;
using CMS.EventLog;
using CMS.DocumentEngine.Types.KDA;
using CMS.DataEngine;
using System.Linq;

public partial class CMSWebParts_Kadena_Cart_CartDistributorList : CMSAbstractWebPart
{
    #region "Properties"

    public int InventoryType
    {
        get
        {
            return ValidationHelper.GetInteger(GetValue("InventoryType"), 1);
        }
        set
        {
            SetValue("InventoryType", value);
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
            hdnInventoryType.Value = InventoryType.ToString();
            hdnNoDistributorsError.Value = ResHelper.GetString("Kadena.AddToCart.DistributorError");
            hdnInsufficientStockError.Value = ResHelper.GetString("Kadena.AddToCart.StockError");
            hdnMoreThanAllocatedError.Value = ResHelper.GetString("Kadena.AddToCart.AllocatedProductQuantityError");
            hdnCartUpdatedText.Value = ResHelper.GetString("Kadena.AddToCart.SuccessfullyAdded");
            if (DIContainer.Resolve<IKenticoCustomerProvider>().GetCustomerIDByUserID(CurrentUser.UserID) == 0)
            {
                CreateCustomer();
            }
            BindDistributors();
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

    private void BindDistributors()
    {
        int campaignID = GetOpenCampaignID();
        rptCartDistributorList.DataSource = DIContainer.Resolve<IKenticoAddressBookProvider>().GetAddressesListByUserID(CurrentUser.UserID, InventoryType, campaignID);
        rptCartDistributorList.DataBind();
    }

    private int GetOpenCampaignID()
    {
        Campaign campaign = CampaignProvider.GetCampaigns().Columns("CampaignID")
                                .WhereEquals("OpenCampaign", true)
                                .Where(new WhereCondition().WhereEquals("CloseCampaign", false).Or()
                                .WhereEquals("CloseCampaign", null))
                                .WhereEquals("NodeSiteID", CurrentSite.SiteID).FirstOrDefault();
        return campaign?.CampaignID ?? 0;
    }

    /// <summary>
    /// Create cusotmer based on  logged in user details
    /// </summary>
    /// <returns>Customer id</returns>
    private void CreateCustomer()
    {
        try
        {
            CustomerInfo objCustomer = new CustomerInfo();
            objCustomer.CustomerUserID = CurrentUser.UserID;
            objCustomer.CustomerEmail = CurrentUser.Email;
            objCustomer.CustomerFirstName = CurrentUser.FirstName;
            objCustomer.CustomerLastName = CurrentUser.LastName;
            objCustomer.CustomerSiteID = CurrentSite.SiteID;
            CustomerInfoProvider.SetCustomerInfo(objCustomer);
        }
        catch (Exception ex)
        {
            EventLogProvider.LogException("CartDistributorList.ascx.cs", "CreateCustomer()", ex);
        }
    }

    #endregion
}