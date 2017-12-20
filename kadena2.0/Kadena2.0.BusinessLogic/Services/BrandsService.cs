using Kadena.BusinessLogic.Contracts;
using Kadena.WebAPI.KenticoProviders.Contracts;
using System;
using Kadena.Models.Brand;
using System.Collections.Generic;

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
        public List<Brand> GetBrands()
        {
            return kenticoBrands.GetBrands();
        }

        public List<Brand> GetAddressBrands(int addressID)
        {
            return kenticoBrands.GetAddressBrands(addressID);
        }
    }
}
