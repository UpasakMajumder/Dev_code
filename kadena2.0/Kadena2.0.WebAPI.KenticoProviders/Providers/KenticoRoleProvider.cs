using AutoMapper;
using CMS.Membership;
using CMS.SiteProvider;
using Kadena.Models.Membership;
using Kadena.WebAPI.KenticoProviders.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Kadena2.WebAPI.KenticoProviders.Providers
{
    public class KenticoRoleProvider : IKenticoRoleProvider
    {
        private readonly IMapper mapper;

        public KenticoRoleProvider(IMapper mapper)
        {
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public IEnumerable<Role> GetUserRoles(int userId)
        {
            var userRoleIDs = UserRoleInfoProvider.GetUserRoles()
                .Column("RoleID")
                .WhereEquals("UserID", userId);

            var roles = RoleInfoProvider.GetRoles()
                .WhereIn("RoleID", userRoleIDs);

            return mapper.Map<List<Role>>(roles);
        }

        public IEnumerable<Role> GetRoles(int siteId)
        {
            var roles = RoleInfoProvider.GetAllRoles(siteId).ToList();
            return mapper.Map<List<Role>>(roles);
        }

        public void AssignUserRoles(string userName, int siteId, IEnumerable<string> roles)
        {
            var siteName = GetSiteName(siteId);

            foreach (var role in roles)
            {
                UserInfoProvider.AddUserToRole(userName, role, siteName);
            }
        }

        public void RemoveUserRoles(string userName, int siteId, IEnumerable<string> roles)
        {
            var siteName = GetSiteName(siteId);

            foreach (var role in roles)
            {
                UserInfoProvider.RemoveUserFromRole(userName, siteName, role);
            }
        }

        public IEnumerable<User> GetRoleUsers(string roleName, int siteId)
        {
            var role = RoleInfoProvider.GetRoleInfo(roleName, siteId);

            if (role == null)
            {
                throw new ArgumentOutOfRangeException($"Cannot find role '{roleName}' on site '{siteId}'");
            }

            var usersTable = RoleInfoProvider.GetRoleUsers(role.RoleID);

            var users = new List<User>();

            foreach (var row in usersTable.Rows)
            {
                users.Add(mapper.Map<User>(row));
            }

            return users;
        }

        public bool UserHasRole(int userId, string roleName)
        {
            var user = UserInfoProvider.GetUserInfo(userId);

            if (user == null)
            {
                return false;
            }

            return UserInfoProvider.IsUserInRole(user.UserName, roleName, SiteContext.CurrentSiteName);
        }

        private string GetSiteName(int siteId)
        {
            var siteName = SiteInfoProvider.GetSiteName(siteId);

            if (string.IsNullOrEmpty(siteName))
            {
                throw new ArgumentOutOfRangeException(nameof(siteId), $"Unable to find site with id {siteId}");
            }

            return siteName;
        }
    }
}
