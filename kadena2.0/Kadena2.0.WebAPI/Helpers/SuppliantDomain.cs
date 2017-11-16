using Kadena.WebAPI.KenticoProviders.Contracts;
using Kadena2.MicroserviceClients.Contracts.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kadena.WebAPI.Helpers
{
    public class SuppliantDomain : ISuppliantDomainClient
    {
        private readonly IKenticoResourceService _kentico;

        public SuppliantDomain(IKenticoResourceService kentico)
        {
            _kentico = kentico;
        }

        public string GetSuppliantDomain()
        {
            return _kentico.GetCurrentSiteDomain();
        }
    }
}