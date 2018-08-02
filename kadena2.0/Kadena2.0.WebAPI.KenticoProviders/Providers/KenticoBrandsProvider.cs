using AutoMapper;
using CMS.CustomTables;
using Kadena.Models.Brand;
using Kadena.WebAPI.KenticoProviders.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Kadena.WebAPI.KenticoProviders
{
    public class KenticoBrandsProvider : IKenticoBrandsProvider
    {
        private readonly string BrandTable = "KDA.Brand";
        private readonly string AddressBrandTable = "KDA.AddressBrands";
        private readonly IMapper mapper;

        public KenticoBrandsProvider(IMapper mapper)
        {
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public void DeleteBrand(int brandID)
        {
            CustomTableItem brand = CustomTableItemProvider.GetItem(brandID, BrandTable);
            if (brand != null)
            {
                brand.Delete();
            }
        }

        public List<Brand> GetBrands() =>
            mapper.Map<List<Brand>>(CustomTableItemProvider
                                    .GetItems(BrandTable)
                                    .Columns("ItemID,BrandCode,BrandName")
                                    .ToList());

        private Brand CreateBrand(CustomTableItem brandItem)
        {
            if (brandItem == null)
            {
                return null;
            }
            else
            {
                return new Brand()
                {
                    ItemID = brandItem.ItemID,
                    BrandName = brandItem.GetStringValue("BrandName", string.Empty),
                    BrandCode = brandItem.GetIntegerValue("BrandCode", default(int))
                };
            }
        }

        public List<Brand> GetAddressBrands(int addressID)
        {
            var addressBrandsList = CustomTableItemProvider.GetItems(AddressBrandTable)
                       .WhereEquals("AddressID", addressID)
                       .Columns("BrandID")
                       .ToList()
                       .Select(ab => ab.GetIntegerValue("BrandID", default(int)))
                       .ToList();

            return this.GetBrands(addressBrandsList).ToList();
        }

        public IEnumerable<Brand> GetBrands(List<int> brandIds)
        {
            var brands = CustomTableItemProvider
                .GetItems(BrandTable)
                .Columns("ItemID, BrandCode, BrandName")
                .WhereIn("ItemID", brandIds)
                .ToList();
            return mapper.Map<IEnumerable<Brand>>(brands);
        }
    }
}