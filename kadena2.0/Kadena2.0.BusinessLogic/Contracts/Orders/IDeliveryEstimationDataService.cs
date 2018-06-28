using Kadena.Dto.EstimateDeliveryPrice.MicroserviceRequests;

namespace Kadena.BusinessLogic.Contracts.Orders
{
    public interface IDeliveryEstimationDataService
    {
        AddressDto GetSourceAddress();
        EstimateDeliveryPriceRequestDto[] GetDeliveryEstimationRequestData(string provider, string service, decimal weight, AddressDto target);
    }
}
