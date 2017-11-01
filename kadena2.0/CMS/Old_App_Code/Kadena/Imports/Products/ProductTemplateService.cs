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
                var index = (ImportHelper.GetHeaderProperties<ProductDto>()
                    .FirstOrDefault(p => p.Key == "Product Type").Value?
                    .GetCustomAttributes(typeof(HeaderAttribute), false)
                    .FirstOrDefault() as HeaderAttribute)?.Order ?? 0;
                    

                AddOneFromManyValidation(index, "ProductTypes", productTypes, sheet);
                sheet.SetColumnWidth(index, 256*productTypes.Max(t => t.Length));
            }

            return GetWorkbookBytes(workbook);
        }
    }
}