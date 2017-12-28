using Kadena.WebAPI.KenticoProviders.Contracts;
using Kadena2.MicroserviceClients.Contracts.Base;
using System;

namespace Kadena.Helpers
{
    public class MicroProperties : IMicroProperties
    {
        private readonly IKenticoResourceService _kentico;

        public MicroProperties(IKenticoResourceService kentico)
        {
            if (kentico == null)
            {
                throw new ArgumentNullException(nameof(kentico));
            }

            _kentico = kentico;
        }

        public string GetCustomerName()
        {
            return _kentico.GetKenticoSite().Name;
        }

        public string GetServiceUrl(string urlLocationName)
        {
            return _kentico.GetSettingsKey(urlLocationName).TrimEnd('/');
        }
    }
}