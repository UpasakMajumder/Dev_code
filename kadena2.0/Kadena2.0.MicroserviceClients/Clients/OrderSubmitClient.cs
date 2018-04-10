using System;
using Kadena.Dto.General;
using Kadena.Dto.SubmitOrder.MicroserviceRequests;
using Kadena2.MicroserviceClients.Clients.Base;
using Kadena2.MicroserviceClients.Contracts;
using System.Threading.Tasks;
using Kadena2.MicroserviceClients.Contracts.Base;

namespace Kadena2.MicroserviceClients.Clients
{
    public sealed class OrderSubmitClient : SignedClientBase, IOrderSubmitClient
    {
        public OrderSubmitClient(IMicroProperties properties)
        {
            _serviceUrlSettingKey = "KDA_OrderServiceEndpoint";
            _properties = properties ?? throw new ArgumentNullException(nameof(properties));
        }

        public async Task<BaseResponseDto<string>> SubmitOrder(OrderDTO orderData)
        {
            var url = $"{BaseUrl}/api/order";
            return await Post<string>(url, orderData).ConfigureAwait(false);
        }
    }
}

