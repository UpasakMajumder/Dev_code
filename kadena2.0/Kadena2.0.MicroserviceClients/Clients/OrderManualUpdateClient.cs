using System;
using Kadena.Dto.General;
using Kadena2.MicroserviceClients.Clients.Base;
using System.Threading.Tasks;
using Kadena2.MicroserviceClients.Contracts.Base;
using Kadena.Models.SiteSettings;
using Kadena2.MicroserviceClients.Contracts;
using Kadena.Dto.OrderManualUpdate.MicroserviceRequests;

namespace Kadena2.MicroserviceClients.Clients
{
    public sealed class OrderManualUpdateClient : SignedClientBase, IOrderManualUpdateClient
    {
        public OrderManualUpdateClient(IMicroProperties properties)
        {
            _properties = properties ?? throw new ArgumentNullException(nameof(properties));
            _serviceVersionSettingKey = Settings.KDA_OrderServiceVersion;
        }

        public async Task<BaseResponseDto<object>> UpdateOrder(OrderManualUpdateRequestDto request)
        {
            var url = $"{BaseUrl}/order/items";
            return await Patch<object>(url, request).ConfigureAwait(false);
        }
    }
}

