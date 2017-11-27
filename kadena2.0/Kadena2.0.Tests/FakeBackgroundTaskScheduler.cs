using System;
using System.Threading.Tasks;
using System.Threading;
using Kadena.BusinessLogic.Infrastructure;

namespace Kadena.Tests
{
    public class FakeBackgroundTaskScheduler : IBackgroundTaskScheduler
    {
        public void ScheduleBackgroundTask(Func<CancellationToken, Task> workload)
        {
            workload(CancellationToken.None).Wait();
        }
    }
}
