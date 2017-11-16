using Kadena.Dto.General;
using Kadena.Dto.InventoryUpdate.MicroserviceResponses;
using System.Threading.Tasks;

namespace Kadena2.MicroserviceClients.Contracts
{
    public interface IInventoryUpdateClient
    {
        Task<BaseResponseDto<InventoryDataItemDto[]>> GetInventoryItems(string clientId);
    }
}
