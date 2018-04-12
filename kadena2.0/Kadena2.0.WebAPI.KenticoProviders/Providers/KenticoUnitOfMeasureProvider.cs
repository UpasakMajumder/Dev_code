using Kadena.Models.Product;
using System.Linq;
using System;
using AutoMapper;
using CMS.CustomTables;
using Kadena.WebAPI.KenticoProviders.Contracts;

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
            var unit = CustomTableItemProvider.GetItems(CustomTableName)
                                          .WhereEquals("Name", name)
                                          .Columns("Name", "ErpCode", "LocalizationString")
                                          .FirstOrDefault();

            return mapper.Map<UnitOfMeasure>(unit);
        }

        public UnitOfMeasure GetDefaultUnitOfMeasure()
        {
            var unit = CustomTableItemProvider.GetItems(CustomTableName)
                                          .WhereEquals("IsDefault", true)
                                          .Columns("Name", "ErpCode", "LocalizationString")
                                          .FirstOrDefault();

            return mapper.Map<UnitOfMeasure>(unit);
        }
    }
}
