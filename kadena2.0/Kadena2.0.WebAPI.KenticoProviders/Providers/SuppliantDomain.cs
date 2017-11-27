 using Kadena.WebAPI.KenticoProviders.Contracts;
using Kadena2.MicroserviceClients.Contracts.Base;

namespace Kadena.Helpers
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