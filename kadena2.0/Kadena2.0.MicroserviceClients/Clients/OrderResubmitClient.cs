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
        private const string _baseServiceUrlSettingKey = "KDA_MicroservicesBaseAddress";
        private const string _serviceEndpoint = "/api/esbretry";
        private readonly IMicroProperties _properties;

        public OrderResubmitClient(IMicroProperties properties)
        {
            _properties = properties ?? throw new ArgumentNullException(nameof(properties));
        }

        public async Task<BaseResponseDto<ResubmitOrderResponseDto>> Resubmit(ResubmitOrderRequestDto requestDto)
        {
            var url = $"{_properties.GetServiceUrl(_baseServiceUrlSettingKey)}{_serviceEndpoint}";
            return await Post<ResubmitOrderResponseDto>(url, requestDto).ConfigureAwait(false);
        }
    }
}
