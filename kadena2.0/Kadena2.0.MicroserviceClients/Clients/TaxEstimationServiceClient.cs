using Kadena.Dto.General;
using Kadena.KOrder.PaymentService.Infrastucture.Helpers;
using Kadena2.MicroserviceClients.Clients.Base;
using Kadena2.MicroserviceClients.Contracts;
using Kadena2.MicroserviceClients.MicroserviceRequests;
using System.Threading.Tasks;

namespace Kadena2.MicroserviceClients.Clients
{
    public class TaxEstimationServiceClient : ClientBase, ITaxEstimationServiceClient
    {
        //public TaxEstimationServiceClient(IAwsV4Signer signer) : base(signer)
        //{

        //}
        public TaxEstimationServiceClient()
        {

        }


        public async Task<BaseResponseDto<decimal>> CalculateTax(string serviceEndpoint, TaxCalculatorRequestDto request)
        {
            var url = $"{serviceEndpoint}/api/taxcalculator";
            return await Post<decimal>(url, request).ConfigureAwait(false);
        }
    }
}
