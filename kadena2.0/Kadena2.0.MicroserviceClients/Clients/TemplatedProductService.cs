using Kadena2.MicroserviceClients.Contracts;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;
using Kadena.Dto.TemplatedProduct.MicroserviceResponses;
using Kadena2.MicroserviceClients.Communication;

namespace Kadena2.MicroserviceClients.Clients
{
    public class TemplatedProductService : ITemplatedProductService
    {
        public async Task<BaseResponse<GeneratePdfTaskResponseDto>> RunGeneratePdfTask(string endpoint, string templateId, string settingsId)
        {
            using (var httpClient = new HttpClient())
            {
                var url = $"{endpoint.TrimEnd('/')}/api/template/{templateId}/pdf/{settingsId}";

                var response = await httpClient.GetAsync(url, HttpCompletionOption.ResponseContentRead).ConfigureAwait(false);

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    string responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    var responseJson = JsonConvert.DeserializeObject<BaseResponse<GeneratePdfTaskResponseDto>>(responseContent);
                    return responseJson;
                }
                else
                {
                    return new BaseResponse<GeneratePdfTaskResponseDto>()
                    {
                        ErrorMessage = $"Microservice client received status {response.StatusCode}",
                        Success = false
                    };
                }
            }
        }
        
        public async Task<BaseResponse<GeneratePdfTaskStatusResponseDto>> GetGeneratePdfTaskStatus(string endpoint, string templateId, string taskId)
        {
            using (var httpClient = new HttpClient())
            {
                var url = $"{endpoint.TrimEnd('/')}/api/template/{templateId}/pdftask/{taskId}";

                var response = await httpClient.GetAsync(url).ConfigureAwait(false);

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    string responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    var responseJson = JsonConvert.DeserializeObject<BaseResponse<GeneratePdfTaskStatusResponseDto>>(responseContent);
                    return responseJson;
                }
                else
                {
                    return new BaseResponse<GeneratePdfTaskStatusResponseDto>()
                    {
                        ErrorMessage = $"Microservice client received status {response.StatusCode}",
                        Success = false
                    };
                }
            }
        }
    }
}
