using Kadena.WebAPI.KenticoProviders.Contracts;
using Kadena2.MicroserviceClients.Contracts.Base;
using System;

namespace Kadena.Helpers
{
    public class MicroProperties : IMicroProperties
    {
        private readonly IKenticoResourceService _kentico;
        private readonly IKenticoSiteProvider _site;

        public MicroProperties(IKenticoResourceService kentico, IKenticoSiteProvider site)
        {
            _kentico = kentico ?? throw new ArgumentNullException(nameof(kentico));
            _site = site ?? throw new ArgumentNullException(nameof(site));
        }

        public string GetCustomerName()
        {
            return _site.GetKenticoSite()?.Name;
        }

        public string GetServiceUrl(string urlLocationName)
        {
            return _kentico.GetSiteSettingsKey(urlLocationName).TrimEnd('/');
        }
    }
}