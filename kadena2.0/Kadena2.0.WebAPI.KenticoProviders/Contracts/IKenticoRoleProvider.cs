using Kadena.Models;
using System.Collections.Generic;

namespace Kadena.WebAPI.KenticoProviders.Contracts
{
    public interface IKenticoRoleProvider
    {
        IEnumerable<Role> GetUserRoles(int userId);
        IEnumerable<Role> GetRoles(int siteId);
        bool RoleExists(string roleName, string siteName);
        int CreateRole(Role role);
        void AssignUserRole(int userId, int roleId);
    }
}
