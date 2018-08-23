using Kadena2.MicroserviceClients.Contracts;
using System.Threading.Tasks;
using Kadena.Dto.General;
using Kadena.Dto.Order;
using Kadena2.MicroserviceClients.Clients.Base;
using Kadena2.MicroserviceClients.Contracts.Base;
using System;
using Kadena.Models.SiteSettings;

namespace Kadena2.MicroserviceClients.Clients
{
    public sealed class StatisticsClient : SignedClientBase, IStatisticsClient
    {
        public StatisticsClient(IMicroProperties properties)
        {
            _serviceVersionSettingKey = Settings.KDA_StatisticsServiceVersion;
            _properties = properties ?? throw new ArgumentNullException(nameof(properties));
        }

        public async Task<BaseResponseDto<OrderStatisticDto>> GetOrderStatistics()
        {
            var url = $"{BaseUrl}/api/v{Version}/statistics/orders?customerName={_properties.GetCustomerName()}";
            return await Get<OrderStatisticDto>(url).ConfigureAwait(false);
        }
    }
}
