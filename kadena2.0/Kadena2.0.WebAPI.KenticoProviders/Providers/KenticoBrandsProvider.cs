using CMS.CustomTables;
using CMS.DataEngine;
using Kadena.Models.Brand;
using Kadena.WebAPI.KenticoProviders.Contracts;
using System.Collections.Generic;
using System.Linq;

namespace Kadena.WebAPI.KenticoProviders
{
    public class KenticoBrandsProvider : IKenticoBrandsProvider
    {
        private readonly string CustomTableName = "KDA.Brand";
        private readonly string AddressBrandTable = "KDA.AddressBrands";

        public void DeleteBrand(int brandID)
        {
            CustomTableItem brand = CustomTableItemProvider.GetItem(brandID, CustomTableName);
            if (brand != null)
            {
                brand.Delete();
            }
        }

        public List<Brand> GetBrands()
        {
            ObjectQuery<CustomTableItem> brands = CustomTableItemProvider.GetItems(CustomTableName)
                .Columns("ItemID,BrandCode,BrandName");
            if (brands != null)
            {
                return brands.Select(x => CreateBrand(x)).ToList();
            }
            return null;
        }

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
            List<CustomTableItem> addressBrandsList = CustomTableItemProvider.GetItems(AddressBrandTable)
                       .WhereEquals("AddressID", addressID)
                       .Columns("BrandID")
                       .ToList();
            List<CustomTableItem> brandList = new List<CustomTableItem>();
            if (addressBrandsList != null)
            {
                addressBrandsList.ForEach(b =>
                {
                    var brandData = CustomTableItemProvider.GetItem(b.GetIntegerValue("BrandID", default(int)), CustomTableName);
                    brandList.Add(brandData);
                });
                return brandList.Select(x => CreateBrand(x)).Where(x => x != null).ToList();
            }
            return null;
        }
    }
}