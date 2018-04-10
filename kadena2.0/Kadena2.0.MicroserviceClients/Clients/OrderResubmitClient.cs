using Kadena.Dto.General;
using Kadena.Dto.Order.Failed;
using Kadena2.MicroserviceClients.Clients.Base;
using Kadena2.MicroserviceClients.Contracts;
using Kadena2.MicroserviceClients.Contracts.Base;
using System;
using System.Threading.Tasks;

namespace Kadena2.MicroserviceClients.Clients
{
    public class OrderResubmitClient : SignedClientBase, IOrderResubmitClient
    {
        private const string _serviceEndpoint = "/api/esbretry";

        public OrderResubmitClient(IMicroProperties properties)
        {
            _serviceUrlSettingKey = "KDA_MicroservicesBaseAddress";
            _properties = properties ?? throw new ArgumentNullException(nameof(properties));
        }

        public async Task<BaseResponseDto<ResubmitOrderResponseDto>> Resubmit(ResubmitOrderRequestDto requestDto)
        {
            var url = $"{_properties.GetServiceUrl(_serviceUrlSettingKey)}{_serviceEndpoint}";
            return await Post<ResubmitOrderResponseDto>(url, requestDto).ConfigureAwait(false);
        }
    }
}
