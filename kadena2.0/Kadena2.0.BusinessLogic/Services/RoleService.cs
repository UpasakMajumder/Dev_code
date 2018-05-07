using Kadena.BusinessLogic.Contracts;
using Kadena.Models.Membership;
using Kadena.WebAPI.KenticoProviders.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Kadena.BusinessLogic.Services
{
    public class RoleService : IRoleService
    {
        private readonly IKenticoRoleProvider roleProvider;
        private readonly IKenticoLogger log;

        public RoleService(IKenticoRoleProvider roleProvider, IKenticoLogger log)
        {
            this.roleProvider = roleProvider ?? throw new ArgumentNullException(nameof(roleProvider));
            this.log = log ?? throw new ArgumentNullException(nameof(log));
        }

        public void AssignRoles(User user, int siteId, IEnumerable<string> roles)
        {
            if (roles == null)
            {
                throw new ArgumentNullException(nameof(roles));
            }

            var existingSiteRoles = this.roleProvider.GetRoles(siteId).Select(r => r.CodeName);
            var addRoles = existingSiteRoles.Intersect(roles);

            roleProvider.AssignUserRoles(user.UserName, siteId, addRoles);
        }

        public void AssignSSORoles(User user, int siteId, IEnumerable<string> ssoRoles)
        {
            var currentUserRolesNames = roleProvider.GetUserRoles(user.UserId).Select(r => r.CodeName);
            var rolesToAdd = ssoRoles.Except(currentUserRolesNames);
            var rolesToDelete = currentUserRolesNames.Except(ssoRoles);
            var allExistingRolesNames = roleProvider.GetRoles(siteId).Select(r => r.CodeName);
            var unknownSsoRoles = rolesToAdd.Except(allExistingRolesNames);
            var knownRolesToAdd = rolesToAdd.Except(unknownSsoRoles);

            if (unknownSsoRoles.Any())
            {
                var logMessage = $"Unknown roles received when updating user {user.UserId} on site {siteId} : {string.Join(",", unknownSsoRoles)}";
                log.LogError("SSO Update Roles", logMessage);
            }

            roleProvider.RemoveUserRoles(user.UserName, siteId, rolesToDelete);

            roleProvider.AssignUserRoles(user.UserName, siteId, knownRolesToAdd);
        }
    }
}
