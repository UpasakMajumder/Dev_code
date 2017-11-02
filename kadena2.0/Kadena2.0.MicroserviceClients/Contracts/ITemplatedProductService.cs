using Kadena.Dto.General;
using Kadena.Dto.TemplatedProduct.MicroserviceRequests;
using Kadena.Dto.TemplatedProduct.MicroserviceResponses;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Kadena2.MicroserviceClients.Contracts
{
    public interface ITemplatedProductService
    {
        Task<BaseResponseDto<GeneratePdfTaskResponseDto>> RunGeneratePdfTask(string endpoint, string templateId, string settingsId, string siteDomain);
        Task<BaseResponseDto<GeneratePdfTaskStatusResponseDto>> GetGeneratePdfTaskStatus(string endpoint, string templateId, string taskId, string siteDomain);
        Task<BaseResponseDto<bool?>> SetName(string endpoint, Guid templateId, string name, string siteDomain);

        /// <summary>
        /// Returns all copies for master templete for given user
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="masterTemplateId"></param>
        /// <returns>List of template data</returns>

        Task<BaseResponseDto<List<TemplateServiceDocumentResponse>>> GetTemplates(string endpoint, int userId, Guid masterTemplateId, string siteDomain);

        Task<BaseResponseDto<string>> GetEditorUrl(string endpoint, Guid templateId, Guid workSpaceId, bool useHtml, bool use3d, string siteDomain);

        Task<BaseResponseDto<string>> CreateNewTemplate(string endpoint, NewTemplateRequestDto request, string siteDomain);
    }
}
