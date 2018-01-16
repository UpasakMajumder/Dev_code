using CMS.DataEngine;
using Kadena2.MicroserviceClients.Contracts.Base;

namespace Kadena.ScheduledTasks.DeleteExpiredMailingLists
{
    public class StaticMicroProperties : IMicroProperties
    {
        private readonly string site;

        public StaticMicroProperties(string site)
        {
            this.site = site;
        }

        public string GetCustomerName()
        {
            return site;
        }

        public string GetServiceUrl(string urlLocationName)
        {
            string resourceKey = $"{site}.{urlLocationName}";
            return SettingsKeyInfoProvider.GetValue(resourceKey).TrimEnd('/');
        }
    }
}
