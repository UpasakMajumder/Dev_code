using Kadena.Dto.EstimateDeliveryPrice.MicroserviceRequests;

namespace Kadena.BusinessLogic.Contracts.Orders
{
    public interface IDeliveryEstimationDataService
    {
        AddressDto GetSourceAddress();
        WeightDto GetWeightInSiteUnit(decimal weight);
        EstimateDeliveryPriceRequestDto[] GetDeliveryEstimationRequestData(string provider, string service, decimal weight, AddressDto source, AddressDto target);
    }
}
