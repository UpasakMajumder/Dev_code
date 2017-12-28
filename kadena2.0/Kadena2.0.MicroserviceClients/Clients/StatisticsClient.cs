using Kadena2.MicroserviceClients.Contracts;
using System.Threading.Tasks;
using Kadena.Dto.General;
using Kadena.Dto.Order;
using System.Net.Http;
using Kadena2.MicroserviceClients.Clients.Base;
using Kadena2.MicroserviceClients.Contracts.Base;

namespace Kadena2.MicroserviceClients.Clients
{
    public sealed class StatisticsClient : SignedClientBase, IStatisticsClient
    {
        private const string _serviceUrlSettingKey = "KDA_OrderStatisticsServiceEndpoint";
        private readonly IMicroProperties _properties;

        public StatisticsClient(IMicroProperties properties)
        {
            _properties = properties;
        }

        public async Task<BaseResponseDto<OrderStatisticDto>> GetOrderStatistics()
        {
            var url = _properties.GetServiceUrl(_serviceUrlSettingKey);
            return await Get<OrderStatisticDto>($"{url}/api/OrderStats?customerName={_properties.GetCustomerName()}").ConfigureAwait(false);
        }
    }
}
