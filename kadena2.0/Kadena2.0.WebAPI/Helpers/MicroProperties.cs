using Kadena.WebAPI.KenticoProviders.Contracts;
using Kadena2.MicroserviceClients.Contracts.Base;

namespace Kadena.WebAPI.Helpers
{
    public class MicroProperties : IMicroProperties
    {
        private readonly IKenticoResourceService _kentico;

        public MicroProperties(IKenticoResourceService kentico)
        {
            _kentico = kentico;
        }

        public string GetCustomerName()
        {
            return _kentico.GetKenticoSite().Name;
        }

        public string GetServiceUrl(string urlLocationName)
        {
            return _kentico.GetSettingsKey(urlLocationName);
        }
    }
}