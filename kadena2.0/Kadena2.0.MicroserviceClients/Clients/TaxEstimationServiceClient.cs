using Kadena.Dto.General;
using Kadena2.MicroserviceClients.Clients.Base;
using Kadena2.MicroserviceClients.Contracts;
using Kadena2.MicroserviceClients.Contracts.Base;
using Kadena2.MicroserviceClients.MicroserviceRequests;
using System.Threading.Tasks;

namespace Kadena2.MicroserviceClients.Clients
{
    public sealed class TaxEstimationServiceClient : SignedClientBase, ITaxEstimationServiceClient
    {
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
