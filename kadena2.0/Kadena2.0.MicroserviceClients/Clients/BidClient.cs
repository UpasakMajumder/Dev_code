using Kadena2.MicroserviceClients.Contracts;
using System.Collections.Generic;
using System.Threading.Tasks;
using Kadena.Dto.General;
using Kadena.Dto.KSource;
using System.Net.Http;
using Kadena2.MicroserviceClients.Clients.Base;

namespace Kadena2.MicroserviceClients.Clients
{
    public class BidClient : ClientBase, IBidClient
    {
        public async Task<BaseResponseDto<IEnumerable<ProjectDto>>> GetProjects(string endPoint, string workGroupName)
        {
            using (var client = new HttpClient())
            {
                using (var message = await client.GetAsync($"{endPoint}/{workGroupName}").ConfigureAwait(false))
                {
                    return await ReadResponseJson<IEnumerable<ProjectDto>>(message);
                }
            }
        }
    }
}
