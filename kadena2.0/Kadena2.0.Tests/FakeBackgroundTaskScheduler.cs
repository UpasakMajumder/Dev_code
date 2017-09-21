using Kadena.WebAPI.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

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
