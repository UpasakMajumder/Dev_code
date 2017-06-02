using CMS.DocumentEngine;
using CMS.Ecommerce;
using CMS.Helpers;
using CMS.Localization;
using CMS.Membership;
using CMS.PortalEngine.Web.UI;
using CMS.SiteProvider;

namespace Kadena.CMSWebParts.Kadena.Metrics
{
  public partial class DashboardMetrics : CMSAbstractWebPart
  {
    #region Public methods

    public override void OnContentLoaded()
    {
      base.OnContentLoaded();
      SetupControl();
    }

    protected void SetupControl()
    {
      if (!StopProcessing && !IsPostBack)
      {
        // TODO: need to be implemented!
        ltlOrdersToDatePerYear.Text = "150";
        // TODO: need to be implemented!
        ltlAverageProductionTime.Text = "2 days";
        ltlTotalProductsAvailable.Text = GetNumberOfTotalProductsAvailable().ToString();
        ltlNumberOfusers.Text = GetNumberOfUsers().ToString();
      }
    }

    #endregion

    #region Private methods

    private int GetNumberOfTotalProductsAvailable()
    {
      return CacheHelper.Cache(cs => GetNumberOfTotalProductsAvailableInternal(), new CacheSettings(20, "Kadena.DashboardMetrics.TotalProductsAvailable_" + SiteContext.CurrentSiteName + "_" + LocalizationContext.PreferredCultureCode));
    }

    private int GetNumberOfTotalProductsAvailableInternal()
    {
      var result = 0;

      var tree = new TreeProvider(MembershipContext.AuthenticatedUser);

      var pages = tree.SelectNodes()
          .Types("KDA.Product")
          .OnCurrentSite()
          .Culture(LocalizationContext.PreferredCultureCode);

      foreach (CMS.DocumentEngine.TreeNode page in pages)
      {
        if (!page.IsLink)
        {
          if (page.GetStringValue("ProductType", string.Empty).Contains("KDA.InventoryProduct"))
          {
            var inventoryProduct = SKUInfoProvider.GetSKUInfo(page.NodeSKUID);
            if (inventoryProduct.SKUTrackInventory == TrackInventoryTypeEnum.Disabled)
            {
              result++;
            }
            else
            {
              if (inventoryProduct.SKUAvailableItems > 0)
              {
                result++;
              }
            }
          }
          else
          {
            result++;
          }
        }
      }
      return result;
    }

    private int GetNumberOfUsers()
    {
      return CacheHelper.Cache(cs => GetNumberOfUsersInternal(), new CacheSettings(20, "Kadena.DashboardMetrics.NumberOfUsers_" + SiteContext.CurrentSiteName));
    }

    private int GetNumberOfUsersInternal()
    {
      var result = 0;

      var users = UserInfoProvider.GetUsers();

      foreach (var user in users)
      {
        if (user.IsInSite(SiteContext.CurrentSiteName) && user.SiteIndependentPrivilegeLevel == CMS.Base.UserPrivilegeLevelEnum.None && user.UserName != "public")
        {
          result++;
        }
      }
      return result;
    }

    #endregion
  }
}