using Kadena.WebAPI.KenticoProviders.Contracts;
using Kadena2.MicroserviceClients.Contracts.Base;

namespace Kadena.Helpers
{
    public class SuppliantDomain : ISuppliantDomainClient
    {
        private readonly IKenticoSiteProvider _site;

        public SuppliantDomain(IKenticoSiteProvider site)
        {
            _site = site;
        }

        public string GetSuppliantDomain()
        {
            return _site.GetCurrentSiteDomain();
        }
    }
}