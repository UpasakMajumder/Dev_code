using Kadena.Dto.General;
using Kadena.Dto.Payment.CreditCard.MicroserviceRequests;
using Kadena.KOrder.PaymentService.Infrastucture.Helpers;
using Kadena2.MicroserviceClients.Clients.Base;
using Kadena2.MicroserviceClients.Contracts;
using System.Threading.Tasks;

namespace Kadena2.MicroserviceClients.Clients
{
    public class CreditCardManagerClient : ClientBase, ICreditCardManagerClient
    {
        //public CreditCardManagerClient() : base()
        //{

        //}

        //public CreditCardManagerClient(IAwsV4Signer signer) : base(signer)
        //{

        //}

        public async Task<BaseResponseDto<object>> CreateCustomerContainer(string serviceEndpoint, CreateCustomerContainerRequestDto request)
        {
            var url = $"{serviceEndpoint.TrimEnd('/')}/api/Customer/";
            return await Post<object>(url, request).ConfigureAwait(false);
        }
    }
}
