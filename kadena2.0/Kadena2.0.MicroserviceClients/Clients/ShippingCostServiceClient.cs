using Kadena.Dto.EstimateDeliveryPrice.MicroserviceRequests;
using Kadena.Dto.EstimateDeliveryPrice.MicroserviceResponses;
using Kadena.Dto.General;
using Kadena2.MicroserviceClients.Clients.Base;
using Kadena2.MicroserviceClients.Contracts;
using Kadena2.MicroserviceClients.Contracts.Base;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Kadena2.MicroserviceClients.Clients
{
    public class ShippingCostServiceClient : ClientBase, IShippingCostServiceClient
    {
        private const string _serviceUrlSettingKey = "KDA_ShippingCostServiceUrl";
        private readonly IMicroProperties _properties;

        public ShippingCostServiceClient(IMicroProperties properties)
        {
            _properties = properties;
        }

        public string GetRequestString(EstimateDeliveryPriceRequestDto request)
        {
            return JsonConvert.SerializeObject(request, camelCaseSerializer);
        }

        public async Task<BaseResponseDto<EstimateDeliveryPricePayloadDto>> EstimateShippingCost(string requestBody)
        {
            var url = _properties.GetServiceUrl(_serviceUrlSettingKey);
            url = $"{url}/api/shippingcost";
            using (var httpClient = new HttpClient())
            {
                using (var request = new HttpRequestMessage(HttpMethod.Post, url))
                {
                    request.Content = new StringContent(requestBody, Encoding.UTF8, "application/json");

                    using (var response = await httpClient.SendAsync(request).ConfigureAwait(false))
                    {
                        return await ReadResponseJson<EstimateDeliveryPricePayloadDto>(response).ConfigureAwait(false);
                    }
                }
            }
        }
    }
}
