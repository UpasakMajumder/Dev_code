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
        private readonly IKenticoRoleProvider roles;
        private readonly IKenticoLogger log;

        public RoleService(IKenticoRoleProvider roles, IKenticoLogger log)
        {
            if (roles == null)
            {
                throw new ArgumentNullException(nameof(roles));
            }
            if (log == null)
            {
                throw new ArgumentNullException(nameof(log));
            }

            this.roles = roles;
            this.log = log;
        }

        public void AssignSSORoles(User user, int siteId, IEnumerable<string> ssoRoles)
        {
            var currentUserRolesNames = roles.GetUserRoles(user.UserId).Select(r => r.CodeName);
            var rolesToAdd = ssoRoles.Except(currentUserRolesNames);
            var rolesToDelete = currentUserRolesNames.Except(ssoRoles);
            var allExistingRolesNames = roles.GetRoles(siteId).Select(r => r.CodeName);
            var unknownSsoRoles = rolesToAdd.Except(allExistingRolesNames);
            var knownRolesToAdd = rolesToAdd.Except(unknownSsoRoles);

            if (unknownSsoRoles.Any())
            {
                var logMessage = $"Unknown roles received when updating user {user.UserId} on site {siteId} : {string.Join(",", unknownSsoRoles)}";
                log.LogError("SSO Update Roles", logMessage);
            }

            roles.RemoveUserRoles(user.UserName, siteId, rolesToDelete);

            roles.AssignUserRoles(user.UserName, siteId, knownRolesToAdd);
        }
    }
}
