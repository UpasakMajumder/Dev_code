using CMS.CustomTables;
using CMS.CustomTables.Types.KDA;
using CMS.DocumentEngine.Types.KDA;
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
            BindCampaignList(sheet);
            BindProgramList(sheet);
            BindPOSList(sheet);
            BindStatesGroupList(sheet);
            BindBrandList(sheet);
            BindProductCategoryList(sheet);
            BindStatusList(sheet);
            BindItemSpecsList(sheet);
            return sheet;
        }

        private void BindCampaignList(ISheet sheet)
        {
            string[] campaignNames = CampaignProvider.GetCampaigns().OnCurrentSite().Select(x => x.Name).ToArray();
            if (campaignNames != null && campaignNames.Count() > 0)
            {
                AddOneFromManyValidation(0, "CampaignNames", campaignNames, sheet);
            }
        }

        private void BindProgramList(ISheet sheet)
        {
            string[] programNames = ProgramProvider.GetPrograms().OnCurrentSite().Select(x => x.ProgramName).ToArray();
            if (programNames != null && programNames.Count() > 0)
            {
                AddOneFromManyValidation(1, "ProgramNames", programNames, sheet);
            }
        }

        private void BindPOSList(ISheet sheet)
        {
            string[] posList = CustomTableItemProvider.GetItems<POSNumberItem>().ToList().Select(x => x.POSNumber.ToString()).ToArray();
            if (posList != null && posList.Count() > 0)
            {
                AddOneFromManyValidation(2, "POSList", posList, sheet);
            }
        }

        private void BindStatesGroupList(ISheet sheet)
        {
            string[] statesGroupList = CustomTableItemProvider.GetItems<StatesGroupItem>().ToList().Select(x => x.GroupName).ToArray();
            if (statesGroupList != null && statesGroupList.Count() > 0)
            {
                AddOneFromManyValidation(5, "StatesGroupList", statesGroupList, sheet);
            }
        }

        private void BindBrandList(ISheet sheet)
        {
            string[] brandList = CustomTableItemProvider.GetItems<BrandItem>().ToList().Select(x => x.BrandName).ToArray();
            if (brandList != null && brandList.Count() > 0)
            {
                AddOneFromManyValidation(8, "BrandList", brandList, sheet);
            }
        }

        private void BindProductCategoryList(ISheet sheet)
        {
            string[] productCategoryList = ProductCategoryProvider.GetProductCategories().OnCurrentSite().Select(x => x.ProductCategoryTitle).ToArray();
            if (productCategoryList != null && productCategoryList.Count() > 0)
            {
                AddOneFromManyValidation(11, "ProductCategoryList", productCategoryList, sheet);
            }
        }

        private void BindStatusList(ISheet sheet)
        {
            string[] statusList = new string[] { "Active", "In-Active" };
            AddOneFromManyValidation(15, "ProductStatus", statusList, sheet);
        }

        private void BindItemSpecsList(ISheet sheet)
        {
            string[] itemSpecsList = CustomTableItemProvider.GetItems<ProductItemSpecsItem>().ToList().Select(x => x.ItemSpec).ToArray();
            if (itemSpecsList != null && itemSpecsList.Count() > 0)
            {
                AddOneFromManyValidation(16, "ItemSpecsList", itemSpecsList, sheet);
            }
        }
    }
}