using Kadena.Dto.General;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Kadena2.MicroserviceClients.Contracts
{
    public interface IExportClient
    {
        Task<BaseResponseDto<string>> ExportMailingList(Guid containerId, string siteName);
    }
}
