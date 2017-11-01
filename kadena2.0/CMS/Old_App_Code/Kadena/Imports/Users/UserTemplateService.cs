using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Kadena.Old_App_Code.Kadena.Imports.Users
{
    public class UserTemplateService : TemplateServiceBase
    {
        public byte[] GetUserTemplateFile(int siteID)
        {
            var columns = GetImportColumns<UserDto>();
            var roles = OrderRolesByPriority(new RoleProvider().GetAllRoles(siteID).Select(r => r.Description).ToArray());
            var file = CreateTemplateFile(columns, roles);
            return file;
        }

        public byte[] GetAddressTemplateFile()
        {
            var columns = GetImportColumns<AddressDto>();
            var file = CreateTemplateFile(columns);
            return file;
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

        /// <summary>
        /// Creates xlsx file.
        /// </summary>
        /// <param name="columns">Columns to create. Expects last column to be role.</param>
        /// <param name="roles">Roles to add to role select box for last column.</param>
        /// <returns></returns>
        private byte[] CreateTemplateFile(string[] columns, string[] roles = null)
        {
            // create workbook
            IWorkbook workbook = new XSSFWorkbook();
            var sheet = workbook.CreateSheet("Users");
            CreateSheetHeader(columns, sheet);

            // add validation for roles
            if (roles != null)
            {
                var rolesColumnIndex = columns.Length - 1; // role column should be last
                AddRolesValidation(rolesColumnIndex, roles, sheet);
            }

            using (var ms = new MemoryStream())
            {
                workbook.Write(ms);
                var bytes = ms.ToArray();
                return bytes;
            }
        }

        private void AddRolesValidation(int rolesColumnIndex, string[] roles, ISheet sheet)
        {
            var workbook = sheet.Workbook;
            var rolesSheet = workbook.CreateSheet("Roles");
            workbook.SetSheetHidden(1, SheetState.VeryHidden);
            for (int i = 0; i < roles.Length; i++)
            {
                rolesSheet.CreateRow(i)
                    .CreateCell(0)
                    .SetCellValue(roles[i]);
            }

            var addressList = new CellRangeAddressList(1, MaxRowsPerSheet - 1, rolesColumnIndex, rolesColumnIndex);
            var validationHelper = sheet.GetDataValidationHelper();
            var validationConstraint = validationHelper.CreateFormulaListConstraint("Roles!$A$1:$A$" + roles.Length);
            var validation = validationHelper.CreateValidation(validationConstraint, addressList);
            validation.ShowErrorBox = true;
            validation.CreateErrorBox("Validation failed", "Please choose a valid role.");
            sheet.AddValidationData(validation);
        }
    }
}