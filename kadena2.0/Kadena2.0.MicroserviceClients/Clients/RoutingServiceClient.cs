using Kadena.Dto.General;
using Kadena2.MicroserviceClients.Clients.Base;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using Kadena2.MicroserviceClients.Contracts.Base;
using Kadena.Dto.Routing;
using Kadena.Dto.Routing.MicroserviceRequests;
using Kadena2.MicroserviceClients.Contracts;
using Kadena.Models.SiteSettings;

namespace Kadena2.MicroserviceClients.Clients
{
    public sealed class RoutingServiceClient : SignedClientBase, IRoutingServiceClient
    {
        public RoutingServiceClient(IMicroProperties properties)
        {
            _properties = properties ?? throw new ArgumentNullException(nameof(properties));
            _serviceVersionSettingKey = Settings.KDA_RoutingServiceVersion;
        }

        public async Task<BaseResponseDto<IEnumerable<RoutingDto>>> GetRoutings(string orderId = null, string siteId = null)
        {
            var url = $"{BaseUrl}/api/v{Version}/routing/erp?orderId={orderId}&siteId={siteId}";
            return await Get<IEnumerable<RoutingDto>>(url).ConfigureAwait(false);
        }

        public async Task<BaseResponseDto<RoutingDto>> SetRouting(SetRoutingRequestDto requestBody)
        {
            var url = $"{BaseUrl}/api/v{Version}/routing/erp";
            return await Post<RoutingDto>(url, requestBody).ConfigureAwait(false);
        }

        public async Task<BaseResponseDto<object>> DeleteRouting(DeleteRoutingRequestDto requestBody)
        {
            var url = $"{BaseUrl}/api/v{Version}/routing/erp";
            return await Delete<object>(url, requestBody).ConfigureAwait(false);
        }
    }
}

