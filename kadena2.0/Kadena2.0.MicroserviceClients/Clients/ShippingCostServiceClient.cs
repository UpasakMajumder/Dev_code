using Kadena.Dto.EstimateDeliveryPrice.MicroserviceRequests;
using Kadena.Dto.EstimateDeliveryPrice.MicroserviceResponses;
using Kadena.Dto.General;
using Kadena.Models.SiteSettings;
using Kadena2.MicroserviceClients.Clients.Base;
using Kadena2.MicroserviceClients.Contracts;
using Kadena2.MicroserviceClients.Contracts.Base;
using System;
using System.Threading.Tasks;

namespace Kadena2.MicroserviceClients.Clients
{
    public sealed class ShippingCostServiceClient : SignedClientBase, IShippingCostServiceClient
    {
        public ShippingCostServiceClient(IMicroProperties properties)
        {
            _properties = properties ?? throw new ArgumentNullException(nameof(properties));
            _serviceVersionSettingKey = Settings.KDA_ShippingCostServiceVersion;
        }

        public string GetRequestString(EstimateDeliveryPriceRequestDto request)
        {
            return SerializeRequestContent(request);
        }

        public async Task<BaseResponseDto<EstimateDeliveryPricePayloadDto>> EstimateShippingCost(EstimateDeliveryPriceRequestDto requestBody)
        {
            var url = $"{BaseUrl}/shippingcosts";
            return await Post<EstimateDeliveryPricePayloadDto>(url, requestBody).ConfigureAwait(false);
        }
    }
}
