using CMS.DataEngine;
using CMS.Ecommerce;
using CMS.SiteProvider;
using Kadena2.Carriers.ServiceApi;
using System.Collections.Generic;
using System.Linq;

namespace Kadena2.Carriers
{
    class EstimatePriceRequestFactory
    {
        public EstimateDeliveryPriceRequest Create(Delivery delivery, string provider, string service)
        {
            return new EstimateDeliveryPriceRequest()
            {
                provider = provider,
                providerService = service.Replace("#", ""), // hack to solve non-unique service keys
                sourceAddress = GetSourceAddressFromConfig(),
                targetAddress = GetAddress(delivery.DeliveryAddress),
                weight = new Weight() { unit = "Lb", value = (double)delivery.Weight }
            };
        }

        private Address GetSourceAddressFromConfig()
        {
            var addressLines = new[]
            {
                SettingsKeyInfoProvider.GetValue(SiteContext.CurrentSiteName + ".KDA_EstimateDeliveryPrice_SenderAddressLine1"),
                SettingsKeyInfoProvider.GetValue(SiteContext.CurrentSiteName + ".KDA_EstimateDeliveryPrice_SenderAddressLine2")
            }.Where(a => !string.IsNullOrWhiteSpace(a)).ToList();

            return new Address()
            {
                city = SettingsKeyInfoProvider.GetValue(SiteContext.CurrentSiteName + ".KDA_EstimateDeliveryPrice_SenderCity"),
                country = SettingsKeyInfoProvider.GetValue(SiteContext.CurrentSiteName + ".KDA_EstimateDeliveryPrice_SenderCountry"),
                postal = SettingsKeyInfoProvider.GetValue(SiteContext.CurrentSiteName + ".KDA_EstimateDeliveryPrice_SenderPostal"),
                state = SettingsKeyInfoProvider.GetValue(SiteContext.CurrentSiteName + ".KDA_EstimateDeliveryPrice_SenderState"),
                streetLines = addressLines
            };
        }

        private Address GetAddress(IAddress address)
        {
            if (address == null)
                return new Address();

            return new Address()
            {
                city = address.AddressCity,
                country = address.GetCountryTwoLetterCode(),
                postal = address.AddressZip,
                state = address.GetStateCode(),
                streetLines = new List<string> { address.AddressLine1, address.AddressLine2 }
            };
        }
    }
}
