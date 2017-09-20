using Kadena2.MicroserviceClients.Contracts;
using System.Net.Http;
using System.Threading.Tasks;
using Kadena.Dto.TemplatedProduct.MicroserviceResponses;
using Kadena2.MicroserviceClients.Clients.Base;
using Kadena.Dto.General;
using System;
using Newtonsoft.Json;
using System.Collections.Generic;

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

        public async Task<BaseResponseDto<bool?>> SetName(string endpoint, Guid templateId, string name)
        {
            using (var httpClient = new HttpClient())
            {
                var url = $"{endpoint.TrimEnd('/')}/api/template";
                using (var content = new StringContent(JsonConvert.SerializeObject(new
                {
                    templateId = templateId,
                    name = name
                }), System.Text.Encoding.UTF8, "application/json"))
                {
                    using (var response = await httpClient.PutAsync(url, content))
                    {
                        return await ReadResponseJson<bool?>(response);
                    }
                }
            }
        }

        public async Task<BaseResponseDto<List<TemplateServiceDocumentResponse>>> GetTemplates(string endpoint, int userId, Guid masterTemplateId)
        {
            using (var httpClient = new HttpClient())
            {
                var url = $"{endpoint.TrimEnd('/')}/api/template/{masterTemplateId}/users/{userId}";
                using (var response = await httpClient.GetAsync(url).ConfigureAwait(false))
                {
                    return await ReadResponseJson<List<TemplateServiceDocumentResponse>>(response);
                }
            }
        }
    }
}
