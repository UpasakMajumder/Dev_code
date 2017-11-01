using NPOI.SS.UserModel;
using NPOI.SS.Util;
using System.IO;
using System.Linq;

namespace Kadena.Old_App_Code.Kadena.Imports
{
    public abstract class TemplateServiceBase
    {
        protected static readonly int MaxRowsPerSheet = 1024 * 1024;

        protected static void CreateSheetHeader(string[] columns, ISheet sheet)
        {
            var row = sheet.CreateRow(0);
            var style = CreateHeaderStyle(sheet.Workbook);
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
        }

        protected static ICellStyle CreateHeaderStyle(IWorkbook workbook)
        {
            var font = workbook.CreateFont();
            font.IsBold = true;
            var style = workbook.CreateCellStyle();
            style.SetFont(font);
            return style;
        }

        protected string[] GetImportColumns<T>() where T : class
        {
            return ImportHelper.GetHeaderProperties<T>()
                .Select(p => p.Key)
                .ToArray();
        }

        protected byte[] GetWorkbookBytes(IWorkbook workbook)
        {
            using (var ms = new MemoryStream())
            {
                workbook.Write(ms);
                var bytes = ms.ToArray();
                return bytes;
            }
        }

        protected void AddOneFromManyValidation(int columnIndex, string sheetWithValuesName, string[] values, ISheet sheet)
        {
            var workbook = sheet.Workbook;
            var valuesSheet = workbook.CreateSheet(sheetWithValuesName);
            workbook.SetSheetHidden(1, SheetState.VeryHidden);
            for (int i = 0; i < values.Length; i++)
            {
                valuesSheet.CreateRow(i)
                    .CreateCell(0)
                    .SetCellValue(values[i]);
            }

            var addressList = new CellRangeAddressList(1, MaxRowsPerSheet - 1, columnIndex, columnIndex);
            var validationHelper = sheet.GetDataValidationHelper();
            var validationConstraint = validationHelper.CreateFormulaListConstraint($"{sheetWithValuesName}!$A$1:$A$" + values.Length);
            var validation = validationHelper.CreateValidation(validationConstraint, addressList);
            validation.ShowErrorBox = true;
            validation.CreateErrorBox("Validation failed", "Please choose a valid value.");
            sheet.AddValidationData(validation);
        }
    }
}