using CMS.DataEngine;
using CMS.SiteProvider;
using Kadena2.MicroserviceClients.Contracts.Base;

namespace Kadena2.Carriers.Helpers
{
    // This class was created to not add reference to WebApi project.
    class MicroProperties : IMicroProperties
    {
        public string GetCustomerName()
        {
            return SiteContext.CurrentSiteName;
        }

        public string GetServiceUrl(string urlLocationName)
        {
            return SettingsKeyInfoProvider.GetValue($"{SiteContext.CurrentSiteName}.{urlLocationName}");
        }
    }
}
