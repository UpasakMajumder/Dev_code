using CMS.DocumentEngine;
using CMS.Ecommerce;
using CMS.EventLog;
using CMS.Helpers;
using CMS.Localization;
using CMS.Membership;
using CMS.PortalEngine.Web.UI;
using CMS.SiteProvider;
using Kadena.Models;
using Kadena.Models.Product;
using Kadena.Container.Default;
using Kadena2.MicroserviceClients.Contracts;
using System;

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
                var orderStatisticsData = GetOrderStatistics();

                if (orderStatisticsData != null)
                {
                    ltlOrdersToDatePerYear.Text = orderStatisticsData.numberOfOrders.ToString();
                    ltlAverageProductionTime.Text = orderStatisticsData.productionAvgTime.ToString("N2");
                }
                else
                {
                    ltlOrdersToDatePerYearTitles.Visible = ltlAverageProductionTimeTitle.Visible = ltlAverageProductionTimeDaysTitle.Visible = false;
                }
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

            foreach (TreeNode page in pages)
            {
                if (!page.IsLink)
                {
                    var inventoryProduct = SKUInfoProvider.GetSKUInfo(page.NodeSKUID);
                    if (inventoryProduct == null)
                    {
                        // skip invalid SKU id
                        continue;
                    }

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

        private OrderStatisticsData GetOrderStatistics()
        {
            return CacheHelper.Cache(cs => GetOrderStatisticsInternal(), new CacheSettings(20, "Kadena.DashboardMetrics.OrderStatistics_" + SiteContext.CurrentSiteName));
        }

        private OrderStatisticsData GetOrderStatisticsInternal()
        {
            var statisticClient = DIContainer.Resolve<IStatisticsClient>();
            var requestResult = statisticClient.GetOrderStatistics().Result;

            if (!requestResult.Success)
            {
                EventLogProvider.LogException("DASHBOARD", "GET ORDER STATISTICS", new InvalidOperationException(requestResult.ErrorMessages));
                return null;
            }

            return new OrderStatisticsData
            {
                numberOfOrders = requestResult.Payload?.NumberOfOrders ?? 0,
                productionAvgTime = requestResult.Payload?.ProductionAvgTime ?? 0
            };
        }

        #endregion
    }
}