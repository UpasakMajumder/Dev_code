using Kadena.Models.Membership;
using System.Collections.Generic;

namespace Kadena.WebAPI.KenticoProviders.Contracts
{
    public interface IKenticoRoleProvider
    {
        IEnumerable<Role> GetUserRoles(int userId);
        IEnumerable<Role> GetRoles(int siteId);
        void AssignUserRoles(string userName, int siteId, IEnumerable<string> roles);
        void RemoveUserRoles(string userName, int siteId, IEnumerable<string> roles);
    }
}
