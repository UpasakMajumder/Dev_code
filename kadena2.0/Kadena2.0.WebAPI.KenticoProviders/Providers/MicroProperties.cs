using Kadena.WebAPI.KenticoProviders.Contracts;
using Kadena2.MicroserviceClients.Contracts.Base;
using System;

namespace Kadena.Helpers
{
    public class MicroProperties : IMicroProperties
    {
        private readonly IKenticoResourceService _resources;
        private readonly IKenticoSiteProvider _site;

        public MicroProperties(IKenticoResourceService resources, IKenticoSiteProvider site)
        {
            _resources = resources ?? throw new ArgumentNullException(nameof(resources));
            _site = site ?? throw new ArgumentNullException(nameof(site));
        }       
        public string GetCustomerName()
        {
            return _site.GetKenticoSite()?.Name;
        }

        public string GetServiceUrl(string urlLocationName)
        {
            return _resources.GetSiteSettingsKey(urlLocationName).TrimEnd('/');
        }

        public int GetApiVersion(string apiVersionKeyName)
        {
            return _resources.GetSiteSettingsKey<int>(apiVersionKeyName);
        }
    }
}