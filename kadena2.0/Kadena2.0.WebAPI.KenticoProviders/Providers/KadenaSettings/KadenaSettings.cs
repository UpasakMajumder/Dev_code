using Kadena.WebAPI.KenticoProviders.Contracts;
using Kadena2.WebAPI.KenticoProviders.Contracts.KadenaSettings;
using System;

namespace Kadena2.WebAPI.KenticoProviders.Providers.KadenaSettings
{
    public class KadenaSettings : IKadenaSettings
    {
        private readonly IKenticoResourceService resources;

        public KadenaSettings(IKenticoResourceService resources)
        {
            this.resources = resources ?? throw new ArgumentNullException(nameof(resources));
        }

        public string DefaultSiteCompanyName => resources.GetSiteSettingsKey("KDA_CustomerFullName");
        public string DefaultSitePersonalName => resources.GetSiteSettingsKey("KDA_CustomerPersonalName");
        public string DefaultCustomerCompanyName => resources.GetSiteSettingsKey("KDA_ShippingAddress_DefaultCompanyName");
        public string TermsAndConditionsPage => resources.GetSiteSettingsKey("KDA_TermsAndConditionPage");
        public string CheckoutPageUrl => resources.GetSiteSettingsKey("KDA_CheckoutPageUrl");
        public string ErpCustomerId => resources.GetSiteSettingsKey("KDA_ErpCustomerId");
        public string OrderNotificationEmail => resources.GetSiteSettingsKey("KDA_OrderNotificationEmail");

        public bool FTPArtworkEnabled(int siteId)
        {
            return resources.GetSettingsKey<bool>("KDA_FTPAW_Enabled", siteId);
        }

        public string FTPArtworkUrl(int siteId)
        {
            return resources.GetSettingsKey<string>("KDA_FTPAW_Url", siteId);
        }

        public string FTPArtworkUsername(int siteId)
        {
            return resources.GetSettingsKey<string>("KDA_FTPAW_Username", siteId);
        }

        public string FTPArtworkPassword(int siteId)
        {
            return resources.GetSettingsKey<string>("KDA_FTPAW_Password", siteId);
        }
    }
}
