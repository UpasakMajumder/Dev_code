using Kadena.WebAPI.Contracts;
using Kadena.WebAPI.KenticoProviders.Contracts;

namespace Kadena.WebAPI.Services
{
    public class SiteDataService : ISiteDataService
    {
        private readonly IKenticoResourceService _kentico;

        public SiteDataService(IKenticoResourceService kentico)
        {
            _kentico = kentico;
        }

        public string GetOrderInfoRecepients(string siteName)
        {
            return _kentico.GetSettingsKey(siteName ?? string.Empty, "KDA_OrderNotificationEmail");
        }
    }
}