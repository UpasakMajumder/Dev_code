using NPOI.SS.UserModel;
using System.Collections.Generic;
using System.Linq;

namespace Kadena.Old_App_Code.Kadena.Imports.Products
{
    public class ProductImagesTemplateService : TemplateServiceBase
    {
        protected override ISheet CreateSheet(List<Column> headers)
        {
            var sheet = base.CreateSheet(headers);

            var indexOfUrl1 = headers.FirstOrDefault(c => c.PropertyInfo.Name == nameof(ProductImageDto.ImageURL)).Order;
            var indexOfUrl2 = headers.FirstOrDefault(c => c.PropertyInfo.Name == nameof(ProductImageDto.ThumbnailURL)).Order;

            var urlFieldWidth = 256 * 100;
            sheet.SetColumnWidth(indexOfUrl1, urlFieldWidth);
            sheet.SetColumnWidth(indexOfUrl2, urlFieldWidth);

            return sheet;
        }
    }
}