using System;
using System.Collections.Generic;
using Kadena.BusinessLogic.Contracts;
using Kadena.Models.ErpSystem;
using Kadena.WebAPI.KenticoProviders.Contracts;

namespace Kadena.BusinessLogic.Services
{
    public class ErpSystemsService : IErpSystemsService
    {
        private readonly IKenticoErpSystemsProvider _kenticoErpSystemsProvider;

        public ErpSystemsService(IKenticoErpSystemsProvider kenticoErpSystemsProvider)
        {
            _kenticoErpSystemsProvider = kenticoErpSystemsProvider ?? throw new ArgumentNullException(nameof(kenticoErpSystemsProvider));
        }

        public IEnumerable<ErpSystem> GetAll()
        {
            return _kenticoErpSystemsProvider.GetErpSystems();
        }
    }
}
