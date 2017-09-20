using CMS.Membership;
using System.Linq;

namespace Kadena.Old_App_Code.Kadena.Imports.Users
{
    public class RoleProvider
    {
        public Role[] GetAllRoles(int siteID)
        {
            var roles = RoleInfoProvider.GetAllRoles(siteID)
                .Select(s => new Role { ID = s.RoleID, Description = s.RoleDisplayName })
                .ToArray();
            return roles;
        }
    }
}