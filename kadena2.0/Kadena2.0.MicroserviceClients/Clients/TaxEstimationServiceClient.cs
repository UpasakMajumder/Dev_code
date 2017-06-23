using Kadena.Dto.General;
using Kadena2.MicroserviceClients.Clients.Base;
using Kadena2.MicroserviceClients.Contracts;
using Kadena2.MicroserviceClients.MicroserviceRequests;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Kadena2.MicroserviceClients.Clients
{
    public class TaxEstimationServiceClient : ClientBase, ITaxEstimationService
    {
        public async Task<BaseResponse<double>> CalculateTax(string serviceEndpoint, TaxCalculatorRequestDto request)
        {
            using (var httpClient = new HttpClient())
            {
                var requestBody = JsonConvert.SerializeObject(request);
                using (var content = new StringContent(requestBody, Encoding.UTF8, "application/json"))
                {
                    using (var response = await httpClient.PostAsync(serviceEndpoint, content))
                    {
                        return await ReadResponseJson<double>(response);
                    }
                }
            }
        }
    }
}
