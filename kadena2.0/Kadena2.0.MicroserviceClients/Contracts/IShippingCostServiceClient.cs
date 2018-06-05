using Kadena.Dto.EstimateDeliveryPrice.MicroserviceRequests;
using Kadena.Dto.EstimateDeliveryPrice.MicroserviceResponses;
using Kadena.Dto.General;
using System.Threading.Tasks;

namespace Kadena2.MicroserviceClients.Contracts
{
    public interface IShippingCostServiceClient
    {
        string GetRequestString(EstimateDeliveryPriceRequestDto[] request);
        Task<BaseResponseDto<EstimateDeliveryPricePayloadDto[]>> EstimateShippingCost(EstimateDeliveryPriceRequestDto[] requestBody);
    }
}
