using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.Collections.Generic;
using System.Linq;

namespace Kadena.Old_App_Code.Kadena.Imports.Products
{
    public class ProductImagesTemplateService : TemplateServiceBase
    {
        public byte[] GetProductImagesTemplateFile(int siteID)
        {
            var columns = GetImportColumns<ProductImageDto>();
            return CreateTemplateFile(columns);
        }
        private byte[] CreateTemplateFile(List<Column> columns)
        {
            IWorkbook workbook = new XSSFWorkbook();
            var sheet = workbook.CreateSheet("ProductImages");
            var columnInfos = ImportHelper.GetHeaderProperties<ProductImageDto>();
            CreateSheetHeader(columns, sheet);

            var indexOfUrl1 = columnInfos.FirstOrDefault(c => c.Name == "Image URL").Order;
            var indexOfUrl2 = columnInfos.FirstOrDefault(c => c.Name == "Thumbnail URL").Order;

            var urlFieldWidth = 256 * 100;
            sheet.SetColumnWidth(indexOfUrl1, urlFieldWidth);
            sheet.SetColumnWidth(indexOfUrl2, urlFieldWidth);

            return GetWorkbookBytes(workbook);
        }
    }
}