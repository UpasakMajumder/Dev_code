using CMS.CustomTables;
using Kadena.WebAPI.KenticoProviders.Contracts;

namespace Kadena.WebAPI.KenticoProviders
{
    public class KenticoBrandsProvider : IKenticoBrandsProvider
    {
        private readonly string CustomTableName = "KDA.Brand";

        public void DeleteBrand(int brandID)
        {
            CustomTableItem brand = CustomTableItemProvider.GetItem(brandID, CustomTableName);
            if (brand != null)
            {
                brand.Delete();
            }
        }
    }
}
