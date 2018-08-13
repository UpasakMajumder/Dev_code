using Kadena.Models.Brand;
using System.Collections.Generic;

namespace Kadena.WebAPI.KenticoProviders.Contracts
{
    public interface IKenticoBrandsProvider
    {
        void DeleteBrand(int brandID);

        List<Brand> GetBrands();

        List<Brand> GetAddressBrands(int addressID);

        IEnumerable<Brand> GetBrands(List<int> brandIds);
    }
}