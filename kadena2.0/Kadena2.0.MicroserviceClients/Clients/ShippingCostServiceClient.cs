using Kadena.Dto.EstimateDeliveryPrice.MicroserviceRequests;
using Kadena.Dto.EstimateDeliveryPrice.MicroserviceResponses;
using Kadena.Dto.General;
using Kadena2.MicroserviceClients.Clients.Base;
using Kadena2.MicroserviceClients.Contracts;
using Kadena2.MicroserviceClients.Contracts.Base;
using System.Threading.Tasks;

namespace Kadena2.MicroserviceClients.Clients
{
    public sealed class ShippingCostServiceClient : SignedClientBase, IShippingCostServiceClient
    {
        private const string _serviceUrlSettingKey = "KDA_ShippingCostServiceUrl";
        private readonly IMicroProperties _properties;

        public ShippingCostServiceClient(IMicroProperties properties)
        {
            _properties = properties;
        }

        public string GetRequestString(EstimateDeliveryPriceRequestDto request)
        {
            return SerializeRequestContent(request);
        }

        public async Task<BaseResponseDto<EstimateDeliveryPricePayloadDto>> EstimateShippingCost(EstimateDeliveryPriceRequestDto requestBody)
        {
            var url = _properties.GetServiceUrl(_serviceUrlSettingKey);
            url = $"{url}/api/shippingcosts";
            return await Post<EstimateDeliveryPricePayloadDto>(url, requestBody).ConfigureAwait(false);
        }
    }
}
