using Kadena.Dto.General;
using Kadena.Dto.TemplatedProduct.MicroserviceRequests;
using Kadena.Dto.TemplatedProduct.MicroserviceResponses;
using Kadena2.MicroserviceClients.Contracts.Base;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Kadena2.MicroserviceClients.Contracts
{
    public interface ITemplatedProductService : ISuppliantDomainClient
    {
        Task<BaseResponseDto<GeneratePdfTaskResponseDto>> RunGeneratePdfTask(string endpoint, string templateId, string settingsId);
        Task<BaseResponseDto<GeneratePdfTaskStatusResponseDto>> GetGeneratePdfTaskStatus(string endpoint, string templateId, string taskId);
        Task<BaseResponseDto<bool?>> SetName(string endpoint, Guid templateId, string name);

        /// <summary>
        /// Returns all copies for master templete for given user
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="masterTemplateId"></param>
        /// <returns>List of template data</returns>

        Task<BaseResponseDto<List<TemplateServiceDocumentResponse>>> GetTemplates(string endpoint, int userId, Guid masterTemplateId);

        Task<BaseResponseDto<string>> GetEditorUrl(string endpoint, Guid templateId, Guid workSpaceId, bool useHtml, bool use3d);

        Task<BaseResponseDto<string>> CreateNewTemplate(string endpoint, NewTemplateRequestDto request);
    }
}
