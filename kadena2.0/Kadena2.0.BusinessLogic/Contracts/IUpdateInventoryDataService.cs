using System.Threading.Tasks;

namespace Kadena.BusinessLogic.Contracts
{
    public interface IUpdateInventoryDataService
    {
        Task<string> UpdateInventoryData();
    }
}
