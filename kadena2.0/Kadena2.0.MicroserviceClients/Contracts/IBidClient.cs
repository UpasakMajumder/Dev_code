using Kadena.Dto.General;
using Kadena.Dto.KSource;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Kadena2.MicroserviceClients.Contracts
{
    public interface IBidClient
    {
        Task<BaseResponseDto<IEnumerable<ProjectDto>>> GetProjects(string endPoint, string workGroupName);
    }
}
