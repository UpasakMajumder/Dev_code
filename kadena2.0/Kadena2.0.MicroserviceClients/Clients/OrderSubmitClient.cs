using Kadena.Dto.General;
using Kadena.Dto.SubmitOrder.MicroserviceRequests;
using Kadena2.MicroserviceClients.Clients.Base;
using Kadena2.MicroserviceClients.Contracts;
using System.Threading.Tasks;
using Kadena2.MicroserviceClients.Contracts.Base;

namespace Kadena2.MicroserviceClients.Clients
{
    public class OrderSubmitClient : ClientBase, IOrderSubmitClient
    {
        private const string _serviceUrlSettingKey = "KDA_OrderServiceEndpoint";
        private readonly IMicroProperties _properties;

        public OrderSubmitClient(IMicroProperties properties)
        {
            _properties = properties;
        }

        public async Task<BaseResponseDto<string>> FinishOrder(string orderNumber)
        {
            // TODO: Was timeout = 60s here, I think useless
            var url = _properties.GetServiceUrl(_serviceUrlSettingKey);
            url = $"{url}/api/order";
            return await Patch<string>(url, orderNumber).ConfigureAwait(false);
        }

        public async Task<BaseResponseDto<string>> SubmitOrder(OrderDTO orderData)
        {
            // TODO: Was timeout = 60s here, I think useless
            var url = _properties.GetServiceUrl(_serviceUrlSettingKey);
            url = $"{url}/api/order";
            return await Post<string>(url, orderData).ConfigureAwait(false);
        }
    }
}

