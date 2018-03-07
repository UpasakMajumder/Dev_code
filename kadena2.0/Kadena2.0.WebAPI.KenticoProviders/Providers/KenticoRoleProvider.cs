using AutoMapper;
using CMS.Membership;
using CMS.SiteProvider;
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

        public void AssignUserRoles(string userName, int siteId, IEnumerable<string> roles)
        {
            var siteName = SiteInfoProvider.GetSiteName(siteId);

            if (string.IsNullOrEmpty(siteName))
            {
                throw new ArgumentOutOfRangeException(nameof(siteId), $"Unable to find site with id {siteId}");
            }

            foreach (var role in roles)
            {
                UserInfoProvider.AddUserToRole(userName, siteName, role);
            }
        }
    }
}
