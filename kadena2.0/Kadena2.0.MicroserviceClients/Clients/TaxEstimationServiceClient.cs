using Kadena.Dto.General;
using Kadena.Models.SiteSettings;
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
            _properties = properties ?? throw new ArgumentNullException(nameof(properties));
            _serviceVersionSettingKey = Settings.KDA_TaxEstimationServiceVersion;
        }

        public async Task<BaseResponseDto<decimal>> CalculateTax(TaxCalculatorRequestDto request)
        {
            var url = $"{BaseUrl}/api/v{Version}/taxcalculator";
            return await Post<decimal>(url, request).ConfigureAwait(false);
        }
    }
}
