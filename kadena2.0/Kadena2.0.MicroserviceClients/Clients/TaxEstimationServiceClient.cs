using Kadena.Dto.General;
using Kadena2.MicroserviceClients.Clients.Base;
using Kadena2.MicroserviceClients.Contracts;
using Kadena2.MicroserviceClients.Contracts.Base;
using Kadena2.MicroserviceClients.MicroserviceRequests;
using System;
using System.Threading.Tasks;

namespace Kadena2.MicroserviceClients.Clients
{
    public sealed class TaxEstimationServiceClient : SignedClientBase, ITaxEstimationServiceClient
    {
        public TaxEstimationServiceClient(IMicroProperties properties)
        {
            _serviceUrlSettingKey = "KDA_TaxEstimationServiceEndpoint";
            _properties = properties ?? throw new ArgumentNullException(nameof(properties));
        }

        public async Task<BaseResponseDto<decimal>> CalculateTax(TaxCalculatorRequestDto request)
        {
            var url = _properties.GetServiceUrl(_serviceUrlSettingKey);
            url = $"{url}/api/taxcalculator";
            return await Post<decimal>(url, request).ConfigureAwait(false);
        }
    }
}
