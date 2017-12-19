using Kadena2.MicroserviceClients.Clients.Base;
using Kadena2.MicroserviceClients.Contracts;
using System.Threading.Tasks;
using Kadena.Dto.ViewOrder.MicroserviceResponses;
using Kadena.Dto.General;
using Kadena.Dto.Order;
using Kadena2.MicroserviceClients.Contracts.Base;

namespace Kadena2.MicroserviceClients.Clients
{
    public sealed class OrderViewClient : SignedClientBase, IOrderViewClient
    {
        private const string _serviceUrlSettingKey = "KDA_OrderViewServiceUrl";
        private readonly IMicroProperties _properties;

        public OrderViewClient(IMicroProperties properties)
        {
            _properties = properties;
        }

        public async Task<BaseResponseDto<GetOrderByOrderIdResponseDTO>> GetOrderByOrderId(string orderId)
        {
            var url = _properties.GetServiceUrl(_serviceUrlSettingKey);
            url = $"{url}/api/order/{orderId}";
            return await Get<GetOrderByOrderIdResponseDTO>(url).ConfigureAwait(false);
        }

        public async Task<BaseResponseDto<OrderListDto>> GetOrders(int customerId, int pageNumber, int quantity)
        {
            var url = _properties.GetServiceUrl(_serviceUrlSettingKey);
            var parameterizedUrl = $"{url}/api/Order?ClientId={customerId}&pageNumber={pageNumber}&quantity={quantity}";
            return await Get<OrderListDto>(parameterizedUrl).ConfigureAwait(false);
        }

        public async Task<BaseResponseDto<OrderListDto>> GetOrders(string siteName, int pageNumber, int quantity)
        {
            var url = _properties.GetServiceUrl(_serviceUrlSettingKey);
            url = $"{url}/api/Order/forSite?siteName={siteName}&pageNumber={pageNumber}&quantity={quantity}";
            return await Get<OrderListDto>(url).ConfigureAwait(false);
        }

        public async Task<BaseResponseDto<OrderListDto>> GetOrders(string siteName, int pageNumber, int quantity, int campaignID, string orderType)
        {
            var url = _properties.GetServiceUrl(_serviceUrlSettingKey);
            string campaignParameter = campaignID > 0 ? $"&CampaignId={campaignID}" : string.Empty;
            url = $"{url}/api/Order/byquery?siteName={siteName}&pageNumber={pageNumber}&quantity={quantity}&Type={orderType}{campaignParameter}";
            return await Get<OrderListDto>(url).ConfigureAwait(false);
        }

        public async Task<BaseResponseDto<OrderListDto>> GetOrders(int customerId, int pageNumber, int quantity, int campaignID, string orderType)
        {
            var url = _properties.GetServiceUrl(_serviceUrlSettingKey);
            string campaignParameter = campaignID > 0 ? $"&CampaignId={campaignID}" : string.Empty;
            var parameterizedUrl = $"{url}/api/Order/byquery?ClientId={customerId}&pageNumber={pageNumber}&quantity={quantity}&Type={orderType}{campaignParameter}";
            return await Get<OrderListDto>(parameterizedUrl).ConfigureAwait(false);
        }
    }
}

