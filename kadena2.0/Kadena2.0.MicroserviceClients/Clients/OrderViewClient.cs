using Kadena2.MicroserviceClients.Clients.Base;
using Kadena2.MicroserviceClients.Contracts;
using System.Threading.Tasks;
using Kadena.Dto.ViewOrder.MicroserviceResponses;
using Kadena.Dto.General;
using Kadena.Dto.Order;
using Kadena2.MicroserviceClients.Contracts.Base;
using System;
using System.Linq;

namespace Kadena2.MicroserviceClients.Clients
{
    public sealed class OrderViewClient : SignedClientBase, IOrderViewClient
    {
        public const string DateArgumentFormat = "yyyy-MM-dd";
        private const string _serviceUrlSettingKey = "KDA_OrderViewServiceUrl";
        private readonly IMicroProperties _properties;

        public string BaseUrl => _properties.GetServiceUrl(_serviceUrlSettingKey);

        public OrderViewClient(IMicroProperties properties)
        {
            _properties = properties;
        }

        public async Task<BaseResponseDto<GetOrderByOrderIdResponseDTO>> GetOrderByOrderId(string orderId)
        {
            var url = $"{BaseUrl}/api/order/{orderId}";
            return await Get<GetOrderByOrderIdResponseDTO>(url).ConfigureAwait(false);
        }

        public async Task<BaseResponseDto<OrderListDto>> GetOrders(string siteName, int? customerId, int? pageNumber, int? itemsPerPage, 
            DateTime? dateFrom, DateTime? dateTo, string sortBy, bool sortDescending, int? campaignId, string orderType)
        {
            var orderTypeParam = !string.IsNullOrWhiteSpace(orderType) ? $"tp={orderType}" : string.Empty;
            var campaignParam = campaignId > 0 ? $"CampaignId={campaignId}" : string.Empty;
            var sortDescParam = sortDescending ? "sortDesc=true" : string.Empty;
            var sortByParam = !string.IsNullOrWhiteSpace(sortBy) ? $"sort={sortBy}" : string.Empty;
            var dateToParam = dateTo != null ? $"dateTo={dateTo.Value.ToString(DateArgumentFormat)}" : string.Empty;
            var dateFromParam = dateFrom != null ? $"dateFrom={dateFrom.Value.ToString(DateArgumentFormat)}" : string.Empty;
            var itemsPerPageParam = itemsPerPage > 0 ? $"quantity={itemsPerPage}" : string.Empty;
            var pageNumberParam = pageNumber >= 0 ? $"pageNumber={pageNumber}" : string.Empty;
            var customerParam = customerId > 0 ? $"ClientId={customerId}" : string.Empty;
            var siteParam = !string.IsNullOrWhiteSpace(siteName) ? $"siteName={siteName}" : string.Empty;

            var args = string.Join("&", new[]
            {
                orderTypeParam,
                campaignParam,
                sortDescParam,
                sortByParam,
                dateToParam,
                dateFromParam,
                itemsPerPageParam,
                pageNumberParam,
                customerParam,
                siteParam
            }.Where(p => p != string.Empty));

            var parameterizedUrl = $"{BaseUrl}/api/Order/byquery?{args}";
            
            return await Get<OrderListDto>(parameterizedUrl).ConfigureAwait(false);
        }

        public async Task<BaseResponseDto<OrderListDto>> GetOrders(int customerId, int pageNumber, int quantity)
        {
            var parameterizedUrl = $"{BaseUrl}/api/Order?ClientId={customerId}&pageNumber={pageNumber}&quantity={quantity}";
            return await Get<OrderListDto>(parameterizedUrl).ConfigureAwait(false);
        }

        public async Task<BaseResponseDto<OrderListDto>> GetOrders(string siteName, int pageNumber, int quantity)
        {
            var url = $"{BaseUrl}/api/Order/forSite?siteName={siteName}&pageNumber={pageNumber}&quantity={quantity}";
            return await Get<OrderListDto>(url).ConfigureAwait(false);
        }

        public async Task<BaseResponseDto<OrderListDto>> GetOrders(string siteName, int pageNumber, int quantity, int campaignID, string orderType)
        {
            string campaignParameter = campaignID > 0 ? $"&CampaignId={campaignID}" : string.Empty;
            var url = $"{BaseUrl}/api/Order/byquery?siteName={siteName}&pageNumber={pageNumber}&quantity={quantity}&type={orderType}{campaignParameter}";
            return await Get<OrderListDto>(url).ConfigureAwait(false);
        }

        public async Task<BaseResponseDto<OrderListDto>> GetOrders(int customerId, int pageNumber, int quantity, int campaignID, string orderType)
        {
            string campaignParameter = campaignID > 0 ? $"&CampaignId={campaignID}" : string.Empty;
            var parameterizedUrl = $"{BaseUrl}/api/Order/byquery?ClientId={customerId}&pageNumber={pageNumber}&quantity={quantity}&type={orderType}{campaignParameter}";
            return await Get<OrderListDto>(parameterizedUrl).ConfigureAwait(false);
        }
    }
}

