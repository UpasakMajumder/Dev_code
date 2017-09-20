using System;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Hosting;

namespace Kadena.WebAPI.Infrastructure
{
    public class BackgroundTaskScheduler : IBackgroundTaskScheduler
    {
        public void ScheduleBackgroundTask(Func<CancellationToken, Task> workload)
        {
            HostingEnvironment.QueueBackgroundWorkItem(workload);
        }
    }
}