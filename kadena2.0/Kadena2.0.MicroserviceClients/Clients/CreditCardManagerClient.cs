using System;
using Kadena.Dto.General;
using Kadena.Dto.Payment.CreditCard.MicroserviceRequests;
using Kadena2.MicroserviceClients.Clients.Base;
using Kadena2.MicroserviceClients.Contracts;
using Kadena2.MicroserviceClients.Contracts.Base;
using System.Threading.Tasks;

namespace Kadena2.MicroserviceClients.Clients
{
    public sealed class CreditCardManagerClient : SignedClientBase, ICreditCardManagerClient
    {
        public CreditCardManagerClient(IMicroProperties properties)
        {
            _serviceUrlSettingKey = "KDA_CreditCardManagerEndpoint";
            _properties = properties ?? throw new ArgumentNullException(nameof(properties));
        }

        public async Task<BaseResponseDto<object>> CreateCustomerContainer(CreateCustomerContainerRequestDto request)
        {
            var url = $"{BaseUrlOld}/api/Customer/";
            return await Post<object>(url, request).ConfigureAwait(false);
        }

        public async Task<BaseResponseDto<object>> UpdateCustomerContainer(UpdateCustomerContainerRequestDto request)
        {
            var url = $"{BaseUrlOld}/api/Customer/";
            return await Patch<object>(url, request).ConfigureAwait(false);
        }
    }
}
