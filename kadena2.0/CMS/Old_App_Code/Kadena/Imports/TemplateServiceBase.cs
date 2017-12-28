using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;
using System.Collections.Generic;
using System.IO;

namespace Kadena.Old_App_Code.Kadena.Imports
{
    public class TemplateServiceBase
    {
        private const int MaxRowsPerSheet = 1024 * 1024;
        protected int _siteId;

        public byte[] GetTemplateFile<T>(int siteID) where T : class
        {
            _siteId = siteID;
            var columns = GetImportColumns<T>();
            var sheet = CreateSheet(columns);
            return GetWorkbookBytes(sheet.Workbook);
        }

        private static void CreateSheetHeader(List<Column> columns, ISheet sheet)
        {
            var row = sheet.CreateRow(0);
            var standardStyle = CreateHeaderStyle(sheet.Workbook);
            var mandatoryStyle = CreateHeaderMandatoryStyle(sheet.Workbook);
            var charWidth = 256;
            var minimalColumnWidth = charWidth * 18;

            for (int i = 0; i < columns.Count; i++)
            {
                var cell = row.CreateCell(i);
                cell.SetCellValue(columns[i].Name);
                cell.CellStyle = columns[i].IsMandatory ? mandatoryStyle : standardStyle;
                sheet.AutoSizeColumn(i);

                if (sheet.GetColumnWidth(i) < minimalColumnWidth)
                {
                    sheet.SetColumnWidth(i, minimalColumnWidth);
                }
            }
        }

        private static ICellStyle CreateHeaderStyle(IWorkbook workbook)
        {
            var font = workbook.CreateFont();
            font.IsBold = true;
            var style = workbook.CreateCellStyle();
            style.SetFont(font);
            return style;
        }

        private static ICellStyle CreateHeaderMandatoryStyle(IWorkbook workbook)
        {
            var font = workbook.CreateFont();
            font.IsBold = true;
            font.Underline = FontUnderlineType.None;
            font.Color = IndexedColors.Automatic.Index;
            var style = workbook.CreateCellStyle();
            style.SetFont(font);
            return style;
        }

        private static List<Column> GetImportColumns<T>() where T : class
        {
            return ImportHelper.GetHeaderProperties<T>();
        }

        private static byte[] GetWorkbookBytes(IWorkbook workbook)
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

        protected virtual ISheet CreateSheet(List<Column> headers)
        {
            IWorkbook workbook = new XSSFWorkbook();
            var sheet = workbook.CreateSheet();
            CreateSheetHeader(headers, sheet);
            return sheet;
        }
    }
}