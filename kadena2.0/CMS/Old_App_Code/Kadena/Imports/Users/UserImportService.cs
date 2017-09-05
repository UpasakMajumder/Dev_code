using CMS.Membership;
using CMS.SiteProvider;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Kadena.Old_App_Code.Kadena.Imports.Users
{
    public class UserImportService
    {
        public byte[] GetTemplateFile(int siteID)
        {
            var columns = GetColumns();
            var roles = GetAllRoles(siteID).Select(r => r.Description).ToArray();
            var file = CreateTemplateFile(columns, roles);
            return file;
        }

        public void ProcessImportFile(byte[] importFileData, ExcelType type, int siteID)
        {
            var rows = ReadDataFromExcelFile(importFileData, type);

            // TODO: process data
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

        private Role[] GetAllRoles(int siteID)
        {
            var roles = RoleInfoProvider.GetAllRoles(siteID)
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

            var font = workbook.CreateFont();
            font.IsBold = true;
            var style = workbook.CreateCellStyle();
            style.SetFont(font);
            var charWidth = 256;
            var minimalColumnWidth = charWidth * 18;

            for (int i = 0; i < columns.Length; i++)
            {
                var cell = row.CreateCell(i);
                cell.SetCellValue(columns[i]);
                cell.CellStyle = style;
                sheet.AutoSizeColumn(i);

                if (sheet.GetColumnWidth(i) < minimalColumnWidth)
                {
                    sheet.SetColumnWidth(i, minimalColumnWidth);
                }
            }

            // add validation for roles
            var rolesSheet = workbook.CreateSheet("Roles");
            workbook.SetSheetHidden(1, SheetState.VeryHidden);
            for (int i = 0; i < roles.Length; i++)
            {
                rolesSheet.CreateRow(i)
                    .CreateCell(0)
                    .SetCellValue(roles[i]);
            }

            var rolesColumnIndex = columns.Length - 1;
            var maxRowsPerSheet = 1024 * 1024;
            var addressList = new CellRangeAddressList(1, maxRowsPerSheet - 1, rolesColumnIndex, rolesColumnIndex);
            var validationHelper = sheet.GetDataValidationHelper();
            var validationConstraint = validationHelper.CreateFormulaListConstraint("Roles!$A1:$A" + roles.Length);
            var validation = validationHelper.CreateValidation(validationConstraint, addressList);
            validation.ShowErrorBox = true;
            validation.CreateErrorBox("Validation failed", "Please choose a valid role.");
            sheet.AddValidationData(validation);

            using (var ms = new MemoryStream())
            {
                workbook.Write(ms);
                var bytes = ms.ToArray();
                return bytes;
            }
        }

        /// <summary>
        /// Reads first sheet from the file.
        /// </summary>
        private static List<string[]> ReadDataFromExcelFile(byte[] fileData, ExcelType type)
        {
            using (var file = new MemoryStream(fileData))
            {
                var workBook = OpenWorkBook(file, type);
                var sheet = workBook.GetSheetAt(0);

                var header = GetHeader(sheet);
                if (header.Length == 0)
                {
                    return new List<string[]>();
                }

                var data = new List<string[]> { header };
                var rowsEnumarator = sheet.GetRowEnumerator();

                // skip header
                rowsEnumarator.MoveNext();

                while (rowsEnumarator.MoveNext())
                {
                    var row = (IRow)rowsEnumarator.Current;
                    var rowData = new string[header.Length];
                    for (int i = 0; i < header.Length; i++)
                    {
                        rowData[i] = row.GetCell(i)?.ToString();
                    }

                    data.Add(rowData);
                }

                return data;
            }
        }

        private static IWorkbook OpenWorkBook(Stream file, ExcelType type)
        {
            if (type == ExcelType.Xlsx)
            {
                return new XSSFWorkbook(file);
            }
            else
            {
                return new HSSFWorkbook(file);
            }
        }

        private static string[] GetHeader(ISheet sheet)
        {
            var columnNames = new List<string>();

            var headerRow = sheet.GetRow(0);
            if (headerRow == null)
            {
                return new string[0];
            }

            foreach (var cell in headerRow)
            {
                columnNames.Add(cell.StringCellValue);
            }

            return columnNames.ToArray();
        }
    }
}