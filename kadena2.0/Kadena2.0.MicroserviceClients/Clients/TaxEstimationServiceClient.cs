using Kadena2.MicroserviceClients.Contracts;
using Kadena2.MicroserviceClients.Requests;
using Kadena2.MicroserviceClients.Responses;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Kadena2.MicroserviceClients.Clients
{
    public class TaxEstimationServiceClient : ITaxEstimationService
    {
        public async Task<TaxCalculatorServiceResponseDto> CalculateTax(string serviceEndpoint, TaxCalculatorRequestDto request)
        {
            using (var httpClient = new HttpClient())
            {
                var requestBody = JsonConvert.SerializeObject(request);
                var content = new StringContent(requestBody, Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync(serviceEndpoint, content);

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    var submitResponse = JsonConvert.DeserializeObject<TaxCalculatorServiceResponseDto>(responseContent);
                    return submitResponse;
                }
                else
                {
                    return CreateErrorResponse($"HTTP error - {response.StatusCode}");
                }
            }
        }

        private TaxCalculatorServiceResponseDto CreateErrorResponse(string errorMessage)
        {
            return new TaxCalculatorServiceResponseDto()
            {
                Success = false,
                Payload = 0.0d,
                ErrorMessages = $"HTTP error - {errorMessage}"
            };
        }
    }
}
