using System;
using System.Threading;
using System.Threading.Tasks;

namespace Kadena.Helpers.Infrastructure
{
    public interface IBackgroundTaskScheduler
    {
        void ScheduleBackgroundTask(Func<CancellationToken, Task> workload);
    }
}