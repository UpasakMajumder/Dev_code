using Kadena.Dto.General;
using Kadena.Dto.TemplatedProduct.MicroserviceResponses;
using System;
using System.Threading.Tasks;

namespace Kadena2.MicroserviceClients.Contracts
{
    public interface ITemplatedProductService
    {
        Task<BaseResponseDto<GeneratePdfTaskResponseDto>> RunGeneratePdfTask(string endpoint, string templateId, string settingsId);
        Task<BaseResponseDto<GeneratePdfTaskStatusResponseDto>> GetGeneratePdfTaskStatus(string endpoint, string templateId, string taskId);
        Task<BaseResponseDto<bool?>> SetName(string endpoint, Guid templateId, string name);
    }
}
