using Kadena2.MicroserviceClients.Clients.Base;
using Kadena2.MicroserviceClients.Contracts;
using System.Threading.Tasks;
using Kadena.Dto.General;
using Kadena.Dto.InventoryUpdate.MicroserviceResponses;
using Kadena.KOrder.PaymentService.Infrastucture.Helpers;

namespace Kadena2.MicroserviceClients.Clients
{
    public class InventoryUpdateClient : ClientBase, IInventoryUpdateClient
    {
        //public InventoryUpdateClient(IAwsV4Signer signer) : base(signer)
        //{

        //}

        public async Task<BaseResponseDto<InventoryDataItemDto[]>> GetInventoryItems(string serviceEndpoint, string clientId)
        {
            var url = $"{serviceEndpoint.TrimEnd('/')}/api/Inventory?erpClientId={clientId}";
            return await Get<InventoryDataItemDto[]>(serviceEndpoint).ConfigureAwait(false);
        }
    }
}

