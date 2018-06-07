using Kadena2.MicroserviceClients.Clients.Base;
using Kadena2.MicroserviceClients.Contracts;
using System.Threading.Tasks;
using Kadena.Dto.General;
using Kadena.Dto.InventoryUpdate.MicroserviceResponses;
using Kadena2.MicroserviceClients.Contracts.Base;
using System;

namespace Kadena2.MicroserviceClients.Clients
{
    public sealed class InventoryUpdateClient : SignedClientBase, IInventoryUpdateClient
    {
        public InventoryUpdateClient(IMicroProperties properties)
        {
            _serviceUrlSettingKey = "KDA_InventoryUpdateServiceEndpoint";
            _properties = properties ?? throw new ArgumentNullException(nameof(properties));
        }

        public async Task<BaseResponseDto<InventoryDataItemDto[]>> GetInventoryItems(string clientId)
        {
            var url = $"{BaseUrlOld}/api/Inventory?erpClientId={clientId}";
            return await Get<InventoryDataItemDto[]>(url).ConfigureAwait(false);
        }
    }
}

