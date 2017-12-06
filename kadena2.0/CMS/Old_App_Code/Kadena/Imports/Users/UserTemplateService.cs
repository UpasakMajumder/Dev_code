using NPOI.SS.UserModel;
using System.Collections.Generic;
using System.Linq;

namespace Kadena.Old_App_Code.Kadena.Imports.Users
{
    public class UserTemplateService : TemplateServiceBase
    {
        /// <summary>
        /// Creates xlsx file.
        /// </summary>
        /// <param name="headers">Columns to create. Expects last column to be role.</param>
        /// <returns></returns>
        protected override ISheet CreateSheet(List<Column> headers)
        {
            var sheet = base.CreateSheet(headers);

            // Roles to add to role select box for last column.
            var roles = OrderRolesByPriority(new RoleProvider().GetAllRoles(_siteId).Select(r => r.Description).ToArray());
            // add validation for roles
            if (roles != null)
            {
                var rolesColumnIndex = headers.Count - 1; // role column should be last
                AddOneFromManyValidation(rolesColumnIndex, "Roles", roles, sheet);
            }

            return sheet;
        }

        private string[] OrderRolesByPriority(string[] roles)
        {
            var kadenaRoles = new List<string>();
            var sortedRoles = new List<string>();
            foreach (var role in roles.OrderBy(r => r))
            {
                if (role.StartsWith("Kadena", System.StringComparison.InvariantCultureIgnoreCase))
                {
                    kadenaRoles.Add(role);
                }
                else
                {
                    sortedRoles.Add(role);
                }
            }
            sortedRoles.InsertRange(0, kadenaRoles);

            return sortedRoles.ToArray();
        }
    }
}