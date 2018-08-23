using System;
using Kadena.Dto.General;
using Kadena.Dto.SubmitOrder.MicroserviceRequests;
using Kadena2.MicroserviceClients.Clients.Base;
using Kadena2.MicroserviceClients.Contracts;
using System.Threading.Tasks;
using Kadena2.MicroserviceClients.Contracts.Base;
using Kadena.Models.SiteSettings;

namespace Kadena2.MicroserviceClients.Clients
{
    public sealed class OrderSubmitClient : SignedClientBase, IOrderSubmitClient
    {
        public OrderSubmitClient(IMicroProperties properties)
        {
            _properties = properties ?? throw new ArgumentNullException(nameof(properties));
            _serviceVersionSettingKey = Settings.KDA_OrderServiceVersion;
        }

        public async Task<BaseResponseDto<string>> SubmitOrder(OrderDTO orderData)
        {
            var url = $"{BaseUrl}/api/v{Version}/order";
            return await Post<string>(url, orderData).ConfigureAwait(false);
        }
    }
}

