using Kadena.Dto.General;
using Kadena.Dto.TemplatedProduct.MicroserviceRequests;
using Kadena.Dto.TemplatedProduct.MicroserviceResponses;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Kadena.MicroserviceClients.Contracts
{
    public interface ITemplatedClient
    {
        Task<BaseResponseDto<GeneratePdfTaskResponseDto>> RunGeneratePdfTask(string templateId, string settingsId);
        Task<BaseResponseDto<GeneratePdfTaskStatusResponseDto>> GetGeneratePdfTaskStatus(string templateId, string taskId);

        /// <summary>
        /// Updates specified template and PATCHes it's meta data
        /// </summary>
        /// <param name="templateId"></param>
        /// <param name="name"></param>
        /// <param name="meta"></param>
        /// <returns></returns>
        Task<BaseResponseDto<bool?>> UpdateTemplate(Guid templateId, string name, Dictionary<string, object> meta);

        /// <summary>
        /// Returns all copies for master templete for given user
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="masterTemplateId"></param>
        /// <returns>List of template data</returns>
        Task<BaseResponseDto<List<TemplateServiceDocumentResponse>>> GetTemplates(int userId, Guid masterTemplateId);

        Task<BaseResponseDto<string>> GetEditorUrl(Guid templateId, Guid workSpaceId, bool useHtml, bool use3d);

        Task<BaseResponseDto<string>> CreateNewTemplate(NewTemplateRequestDto request);

        /// <summary>
        /// Assign specified container to specified template.
        /// </summary>
        /// <param name="containerId">Id of container.</param>
        /// <param name="templateId">Id of template.</param>
        /// <param name="workspaceId">Id of template workspace</param>
        /// <returns>Url to Chili's editor.</returns>
        Task<BaseResponseDto<string>> SetMailingList(string containerId, string templateId, string workSpaceId, bool use3d);

        Task<BaseResponseDto<string>> GetPreview(Guid templateId, Guid settingId);
    }
}
