using Kadena.Models.Membership;
using System.Collections.Generic;

namespace Kadena.BusinessLogic.Contracts
{
    public interface IRoleService
    {
        void AssignSSORoles(User user, int siteId, IEnumerable<string> ssoRoles);
        void AssignRoles(User user, int siteId, IEnumerable<string> roles);
    }
}
