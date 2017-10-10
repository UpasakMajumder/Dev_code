using System.Threading.Tasks;

namespace Kadena.ScheduledTasks.Infrastructure
{
    public interface IUpdateInventoryDataService
    {
        Task<string> UpdateInventoryData();
    }
}
