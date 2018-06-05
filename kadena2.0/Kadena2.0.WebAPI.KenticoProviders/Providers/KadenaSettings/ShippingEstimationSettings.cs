using Kadena.WebAPI.KenticoProviders.Contracts;
using Kadena2.WebAPI.KenticoProviders.Contracts.KadenaSettings;
using System;

namespace Kadena2.WebAPI.KenticoProviders.Providers.KadenaSettings
{
    public class ShippingEstimationSettings : IShippingEstimationSettings
    {
        private readonly IKenticoResourceService resources;

        public ShippingEstimationSettings(IKenticoResourceService resources)
        {
            if (resources == null)
            {
                throw new ArgumentNullException(nameof(resources));
            }

            this.resources = resources;
        }

        public string SenderAddressLine1 => resources.GetSiteSettingsKey("KDA_EstimateDeliveryPrice_SenderAddressLine1");

        public string SenderAddressLine2 => resources.GetSiteSettingsKey("KDA_EstimateDeliveryPrice_SenderAddressLine2");

        public string SenderCountry => resources.GetSiteSettingsKey("KDA_EstimateDeliveryPrice_SenderCountry");

        public string SenderState => resources.GetSiteSettingsKey("KDA_EstimateDeliveryPrice_SenderState");

        public string SenderCity => resources.GetSiteSettingsKey("KDA_EstimateDeliveryPrice_SenderCity");

        public string SenderPostal => resources.GetSiteSettingsKey("KDA_EstimateDeliveryPrice_SenderPostal");
    }
}
