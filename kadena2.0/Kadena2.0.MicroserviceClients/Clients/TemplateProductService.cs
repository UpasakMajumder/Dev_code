using Kadena2.MicroserviceClients.Contracts;
using Kadena2.MicroserviceClients.MicroserviceResponses;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Kadena.Dto.TemplatedProduct.MicroserviceResponses;
using System;

namespace Kadena2.MicroserviceClients.Clients
{
    public class TemplateProductService : ITemplatedProductService
    {
        public async Task<GeneratePdfTaskResponseDto> RunGeneratePdfTask(string endpoint, string templateId)
        {
            using (var httpClient = new HttpClient())
            {
                var url = $"{endpoint.TrimEnd('/')}/template{templateId}/pdf";

                var response = await httpClient.GetAsync(url);

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    var responseJson = JsonConvert.DeserializeObject<GeneratePdfTaskResponseDto>(responseContent);
                    return responseJson;
                }
                else
                {
                    //return CreateErrorResponse($"HTTP error - {response.StatusCode}");
                }
            }

            return null;
        }
        
        public async Task<GeneratePdfTaskStatusResponseDto> GetGeneratePdfTaskStatus(string endpoint, string templateId, string taskId)
        {
            using (var httpClient = new HttpClient())
            {
                var url = $"{endpoint.TrimEnd('/')}/template{templateId}/pdf/{taskId}";

                var response = await httpClient.GetAsync(url);

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    var responseJson = JsonConvert.DeserializeObject<GeneratePdfTaskStatusResponseDto>(responseContent);
                    return responseJson;
                }
                else
                {
                    //return CreateErrorResponse($"HTTP error - {response.StatusCode}");
                }
            }

            return null;
        }

        private GeneratePdfTaskResponseDto CreateErrorResponse(string errorMessage)
        {
            /*    return new GeneratePdfTaskResponseDto()
                {
                    Success = false,
                    Payload = 0.0d,
                    ErrorMessages = $"HTTP error - {errorMessage}"
                };*/

            return null;
        }
    }
}
