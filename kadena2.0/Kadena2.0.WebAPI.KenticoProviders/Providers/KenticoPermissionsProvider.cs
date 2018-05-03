using AutoMapper;
using CMS.Membership;
using CMS.SiteProvider;
using Kadena.Models.Membership;
using Kadena.Models.SiteSettings.Permissions;
using Kadena2.WebAPI.KenticoProviders.Contracts;
using System;
using System.Collections.Generic;

namespace Kadena2.WebAPI.KenticoProviders.Providers
{
    public class KenticoPermissionsProvider : IKenticoPermissionsProvider
    {
        private readonly IMapper mapper;

        public KenticoPermissionsProvider(IMapper mapper)
        {
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public bool CurrentUserHasPermission(string resourceName, string permissionName)
        {
            return CurrentUserHasPermission(resourceName, permissionName, SiteContext.CurrentSiteName);
        }

        public bool CurrentUserHasPermission(string resourceName, string permissionName, string siteName)
        {
            return MembershipContext.AuthenticatedUser.IsAuthorizedPerResource(resourceName, permissionName, siteName);
        }

        public bool UserHasPermission(int userId, string resourceName, string permissionName)
        {
            return UserHasPermission(userId, resourceName, permissionName, SiteContext.CurrentSiteName);
        }

        public bool UserHasPermission(int userId, string resourceName, string permissionName, string siteName)
        {
            var user = UserInfoProvider.GetUserInfo(userId);

            if (user == null)
            {
                return false;
            }

            return UserInfoProvider.IsAuthorizedPerResource(resourceName, permissionName, siteName, user);
        }

        public bool UserCanSeePrices()
        {
            return UserInfoProvider.IsAuthorizedPerResource(ModulePermissions.KadenaOrdersModule,
                                                            ModulePermissions.KadenaOrdersModule.SeePrices, 
                                                            SiteContext.CurrentSiteName, 
                                                            MembershipContext.AuthenticatedUser);
        }

        public bool UserCanSeePrices(int siteId, int userId)
        {
            var userinfo = UserInfoProvider.GetUserInfo(userId);
            var site = SiteInfoProvider.GetSiteInfo(siteId);

            if (userinfo == null || site == null)
                return false;

            return UserInfoProvider.IsAuthorizedPerResource(ModulePermissions.KadenaOrdersModule,
                                                            ModulePermissions.KadenaOrdersModule.SeePrices, 
                                                            site.SiteName, 
                                                            userinfo);
        }

        public bool UserCanSeeAllOrders()
        {
            return UserInfoProvider.IsAuthorizedPerResource(ModulePermissions.KadenaOrdersModule, 
                                                            ModulePermissions.KadenaOrdersModule.SeeAllOrders, 
                                                            SiteContext.CurrentSiteName, 
                                                            MembershipContext.AuthenticatedUser);
        }

        public bool UserCanModifyShippingAddress()
        {
            return UserInfoProvider.IsAuthorizedPerResource(ModulePermissions.KadenaUserSettingsModule, 
                                                            ModulePermissions.KadenaUserSettingsModule.ModifyShippingAddress,
                                                            SiteContext.CurrentSiteName, 
                                                            MembershipContext.AuthenticatedUser);
        }

        public bool UserCanDownloadHiresPdf(int siteId, int userId)
        {
            var userinfo = UserInfoProvider.GetUserInfo(userId);
            var site = SiteInfoProvider.GetSiteInfo(siteId);

            if (userinfo == null || site == null)
                return false;

            return UserInfoProvider.IsAuthorizedPerResource(ModulePermissions.KadenaOrdersModule,
                                                            ModulePermissions.KadenaOrdersModule.CanDownloadHiresPdf,
                                                            site.SiteName, 
                                                            userinfo);
        }

        public IEnumerable<User> GetUsersWithPermission(string resourceName, string permissionName, int siteId)
        {
            var users = new List<User>();
            var site = SiteInfoProvider.GetSiteInfo(siteId);

            if (site == null)
            {
                throw new ArgumentOutOfRangeException(nameof(siteId));
            }

            var userSet = UserInfoProvider.GetRequiredResourceUsers(ModulePermissions.KadenaOrdersModule,
                                                                    ModulePermissions.KadenaOrdersModule.ApproveOrders, 
                                                                    site.SiteName);

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
    }
}
