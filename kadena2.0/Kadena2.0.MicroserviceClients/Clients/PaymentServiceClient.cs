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
        private const string _serviceUrlSettingKey = "KDA_PaymentServiceUrl";
        private readonly IMicroProperties _properties;

        public PaymentServiceClient(IMicroProperties properties)
        {
            if (properties == null)
            {
                throw new ArgumentNullException(nameof(properties));
            }

            this._properties = properties;
        }

        public async Task<BaseResponseDto<AuthorizeAmountResponseDto>> AuthorizeAmount(AuthorizeAmountRequestDto request)
        {
            var url = $"{_properties.GetServiceUrl(_serviceUrlSettingKey)}/api/Payment/PutHold";
            return await Post<AuthorizeAmountResponseDto>(url, request).ConfigureAwait(false);
        }
    }
}

