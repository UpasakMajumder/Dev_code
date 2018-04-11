using Kadena2.MicroserviceClients.Clients.Base;
using Kadena2.MicroserviceClients.Contracts;
using System.Threading.Tasks;
using Kadena.Dto.General;
using Kadena.Dto.CreditCard.MicroserviceRequests;
using Kadena.Dto.CreditCard.MicroserviceResponses;
using Kadena2.MicroserviceClients.Contracts.Base;
using System;

namespace Kadena2.MicroserviceClients.Clients
{
    public sealed class PaymentServiceClient : SignedClientBase, IPaymentServiceClient
    {
        public PaymentServiceClient(IMicroProperties properties)
        {
            _serviceUrlSettingKey = "KDA_PaymentServiceUrl";
            _properties = properties ?? throw new ArgumentNullException(nameof(properties));
        }

        public async Task<BaseResponseDto<AuthorizeAmountResponseDto>> AuthorizeAmount(AuthorizeAmountRequestDto request)
        {
            var url = $"{BaseUrlOld}/api/Payment/PutHold";
            return await Post<AuthorizeAmountResponseDto>(url, request).ConfigureAwait(false);
        }
    }
}

