using CMS.CustomTables;
using CMS.CustomTables.Types.KDA;
using NPOI.SS.UserModel;
using System.Collections.Generic;
using System.Linq;

namespace Kadena.Old_App_Code.Kadena.Imports.POS
{
    public class POSTemplateService : TemplateServiceBase
    {
        protected override ISheet CreateSheet(List<Column> headers)
        {
            var sheet = base.CreateSheet(headers);
            var posCategories = GetPOSCategories();
            if (posCategories != null)
            {
                var posCategoryIndex = headers.Count - 1;
                AddOneFromManyValidation(posCategoryIndex, "POSCategories", posCategories, sheet);
            }
            var brands = GetBrands();
            if (brands != null)
            {
                AddOneFromManyValidation(0, "Brands", brands, sheet);
            }
            return sheet;
        }

        private string[] GetPOSCategories()
        {
            return CustomTableItemProvider.GetItems<POSCategoryItem>().ToList().Select(x => x.PosCategoryName).ToArray();
        }

        private string[] GetBrands()
        {
            return CustomTableItemProvider.GetItems<BrandItem>().ToList().Select(x => x.BrandName).ToArray();
        }
    }
}