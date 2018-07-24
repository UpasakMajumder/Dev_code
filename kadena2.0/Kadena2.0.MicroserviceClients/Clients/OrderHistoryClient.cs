using Kadena.Dto.General;
using Kadena.Dto.ViewOrder.MicroserviceResponses.History;
using Kadena.Models.SiteSettings;
using Kadena2.MicroserviceClients.Clients.Base;
using Kadena2.MicroserviceClients.Contracts;
using Kadena2.MicroserviceClients.Contracts.Base;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Kadena2.MicroserviceClients.Clients
{
    public class OrderHistoryClient : SignedClientBase, IOrderHistoryClient
    {
        private const string _serviceEndpoint = "order/history";
        public OrderHistoryClient(IMicroProperties properties)
        {
            _properties = properties ?? throw new ArgumentNullException(nameof(properties));
            _serviceVersionSettingKey = Settings.KDA_OrderServiceVersion;
        }

        public async Task<BaseResponseDto<List<OrderHistoryUpdateDto>>> Get(string orderId)
        {
            var url = $"{BaseUrl}/{_serviceEndpoint}/{orderId}";
            return await Get<List<OrderHistoryUpdateDto>>(url)
                .ConfigureAwait(false);
        }
    }
}
