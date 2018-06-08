using Kadena.Dto.EstimateDeliveryPrice.MicroserviceRequests;

namespace Kadena.BusinessLogic.Contracts.Orders
{
    public interface IGetDeliveryEstimationDataService
    {
        AddressDto GetSourceAddressForDeliveryEstimation();
        WeightDto GetWeight();
    }
}
