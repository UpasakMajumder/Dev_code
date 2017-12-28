using Kadena2.MicroserviceClients.Clients.Base;
using Kadena2.MicroserviceClients.Contracts;
using System.Threading.Tasks;
using Kadena.Dto.General;
using Kadena.Dto.InventoryUpdate.MicroserviceResponses;
using Kadena2.MicroserviceClients.Contracts.Base;

namespace Kadena2.MicroserviceClients.Clients
{
    public sealed class InventoryUpdateClient : SignedClientBase, IInventoryUpdateClient
    {
        private const string _serviceUrlSettingKey = "KDA_InventoryUpdateServiceEndpoint";
        private readonly IMicroProperties _properties;

        public InventoryUpdateClient(IMicroProperties properties)
        {
            _properties = properties;
        }

        public async Task<BaseResponseDto<InventoryDataItemDto[]>> GetInventoryItems(string clientId)
        {
            var url = _properties.GetServiceUrl(_serviceUrlSettingKey);
            url = $"{url}/api/Inventory?erpClientId={clientId}";
            return await Get<InventoryDataItemDto[]>(url).ConfigureAwait(false);
        }
    }
}

