using AutoMapper;
using CMS.Membership;
using Kadena.Models;
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
            if (mapper == null)
            {
                throw new ArgumentNullException(nameof(mapper));
            }

            this.mapper = mapper;
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

        public bool RoleExists(string roleName, string siteName)
        {
            return RoleInfoProvider.RoleExists(roleName, siteName);
        }

        /// <summary>
        /// Creates new Role
        /// </summary>
        /// <returns>Id of new role</returns>
        public int CreateRole(Role role)
        {
            var newRole = mapper.Map<RoleInfo>(role);
            RoleInfoProvider.SetRoleInfo(newRole);
            return newRole.RoleID;
        }

        public void AssignUserRole(int userId, int roleId)
        {
            UserInfoProvider.AddUserToRole(userId, roleId);
        }
    }
}
