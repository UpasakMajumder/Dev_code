using Kadena.Dto.General;
using Kadena.Dto.TemplatedProduct.MicroserviceRequests;
using Kadena.Dto.TemplatedProduct.MicroserviceResponses;
using Kadena2.MicroserviceClients.Clients.Base;
using Kadena2.MicroserviceClients.Contracts;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

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
                return await ReadResponseJson<GeneratePdfTaskResponseDto>(response).ConfigureAwait(false);
            }
        }

        public async Task<BaseResponseDto<GeneratePdfTaskStatusResponseDto>> GetGeneratePdfTaskStatus(string endpoint, string templateId, string taskId)
        {
            var url = $"{endpoint.TrimEnd('/')}/api/template/{templateId}/pdftask/{taskId}";
            return await Get<GeneratePdfTaskStatusResponseDto>(url).ConfigureAwait(false);
        }

        public async Task<BaseResponseDto<bool?>> SetName(string endpoint, Guid templateId, string name)
        {
            var url = $"{endpoint.TrimEnd('/')}/api/template";
            var body = new
            {
                templateId = templateId,
                name = name
            };

            return await Put<bool?>(url, body).ConfigureAwait(false);
        }

        public async Task<BaseResponseDto<List<TemplateServiceDocumentResponse>>> GetTemplates(string endpoint, int userId, Guid masterTemplateId)
        {
            var url = $"{endpoint.TrimEnd('/')}/api/template/{masterTemplateId}/users/{userId}";
            return await Get<List<TemplateServiceDocumentResponse>>(url).ConfigureAwait(false);
        }

        public async Task<BaseResponseDto<string>> GetEditorUrl(string endpoint, Guid templateId, Guid workSpaceId, bool useHtml, bool use3d)
        {
            var url = $"{endpoint.TrimEnd('/')}/api/template/{templateId}/workspace/{workSpaceId}?useHtml={useHtml.ToString().ToLower()}&use3D={use3d.ToString().ToLower()}";
            return await Get<string>(url).ConfigureAwait(false);
        }

        public async Task<BaseResponseDto<string>> CreateNewTemplate(string endpoint, NewTemplateRequestDto request)
        {
            var url = $"{endpoint.TrimEnd('/')}/api/template";
            return await Post<string>(url, request).ConfigureAwait(false);
        }

        public async Task<BaseResponseDto<string>> SetMailingList(string endPoint, string containerId, string templateId, string workSpaceId, bool use3d)
        {
            var requestUrl = $"{endPoint}/api/template/datasource";
            return await Post<string>(requestUrl, new
            {
                containerId,
                templateId,
                workSpaceId,
                use3d
            }).ConfigureAwait(false);
        }
    }
}
