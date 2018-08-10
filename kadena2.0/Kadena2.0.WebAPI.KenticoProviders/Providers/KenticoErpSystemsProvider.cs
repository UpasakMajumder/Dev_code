using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using CMS.CustomTables;
using CMS.SiteProvider;
using Kadena.Models.ErpSystem;
using Kadena.WebAPI.KenticoProviders.Contracts;

namespace Kadena.WebAPI.KenticoProviders.Providers
{
    public class KenticoErpSystemsProvider : IKenticoErpSystemsProvider
    {
        private const string ErpSystemsCustomTableName = "KDA.ERPSystems";

        private readonly IMapper _mapper;

        public KenticoErpSystemsProvider(IMapper mapper)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public IEnumerable<ErpSystem> GetErpSystems()
        {
            var erpSystems = CustomTableItemProvider.GetItems(ErpSystemsCustomTableName);

            return _mapper.Map<ErpSystem[]>(erpSystems).ToList();
        }
    }
}
