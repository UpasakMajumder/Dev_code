using Kadena.Models.Product;
using NPOI.HSSF.UserModel;
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

        private byte[] CreateTemplateFile(string[] columns, string[] productTypes)
        {
            IWorkbook workbook = new XSSFWorkbook();
            var sheet = workbook.CreateSheet("Products");
            var columnInfos = ImportHelper.GetHeaderProperties<ProductDto>();
            CreateSheetHeader(columns, sheet);
            
            if (productTypes != null)
            {
                var indexOfproductType = columnInfos.FirstOrDefault(c => c.Name == "Product Type").Order;
                AddOneFromManyValidation(indexOfproductType, "ProductTypes", productTypes, sheet);
                sheet.SetColumnWidth(indexOfproductType, 256 * productTypes.Max(t => t.Length));
            }

            var indexOfTrack = columnInfos.FirstOrDefault(c => c.Name == "Track Inventory").Order;
            AddOneFromManyValidation(indexOfTrack, "Trackings", new[] { "Yes", "No", "By variants" }, sheet);

            var indexOfChili1 = columnInfos.FirstOrDefault(c => c.Name == "Chili Template ID").Order;
            var indexOfChili2 = columnInfos.FirstOrDefault(c => c.Name == "Chili Workgroup ID").Order;
            var indexOfChili3 = columnInfos.FirstOrDefault(c => c.Name == "Product Chili Pdf generator SettingsId").Order;
            var guidFieldWidth = 256 * 36;
            sheet.SetColumnWidth(indexOfChili1, guidFieldWidth);
            sheet.SetColumnWidth(indexOfChili2, guidFieldWidth);
            sheet.SetColumnWidth(indexOfChili3, guidFieldWidth);

            return GetWorkbookBytes(workbook);
        }
    }
}