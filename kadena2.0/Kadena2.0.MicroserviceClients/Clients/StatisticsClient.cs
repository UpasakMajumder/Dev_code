using Kadena2.MicroserviceClients.Contracts;
using System.Threading.Tasks;
using Kadena.Dto.General;
using Kadena.Dto.Order;
using Kadena2.MicroserviceClients.Clients.Base;
using Kadena2.MicroserviceClients.Contracts.Base;
using System;

namespace Kadena2.MicroserviceClients.Clients
{
    public sealed class StatisticsClient : SignedClientBase, IStatisticsClient
    {
        public StatisticsClient(IMicroProperties properties)
        {
            _serviceUrlSettingKey = "KDA_OrderStatisticsServiceEndpoint";
            _properties = properties ?? throw new ArgumentNullException(nameof(properties));
        }

        public async Task<BaseResponseDto<OrderStatisticDto>> GetOrderStatistics()
        {
            var url = $"{BaseUrlOld}/api/OrderStats?customerName={_properties.GetCustomerName()}";
            return await Get<OrderStatisticDto>(url).ConfigureAwait(false);
        }
    }
}
