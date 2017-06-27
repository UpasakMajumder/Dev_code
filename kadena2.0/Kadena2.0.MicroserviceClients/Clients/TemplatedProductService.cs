using Kadena2.MicroserviceClients.Contracts;
using System.Net.Http;
using System.Threading.Tasks;
using Kadena.Dto.TemplatedProduct.MicroserviceResponses;
using Kadena2.MicroserviceClients.Clients.Base;
using Kadena.Dto.General;

namespace Kadena2.MicroserviceClients.Clients
{
    public class TemplatedProductService : ClientBase, ITemplatedProductService
    {
        public async Task<BaseResponseDto<GeneratePdfTaskResponseDto>> RunGeneratePdfTask(string endpoint, string templateId, string settingsId)
        {
            using (var httpClient = new HttpClient())
            {
                var url = $"{endpoint.TrimEnd('/')}/api/template/{templateId}/pdf/{settingsId}";
                var response = await httpClient.GetAsync(url, HttpCompletionOption.ResponseContentRead).ConfigureAwait(false);
                return await ReadResponseJson<GeneratePdfTaskResponseDto>(response);
            }
        }
        
        public async Task<BaseResponseDto<GeneratePdfTaskStatusResponseDto>> GetGeneratePdfTaskStatus(string endpoint, string templateId, string taskId)
        {
            using (var httpClient = new HttpClient())
            {
                var url = $"{endpoint.TrimEnd('/')}/api/template/{templateId}/pdftask/{taskId}";
                var response = await httpClient.GetAsync(url).ConfigureAwait(false);
                return await ReadResponseJson<GeneratePdfTaskStatusResponseDto>(response);
            }
        }
    }
}
