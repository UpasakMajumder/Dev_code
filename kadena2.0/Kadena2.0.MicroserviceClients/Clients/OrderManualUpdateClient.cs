using System;
using Kadena.Dto.General;
using Kadena2.MicroserviceClients.Clients.Base;
using System.Threading.Tasks;
using Kadena2.MicroserviceClients.Contracts.Base;
using Kadena.Models.SiteSettings;
using Kadena2.MicroserviceClients.Contracts;
using Kadena.Dto.OrderManualUpdate.MicroserviceRequests;
using Kadena.Dto.ViewOrder.MicroserviceResponses.History;
using System.Collections.Generic;
using Kadena.Dto.OrderManualUpdate.MicroserviceRequests.UpdateShipping;

namespace Kadena2.MicroserviceClients.Clients
{
    public sealed class OrderManualUpdateClient : SignedClientBase, IOrderManualUpdateClient
    {
        public OrderManualUpdateClient(IMicroProperties properties)
        {
            _properties = properties ?? throw new ArgumentNullException(nameof(properties));
            _serviceVersionSettingKey = Settings.KDA_OrderManualUpdateVersion;
        }

        public async Task<BaseResponseDto<object>> UpdateOrder(OrderManualUpdateRequestDto request)
        {
            var url = $"{BaseUrl}/api/v{Version}/order/items";
            return await Patch<object>(url, request).ConfigureAwait(false);
        }

        public async Task<BaseResponseDto<object>> UpdateOrdersShippings(UpdateShippingsRequestDto request)
        {
            var url = $"{BaseUrl}/order/itemsshipping";
            return await Patch<object>(url, request).ConfigureAwait(false);
        }

        public async Task<BaseResponseDto<List<OrderHistoryUpdateDto>>> Get(string orderId)
        {
            var url = $"{BaseUrl}/api/v{Version}/order/history/{orderId}";
            return await Get<List<OrderHistoryUpdateDto>>(url)
                .ConfigureAwait(false);
        }
    }
}

