using Kadena.Dto.General;
using System;
using System.Threading.Tasks;

namespace Kadena2.MicroserviceClients.Contracts
{
    public interface IExportClient
    {
        Task<BaseResponseDto<string>> ExportMailingList(Guid containerId);
    }
}
