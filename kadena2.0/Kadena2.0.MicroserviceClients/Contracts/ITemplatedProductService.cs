using Kadena.Dto.TemplatedProduct.MicroserviceResponses;
using System.Threading.Tasks;

namespace Kadena2.MicroserviceClients.Contracts
{
    public interface ITemplatedProductService
    {
        Task<GeneratePdfTaskResponseDto> RunGeneratePdfTask(string endpoint, string templateId);
        Task<GeneratePdfTaskStatusResponseDto> GetGeneratePdfTaskStatus(string endpoint, string templateId, string taskId);
    }
}
