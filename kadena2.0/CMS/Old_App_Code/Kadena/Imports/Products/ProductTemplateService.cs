using Kadena.Models.Product;
using NPOI.SS.UserModel;
using System.Collections.Generic;
using System.Linq;

namespace Kadena.Old_App_Code.Kadena.Imports.Products
{
    public class ProductTemplateService : TemplateServiceBase
    {
        private string[] GetProductTypes()
        {
            var types = ProductTypes.GetAll().ToList();
            types.Add(ProductTypes.Combine(ProductTypes.MailingProduct, ProductTypes.TemplatedProduct));
            return types.ToArray();
        }

        protected override ISheet CreateSheet(List<Column> headers)
        {
            var sheet = base.CreateSheet(headers);

            var productTypes = GetProductTypes();
            if ((productTypes?.Count() ?? 0) > 0)
            {
                var indexOfproductType = headers.FirstOrDefault(c => c.PropertyInfo.Name == nameof(ProductDto.ProductType)).Order;
                AddOneFromManyValidation(indexOfproductType, "ProductTypes", productTypes, sheet);
                sheet.SetColumnWidth(indexOfproductType, 256 * productTypes.Max(t => t.Length));
            }

            var indexOfTrack = headers.FirstOrDefault(c => c.PropertyInfo.Name == nameof(ProductDto.TrackInventory)).Order;
            AddOneFromManyValidation(indexOfTrack, "Trackings", new[] { "Yes", "No", "By variants" }, sheet);

            var indexOfChili1 = headers.FirstOrDefault(c => c.PropertyInfo.Name == nameof(ProductDto.ChiliTemplateID)).Order;
            var indexOfChili2 = headers.FirstOrDefault(c => c.PropertyInfo.Name == nameof(ProductDto.ChiliWorkgroupID)).Order;
            var indexOfChili3 = headers.FirstOrDefault(c => c.PropertyInfo.Name == nameof(ProductDto.ChiliPdfGeneratorSettingsID)).Order;

            var guidFieldWidth = 256 * 36;

            sheet.SetColumnWidth(indexOfChili1, guidFieldWidth);
            sheet.SetColumnWidth(indexOfChili2, guidFieldWidth);
            sheet.SetColumnWidth(indexOfChili3, guidFieldWidth);

            return sheet;
        }
    }
}