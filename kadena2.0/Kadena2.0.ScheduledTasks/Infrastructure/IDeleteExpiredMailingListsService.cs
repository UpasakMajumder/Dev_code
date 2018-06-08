using System;
using System.Threading.Tasks;

namespace Kadena.ScheduledTasks.Infrastructure
{
    public interface IDeleteExpiredMailingListsService
    {
        Task<string> Delete();
    }
}
