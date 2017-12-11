using Kadena.BusinessLogic.Contracts;
using Kadena.WebAPI.KenticoProviders.Contracts;
using System;

namespace Kadena.BusinessLogic.Services
{
    public class BrandsService : IBrandsService
    {
        private readonly IKenticoBrandsProvider kenticoBrands;

        public BrandsService(IKenticoBrandsProvider kenticoBrands)
        {
            if (kenticoBrands == null)
            {
                throw new ArgumentNullException(nameof(kenticoBrands));
            }
            this.kenticoBrands = kenticoBrands;
        }

        public void DeleteBrand(int brandID)
        {
            kenticoBrands.DeleteBrand(brandID);
        }
    }
}
