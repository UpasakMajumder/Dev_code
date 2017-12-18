using NPOI.SS.UserModel;
using System.Collections.Generic;
using System.Linq;

namespace Kadena.Old_App_Code.Kadena.Imports.Products
{
    public class CampaignProductImagesTemplateService : TemplateServiceBase
    {
        protected override ISheet CreateSheet(List<Column> headers)
        {
            var sheet = base.CreateSheet(headers);
            return sheet;
        }
    }
}