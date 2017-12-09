using Kadena.BusinessLogic.Contracts;
using Kadena.WebAPI.KenticoProviders.Contracts;

namespace Kadena.BusinessLogic.Services
{
    public class BrandsService : IBrandsService
    {
        private readonly IKenticoBrandsProvider kenticoBrands;

        public BrandsService(IKenticoBrandsProvider kenticoBrands)
        {
            this.kenticoBrands = kenticoBrands;
        }

        public void DeleteBrand(int brandID)
        {
            kenticoBrands.DeleteBrand(brandID);
        }
    }
}
