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

        private byte[] CreateTemplateFile(string[] columns, string[] productTypes)
        {
            IWorkbook workbook = new XSSFWorkbook();
            var sheet = workbook.CreateSheet("Products");
            CreateSheetHeader(columns, sheet);
            
            if (productTypes != null)
            {
                var indexOfproductType = ImportHelper.GetHeaderProperties<ProductDto>()
                    .FirstOrDefault(c => c.Name == "Product Type").Order;

                AddOneFromManyValidation(indexOfproductType, "ProductTypes", productTypes, sheet);
                sheet.SetColumnWidth(indexOfproductType, 256 * productTypes.Max(t => t.Length));
            }

            return GetWorkbookBytes(workbook);
        }
    }
}