using Kadena2.MicroserviceClients.Clients.Base;
using Kadena2.MicroserviceClients.Contracts;
using System.Net.Http;
using System.Threading.Tasks;
using Kadena.Dto.General;
using Kadena.Dto.InventoryUpdate.MicroserviceResponses;

namespace Kadena2.MicroserviceClients.Clients
{
    public class InventoryUpdateClient : ClientBase, IInventoryUpdateClient
    {
        public async Task<BaseResponseDto<InventoryDataItemDto[]>> GetInventoryItems(string serviceEndpoint, string clientId)
        {
            using (var httpClient = new HttpClient())
            {
                var url = $"{serviceEndpoint.TrimEnd('/')}/api/Inventory?erpClientId={clientId}";
                using (var response = await httpClient.GetAsync(url).ConfigureAwait(false))
                {
                    return await ReadResponseJson<InventoryDataItemDto[]>(response).ConfigureAwait(false);
                }
            }
        }
    }
}

