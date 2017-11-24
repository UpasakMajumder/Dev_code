using Kadena.Models.Product;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.Collections.Generic;
using System.Linq;

namespace Kadena.Old_App_Code.Kadena.Imports.Products
{
    public class ProductTemplateService : TemplateServiceBase
    {
        public byte[] GetProductTemplateFile(int siteID)
        {
            var columns = GetImportColumns<ProductDto>();
            var productTypes = GetProductTypes();
            return CreateTemplateFile(columns, productTypes.ToArray());
        }

        private List<string> GetProductTypes()
        {
            var types = ProductTypes.GetAll().ToList();
            types.Add(ProductTypes.Combine(ProductTypes.MailingProduct, ProductTypes.TemplatedProduct));
            return types;
        }

        private byte[] CreateTemplateFile(List<Column> columnInfos, string[] productTypes)
        {
            IWorkbook workbook = new XSSFWorkbook();
            var sheet = workbook.CreateSheet("Products");
            CreateSheetHeader(columnInfos, sheet);
            
            if (productTypes != null)
            {
                var indexOfproductType = columnInfos.FirstOrDefault(c => c.PropertyInfo.Name == nameof(ProductDto.ProductType)).Order;
                AddOneFromManyValidation(indexOfproductType, "ProductTypes", productTypes, sheet);
                sheet.SetColumnWidth(indexOfproductType, 256 * productTypes.Max(t => t.Length));
            }

            var indexOfTrack = columnInfos.FirstOrDefault(c => c.PropertyInfo.Name == nameof(ProductDto.TrackInventory)).Order;
            AddOneFromManyValidation(indexOfTrack, "Trackings", new[] { "Yes", "No", "By variants" }, sheet);

            var indexOfChili1 = columnInfos.FirstOrDefault(c => c.PropertyInfo.Name == nameof(ProductDto.ChiliTemplateID)).Order;
            var indexOfChili2 = columnInfos.FirstOrDefault(c => c.PropertyInfo.Name == nameof(ProductDto.ChiliWorkgroupID)).Order;
            var indexOfChili3 = columnInfos.FirstOrDefault(c => c.PropertyInfo.Name == nameof(ProductDto.ChiliPdfGeneratorSettingsID)).Order;
            var indexOfUrl1 = columnInfos.FirstOrDefault(c => c.PropertyInfo.Name == nameof(ProductDto.ImageURL)).Order;
            var indexOfUrl2 = columnInfos.FirstOrDefault(c => c.PropertyInfo.Name == nameof(ProductDto.ThumbnailURL)).Order;

            var guidFieldWidth = 256 * 36;
            var urlFieldWidth = 256 * 100;

            sheet.SetColumnWidth(indexOfChili1, guidFieldWidth);
            sheet.SetColumnWidth(indexOfChili2, guidFieldWidth);
            sheet.SetColumnWidth(indexOfChili3, guidFieldWidth);

            sheet.SetColumnWidth(indexOfUrl1, urlFieldWidth);
            sheet.SetColumnWidth(indexOfUrl2, urlFieldWidth);

            return GetWorkbookBytes(workbook);
        }
    }
}