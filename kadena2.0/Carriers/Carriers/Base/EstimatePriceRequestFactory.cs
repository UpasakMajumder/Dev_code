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
            var estimationData = DIContainer.Resolve<IDeliveryEstimationDataService>();
            var targetAddress = GetAddress(delivery.DeliveryAddress);
            return estimationData.GetDeliveryEstimationRequestData(provider, service, delivery.Weight, targetAddress);
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
