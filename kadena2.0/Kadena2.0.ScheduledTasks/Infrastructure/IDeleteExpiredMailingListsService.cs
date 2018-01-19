using System;
using System.Threading.Tasks;

namespace Kadena.ScheduledTasks.Infrastructure
{
    public interface IDeleteExpiredMailingListsService
    {
        Func<DateTime> GetCurrentTime { get; set; }
        Task<string> Delete();
    }
}
