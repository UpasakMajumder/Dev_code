using CMS.Membership;
using CMS.SiteProvider;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;
using System.IO;
using System.Linq;

namespace Kadena.Old_App_Code.Kadena.Imports.Users
{
    public class UserImportService
    {
        public byte[] GetTemplateFile()
        {
            var columns = GetColumns();
            var roles = GetAllRoles().Select(r => r.Description).ToArray();
            var file = CreateTemplateFile(columns, roles);
            return file;
        }

        private string[] GetColumns()
        {
            return new[]
            {
                "Company", "Organization ID", "Tax registration ID", "First name", "Last name", "Email",
                "Contact name", "Address line", "Address line 2", "City", "Postal code", "Country", "Phone number",
                "Role"
            };
        }

        private Role[] GetAllRoles()
        {
            var roles = RoleInfoProvider.GetAllRoles(SiteContext.CurrentSiteID)
                .Select(s => new Role { ID = s.RoleID, Description = s.RoleDisplayName })
                .ToArray();
            return roles;
        }

        /// <summary>
        /// Creates xlsx file.
        /// </summary>
        /// <param name="columns">Columns to create. Expects last column to be role.</param>
        /// <param name="roles">Roles to add to role select box for last column.</param>
        /// <returns></returns>
        private byte[] CreateTemplateFile(string[] columns, string[] roles)
        {
            // create workbook

            IWorkbook workbook = new XSSFWorkbook();
            var sheet = workbook.CreateSheet("User");
            var row = sheet.CreateRow(0);

            for (int i = 0; i < columns.Length; i++)
            {
                var cell = row.CreateCell(i);
                cell.SetCellValue(columns[i]);
            }

            // add validation for roles
            var rolesColumnIndex = columns.Length - 1;
            var maxRowsPerSheet = 1024 * 1024;
            var addressList = new CellRangeAddressList(1, maxRowsPerSheet - 1, rolesColumnIndex, rolesColumnIndex);
            var validationHelper = sheet.GetDataValidationHelper();
            var validationConstraint = validationHelper.CreateExplicitListConstraint(roles);
            var validation = validationHelper.CreateValidation(validationConstraint, addressList);
            validation.ShowErrorBox = true;
            sheet.AddValidationData(validation);

            using (var ms = new MemoryStream())
            {
                workbook.Write(ms);
                var bytes = ms.ToArray();
                return bytes;
            }
        }
    }
}