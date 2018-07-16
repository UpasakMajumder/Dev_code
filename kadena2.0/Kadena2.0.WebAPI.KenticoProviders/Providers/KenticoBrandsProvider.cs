﻿using CMS.CustomTables;
using Kadena.Models.Brand;
using Kadena.WebAPI.KenticoProviders.Contracts;
using System.Collections.Generic;
using System.Linq;

namespace Kadena.WebAPI.KenticoProviders
{
    public class KenticoBrandsProvider : IKenticoBrandsProvider
    {
        private readonly string BrandTable = "KDA.Brand";
        private readonly string AddressBrandTable = "KDA.AddressBrands";

        public void DeleteBrand(int brandID)
        {
            CustomTableItem brand = CustomTableItemProvider.GetItem(brandID, BrandTable);
            if (brand != null)
            {
                brand.Delete();
            }
        }

        public List<Brand> GetBrands() => CustomTableItemProvider.GetItems(BrandTable)
            .Columns("ItemID,BrandCode,BrandName")
            .Select(CreateBrand)
            .ToList();

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

            var brandsList = addressBrandsList
                .Select(b => CustomTableItemProvider.GetItem(b, BrandTable))
                .Select(CreateBrand)
                .Where(b => b != null)
                .ToList();

            return brandsList;
        }
    }
}