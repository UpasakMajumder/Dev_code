using Kadena2.MicroserviceClients.Contracts;
using System.Collections.Generic;
using System.Threading.Tasks;
using Kadena.Dto.General;
using Kadena.Dto.KSource;
using Kadena2.MicroserviceClients.Clients.Base;
using Kadena.KOrder.PaymentService.Infrastucture.Helpers;

namespace Kadena2.MicroserviceClients.Clients
{
    public class BidClient : ClientBase, IBidClient
    {
        public async Task<BaseResponseDto<IEnumerable<ProjectDto>>> GetProjects(string endPoint, string workGroupName)
        {
            var url = $"{endPoint}/{workGroupName}";
            return await Get<IEnumerable<ProjectDto>>(url).ConfigureAwait(false);
        }
    }
}
