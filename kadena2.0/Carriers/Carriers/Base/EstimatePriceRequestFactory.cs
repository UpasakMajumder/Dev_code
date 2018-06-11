using CMS.Ecommerce;
using Kadena.BusinessLogic.Contracts.Orders;
using Kadena.Container.Default;
using Kadena.Dto.EstimateDeliveryPrice.MicroserviceRequests;
using System.Collections.Generic;

namespace Kadena2.Carriers
{
    class EstimatePriceRequestFactory
    {
        public EstimateDeliveryPriceRequestDto[] Create(Delivery delivery, string provider, string service)
        {
            var estimationData = DIContainer.Resolve<IGetDeliveryEstimationDataService>();

            return new[]{
                new EstimateDeliveryPriceRequestDto()
                {
                    Provider = provider,
                    ProviderService = service.Replace("#", ""), // hack to solve non-unique service keys
                    SourceAddress = estimationData.GetSourceAddressForDeliveryEstimation(),
                    TargetAddress = GetAddress(delivery.DeliveryAddress),
                    Weight = estimationData.GetWeightInSiteUnit(delivery.Weight)
                }
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
