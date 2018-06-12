using Kadena.Dto.EstimateDeliveryPrice.MicroserviceRequests;

namespace Kadena.BusinessLogic.Contracts.Orders
{
    public interface IDeliveryEstimationDataService
    {
        AddressDto GetSourceAddress();
        WeightDto GetWeightInSiteUnit(decimal weight);
    }
}
