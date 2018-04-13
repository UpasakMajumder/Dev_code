using CMS.DataEngine;
using CMS.Ecommerce;
using CMS.SiteProvider;
using Kadena.Dto.EstimateDeliveryPrice.MicroserviceRequests;
using System.Collections.Generic;
using System.Linq;

namespace Kadena2.Carriers
{
    class EstimatePriceRequestFactory
    {
        public EstimateDeliveryPriceRequestDto[] Create(Delivery delivery, string provider, string service)
        {
            return new[]{
                new EstimateDeliveryPriceRequestDto()
                {
                    Provider = provider,
                    ProviderService = service.Replace("#", ""), // hack to solve non-unique service keys
                    SourceAddress = GetSourceAddressFromConfig(),
                    TargetAddress = GetAddress(delivery.DeliveryAddress),
                    Weight = new WeightDto() { Unit = "Lb", Value = (double)delivery.Weight }
                }
            };
        }

        private AddressDto GetSourceAddressFromConfig()
        {
            var addressLines = new[]
            {
                SettingsKeyInfoProvider.GetValue(SiteContext.CurrentSiteName + ".KDA_EstimateDeliveryPrice_SenderAddressLine1"),
                SettingsKeyInfoProvider.GetValue(SiteContext.CurrentSiteName + ".KDA_EstimateDeliveryPrice_SenderAddressLine2")
            }.Where(a => !string.IsNullOrWhiteSpace(a)).ToList();

            return new AddressDto()
            {
                City = SettingsKeyInfoProvider.GetValue(SiteContext.CurrentSiteName + ".KDA_EstimateDeliveryPrice_SenderCity"),
                Country = SettingsKeyInfoProvider.GetValue(SiteContext.CurrentSiteName + ".KDA_EstimateDeliveryPrice_SenderCountry"),
                Postal = SettingsKeyInfoProvider.GetValue(SiteContext.CurrentSiteName + ".KDA_EstimateDeliveryPrice_SenderPostal"),
                State = SettingsKeyInfoProvider.GetValue(SiteContext.CurrentSiteName + ".KDA_EstimateDeliveryPrice_SenderState"),
                StreetLines = addressLines
            };
        }

        private AddressDto GetAddress(IAddress address)
        {
            if (address == null)
                return new AddressDto();

            return new AddressDto()
            {
                City = address.AddressCity,
                Country = address.GetCountryTwoLetterCode(),
                Postal = address.AddressZip,
                State = address.GetStateCode(),
                StreetLines = new List<string> { address.AddressLine1, address.AddressLine2 }
            };
        }
    }
}
