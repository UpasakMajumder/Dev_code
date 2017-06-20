using Kadena.Dto.General;
using Kadena2.MicroserviceClients.Contracts;
using Kadena2.MicroserviceClients.MicroserviceRequests;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Kadena2.MicroserviceClients.Clients
{
    public class TaxEstimationServiceClient : ITaxEstimationService
    {
        public async Task<BaseResponseDTO<double>> CalculateTax(string serviceEndpoint, TaxCalculatorRequestDto request)
        {
            using (var httpClient = new HttpClient())
            {
                var requestBody = JsonConvert.SerializeObject(request);
                var content = new StringContent(requestBody, Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync(serviceEndpoint, content);

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    var submitResponse = JsonConvert.DeserializeObject<BaseResponseDTO<double>>(responseContent);
                    return submitResponse;
                }
                else
                {
                    return new BaseResponseDTO<double>()
                    {
                        Success = false,
                        ErrorMessage = $"HTTP error - {response.StatusCode}",
                        Payload = 0.0d
                    };
                }
            }
        }
    }
}
