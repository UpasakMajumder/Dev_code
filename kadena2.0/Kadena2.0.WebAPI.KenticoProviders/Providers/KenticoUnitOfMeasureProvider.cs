using Kadena.Models.Product;
using System.Linq;
using System;
using AutoMapper;
using CMS.CustomTables;
using Kadena.WebAPI.KenticoProviders.Contracts;
using CMS.Helpers;
using CMS.Localization;

namespace Kadena.WebAPI.KenticoProviders
{
    public class KenticoUnitOfMeasureProvider : IKenticoUnitOfMeasureProvider
    {
        private readonly IMapper mapper;
        private readonly string CustomTableName = "KDA.UnitOfMeasure";

        public KenticoUnitOfMeasureProvider(IMapper mapper)
        {
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public UnitOfMeasure GetUnitOfMeasure(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException(nameof(name), "Empty name of UnitOfMeasure");
            }

            var unit = CustomTableItemProvider.GetItems(CustomTableName)
                                          .WhereEquals("Name", name)
                                          .Columns("Name", "ErpCode", "LocalizationString", "IsDefault")
                                          .FirstOrDefault();

            if (unit == null)
            {
                throw new ArgumentOutOfRangeException(nameof(name), $"Unable to find item with name '{name}' in UnitOfMeasure table");
            }

            return mapper.Map<UnitOfMeasure>(unit);
        }

        public UnitOfMeasure GetUnitOfMeasureByCode(string erpCode)
        {
            var unit = CustomTableItemProvider.GetItems(CustomTableName)
                                          .WhereEquals("ErpCode", erpCode)
                                          .Columns("Name", "ErpCode", "LocalizationString", "IsDefault")
                                          .FirstOrDefault();

            return mapper.Map<UnitOfMeasure>(unit);
        }

        public UnitOfMeasure GetDefaultUnitOfMeasure()
        {
            var unit = CustomTableItemProvider.GetItems(CustomTableName)
                                          .WhereEquals("IsDefault", true)
                                          .Columns("Name", "ErpCode", "LocalizationString", "IsDefault")
                                          .FirstOrDefault();

            return mapper.Map<UnitOfMeasure>(unit);
        }

        public string GetDisplaynameByCode(string erpCode)
        {
            var unit = GetUnitOfMeasureByCode(erpCode);
            return Translate(unit, erpCode);
        }

        public string GetDisplayname(string unitName)
        {
            var unit = GetUnitOfMeasure(unitName);
            return Translate(unit, unitName);
        }

        private string Translate(UnitOfMeasure uom, string defaultName)
        {
            if (uom == null)
            {
                return defaultName;
            }

            return ResHelper.GetString(uom.LocalizationString, LocalizationContext.CurrentCulture.CultureCode);
        }
    }
}
