using Kadena.Dto.General;
using Kadena.Dto.Order;
using Kadena.Dto.ViewOrder.MicroserviceResponses;
using Kadena2.MicroserviceClients.Clients.Base;
using Kadena2.MicroserviceClients.Contracts;
using Kadena2.MicroserviceClients.Contracts.Base;
using System.Linq;
using System.Threading.Tasks;

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

        public async Task<BaseResponseDto<OrderListDto>> GetOrders(OrderListFilter filter)
        {
            var args = string.Join("&", new[]
            {
                filter.CustomerId > 0 ? $"clientId={filter.CustomerId}" : string.Empty,
                filter.CampaignId > 0 ? $"campaignId={filter.CampaignId}" : string.Empty,
                filter.ProgramId > 0 ? $"programId={filter.ProgramId}" : string.Empty,
                filter.DistributorId > 0 ? $"distributorId={filter.DistributorId}" : string.Empty,
                !string.IsNullOrWhiteSpace(filter.OrderType) ? $"tp={filter.OrderType}" : string.Empty,
                !string.IsNullOrWhiteSpace(filter.SortBy) ? $"sort={filter.SortBy}" : string.Empty,
                filter.SortDescending ? "sortDesc=true" : string.Empty,
                filter.DateFrom != null ? $"dateFrom={filter.DateFrom.Value.ToString(DateArgumentFormat)}" : string.Empty,
                filter.DateTo != null ? $"dateTo={filter.DateTo.Value.ToString(DateArgumentFormat)}" : string.Empty,
                !string.IsNullOrWhiteSpace(filter.SiteName) ? $"siteName={filter.SiteName}" : string.Empty,
                filter.PageNumber >= 0 ? $"pageNumber={filter.PageNumber}" : string.Empty,
                filter.ItemsPerPage > 0 ? $"quantity={filter.ItemsPerPage}" : string.Empty
            }.Where(p => p != string.Empty));

            var parameterizedUrl = $"{BaseUrl}/api/Order?{args}";
            
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

