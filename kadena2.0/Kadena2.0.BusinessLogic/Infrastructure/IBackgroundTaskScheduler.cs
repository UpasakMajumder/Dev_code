using System;
using System.Threading;
using System.Threading.Tasks;

namespace Kadena.BusinessLogic.Infrastructure
{
    public interface IBackgroundTaskScheduler
    {
        void ScheduleBackgroundTask(Func<CancellationToken, Task> workload);
    }
}