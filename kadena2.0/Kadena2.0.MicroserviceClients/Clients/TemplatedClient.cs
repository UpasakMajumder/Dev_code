using Kadena.Dto.General;
using Kadena.Dto.TemplatedProduct.MicroserviceRequests;
using Kadena.Dto.TemplatedProduct.MicroserviceResponses;
using Kadena2.MicroserviceClients.Clients.Base;
using Kadena.MicroserviceClients.Contracts;
using Kadena2.MicroserviceClients.Contracts.Base;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Kadena.MicroserviceClients.Clients
{
    public sealed class TemplatedClient : SignedClientBase, ITemplatedClient
    {
        public TemplatedClient(ISuppliantDomainClient suppliantDomain, IMicroProperties properties) : base(suppliantDomain)
        {
            _serviceUrlSettingKey = "KDA_TemplatingServiceEndpoint";
            _properties = properties ?? throw new ArgumentNullException(nameof(properties));
        }

        public async Task<BaseResponseDto<GeneratePdfTaskStatusResponseDto>> GetGeneratePdfTaskStatus(string templateId, string taskId)
        {
            var url = $"{BaseUrlOld}/api/template/{templateId}/pdftask/{taskId}";
            return await Get<GeneratePdfTaskStatusResponseDto>(url).ConfigureAwait(false);
        }

        public async Task<BaseResponseDto<bool?>> UpdateTemplate(Guid templateId, string name, Dictionary<string, object> metaData)
        {
            var url = $"{BaseUrlOld}/api/template";
            var body = new
            {
                templateId,
                name,
                metaData
            };

            return await Patch<bool?>(url, body).ConfigureAwait(false);
        }

        public async Task<BaseResponseDto<List<TemplateServiceDocumentResponse>>> GetTemplates(int userId, Guid masterTemplateId)
        {
            var url = $"{BaseUrlOld}/api/template/{masterTemplateId}/users/{userId}";
            return await Get<List<TemplateServiceDocumentResponse>>(url).ConfigureAwait(false);
        }

        public async Task<BaseResponseDto<string>> GetEditorUrl(Guid templateId, Guid workSpaceId, bool useHtml, bool use3d)
        {
            var url = $"{BaseUrlOld}/api/template/{templateId}/workspace/{workSpaceId}?useHtml={useHtml.ToString().ToLower()}&use3D={use3d.ToString().ToLower()}";
            return await Get<string>(url).ConfigureAwait(false);
        }

        public async Task<BaseResponseDto<string>> CreateNewTemplate(NewTemplateRequestDto request)
        {
            var url = $"{BaseUrlOld}/api/template";
            return await Post<string>(url, request).ConfigureAwait(false);
        }

        public async Task<BaseResponseDto<string>> SetMailingList(string containerId, string templateId, string workSpaceId, bool use3d)
        {
            var requestUrl = $"{BaseUrlOld}/api/template/datasource";
            return await Post<string>(requestUrl, new
            {
                containerId,
                templateId,
                workSpaceId,
                use3d
            }).ConfigureAwait(false);
        }

        public async Task<BaseResponseDto<string>> GetPreview(Guid templateId, Guid settingId)
        {
            var url = $"{BaseUrlOld}/api/template/{templateId}/preview/{settingId}";
            return await Get<string>(url).ConfigureAwait(false);
        }
    }
}
