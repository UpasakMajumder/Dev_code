using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.Collections.Generic;
using System.IO;

namespace Kadena.Old_App_Code.Kadena.Imports
{
    public static class ExcelReader
    {
        /// <summary>
        /// Reads first sheet from the file.
        /// </summary>
        public static List<string[]> ReadDataFromExcelFile(byte[] fileData, ExcelType type)
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