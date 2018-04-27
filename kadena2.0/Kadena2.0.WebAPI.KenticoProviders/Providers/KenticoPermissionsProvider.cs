using AutoMapper;
using CMS.Membership;
using CMS.SiteProvider;
using Kadena.Models.Membership;
using Kadena2.WebAPI.KenticoProviders.Contracts;
using System;
using System.Collections.Generic;

namespace Kadena2.WebAPI.KenticoProviders.Providers
{
    public class KenticoPermissionsProvider : IKenticoPermissionsProvider
    {
        private string OrdersModuleCodename => "Kadena_Orders";

        private string ApproveOrdersPermissionName => "KDA_ApproveOrders";
        private string SeePricesPermissionName => "KDA_SeePrices";

        private readonly IMapper mapper;

        public KenticoPermissionsProvider(IMapper mapper)
        {
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public bool IsAuthorizedPerResource(string resourceName, string permissionName)
        {
            return IsAuthorizedPerResource(resourceName, permissionName, SiteContext.CurrentSiteName);
        }

        public bool IsAuthorizedPerResource(string resourceName, string permissionName, string siteName)
        {
            return MembershipContext.AuthenticatedUser.IsAuthorizedPerResource(resourceName, permissionName, siteName);
        }

        public bool UserCanSeePrices()
        {
            return UserInfoProvider.IsAuthorizedPerResource(OrdersModuleCodename, SeePricesPermissionName, SiteContext.CurrentSiteName, MembershipContext.AuthenticatedUser);
        }

        public bool UserCanSeePrices(int siteId, int userId)
        {
            var userinfo = UserInfoProvider.GetUserInfo(userId);
            var site = SiteInfoProvider.GetSiteInfo(siteId);

            if (userinfo == null || site == null)
                return false;

            return UserInfoProvider.IsAuthorizedPerResource(OrdersModuleCodename, SeePricesPermissionName, site.SiteName, userinfo);
        }

        public bool UserCanSeeAllOrders()
        {
            return UserInfoProvider.IsAuthorizedPerResource(OrdersModuleCodename, "KDA_SeeAllOrders", SiteContext.CurrentSiteName, MembershipContext.AuthenticatedUser);
        }

        public bool UserCanModifyShippingAddress()
        {
            return UserInfoProvider.IsAuthorizedPerResource("Kadena_User_Settings", "KDA_ModifyShippingAddress",
                SiteContext.CurrentSiteName, MembershipContext.AuthenticatedUser);
        }

        public bool UserCanDownloadHiresPdf(int siteId, int userId)
        {
            var userinfo = UserInfoProvider.GetUserInfo(userId);
            var site = SiteInfoProvider.GetSiteInfo(siteId);

            if (userinfo == null || site == null)
                return false;

            return UserInfoProvider.IsAuthorizedPerResource(OrdersModuleCodename, "KDA_CanDownloadHiresPdf", site.SiteName, userinfo);
        }

        public bool UserIsApprover(int siteId, int userId)
        {
            var userinfo = UserInfoProvider.GetUserInfo(userId);
            var site = SiteInfoProvider.GetSiteInfo(siteId);

            if (userinfo == null || site == null)
                return false;

            return UserInfoProvider.IsAuthorizedPerResource(OrdersModuleCodename, ApproveOrdersPermissionName, site.SiteName, userinfo);
        }

        public IEnumerable<User> GetUsersWithPermission(string resourceName, string permissionName, int siteId)
        {
            var users = new List<User>();
            var site = SiteInfoProvider.GetSiteInfo(siteId);

            if (site == null)
            {
                throw new ArgumentOutOfRangeException(nameof(siteId));
            }

            var userSet = UserInfoProvider.GetRequiredResourceUsers(OrdersModuleCodename, ApproveOrdersPermissionName, site.SiteName);

            var rows = userSet?.Tables?[0]?.Rows;

            if (rows != null)
            {
                foreach (var row in rows)
                {
                    users.Add(mapper.Map<User>(row));
                }
            }

            return users;
        }

        public IEnumerable<User> GetUsersWithApproverPermission(int siteId)
        {
            return GetUsersWithPermission(OrdersModuleCodename, ApproveOrdersPermissionName, siteId);
        }
    }
}
