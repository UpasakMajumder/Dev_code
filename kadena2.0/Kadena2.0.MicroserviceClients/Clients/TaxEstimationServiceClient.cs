using Kadena.Dto.General;
using Kadena.KOrder.PaymentService.Infrastucture.Helpers;
using Kadena2.MicroserviceClients.Clients.Base;
using Kadena2.MicroserviceClients.Contracts;
using Kadena2.MicroserviceClients.Contracts.Base;
using Kadena2.MicroserviceClients.MicroserviceRequests;
using System.Threading.Tasks;

namespace Kadena2.MicroserviceClients.Clients
{
    public class TaxEstimationServiceClient : ClientBase, ITaxEstimationServiceClient
    {
        //public TaxEstimationServiceClient(IAwsV4Signer signer) : base(signer)
        //{

        //}
        private const string _serviceUrlSettingKey = "KDA_TaxEstimationServiceEndpoint";
        private readonly IMicroProperties _properties;

        public TaxEstimationServiceClient(IMicroProperties properties)
        {
            _properties = properties;
        }


        public async Task<BaseResponseDto<decimal>> CalculateTax(TaxCalculatorRequestDto request)
        {
            var url = _properties.GetServiceUrl(_serviceUrlSettingKey);
            url = $"{url}/api/taxcalculator";
            return await Post<decimal>(url, request).ConfigureAwait(false);
        }
    }
}
