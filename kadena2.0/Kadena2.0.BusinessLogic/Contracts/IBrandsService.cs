using Kadena.Models.Brand;
using System.Collections.Generic;

namespace Kadena.BusinessLogic.Contracts
{
    public interface IBrandsService
    {
        void DeleteBrand(int brandID);

        List<Brand> GetBrands();

        List<Brand> GetAddressBrands(int addressID);
    }
}