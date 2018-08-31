using Kadena.Dto.General;
using Kadena.Dto.Order.Failed;
using Kadena.Models.SiteSettings;
using Kadena2.MicroserviceClients.Clients.Base;
using Kadena2.MicroserviceClients.Contracts;
using Kadena2.MicroserviceClients.Contracts.Base;
using System;
using System.Threading.Tasks;

namespace Kadena2.MicroserviceClients.Clients
{
    public class OrderResubmitClient : SignedClientBase, IOrderResubmitClient
    {
        private const string _serviceEndpoint = "/esbretry";

        public OrderResubmitClient(IMicroProperties properties)
        {
            _properties = properties ?? throw new ArgumentNullException(nameof(properties));
            _serviceVersionSettingKey = Settings.KDA_OrderResubmitServiceVersion;
        }

        public async Task<BaseResponseDto<ResubmitOrderResponseDto>> Resubmit(ResubmitOrderRequestDto requestDto)
        {
            var url = $"{BaseUrl}/api/v{Version}{_serviceEndpoint}";
            return await Post<ResubmitOrderResponseDto>(url, requestDto).ConfigureAwait(false);
        }
    }
}
