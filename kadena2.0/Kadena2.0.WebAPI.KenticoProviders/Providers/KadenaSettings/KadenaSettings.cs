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
            if (resources == null)
            {
                throw new ArgumentNullException(nameof(resources));
            }

            this.resources = resources;
        }

        public string DefaultSiteCompanyName => resources.GetSettingsKey("KDA_CustomerFullName");
        public string DefaultSitePersonalName => resources.GetSettingsKey("KDA_CustomerPersonalName");
        public string DefaultCustomerCompanyName => resources.GetSettingsKey("KDA_ShippingAddress_DefaultCompanyName");
        public string TermsAndConditionsPage => resources.GetSettingsKey("KDA_TermsAndConditionPage");
        public string ErpCustomerId => resources.GetSettingsKey("KDA_ErpCustomerId");
        public string OrderNotificationEmail => resources.GetSettingsKey("KDA_OrderNotificationEmail");

        public bool FTPArtworkEnabled(string siteName)
        {
            return resources.GetSettingsKey(siteName, "KDA_FTPAW_Enabled").ToLower() == "true";
        }

        public string FTPArtworkUrl(string siteName)
        {
            return resources.GetSettingsKey(siteName, "KDA_FTPAW_Url");
        }

        public string FTPArtworkUsername(string siteName)
        {
            return resources.GetSettingsKey(siteName, "KDA_FTPAW_Username");
        }

        public string FTPArtworkPassword(string siteName)
        {
            return resources.GetSettingsKey(siteName, "KDA_FTPAW_Password");
        }
    }
}
