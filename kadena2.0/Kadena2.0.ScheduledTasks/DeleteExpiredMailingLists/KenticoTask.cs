using CMS;
using CMS.Scheduler;
using Kadena.ScheduledTasks.DeleteExpiredMailingLists;
using Kadena2.WebAPI.KenticoProviders;
using System;

[assembly: RegisterCustomClass("DeleteExpiredMailingLists", typeof(KenticoTask))]

namespace Kadena.ScheduledTasks.DeleteExpiredMailingLists
{
    public class KenticoTask : ITask
    {
        public string Execute(TaskInfo task)
        {
            try
            {
                var service = Services.Resolve<DeleteExpiredMailingListsService>();
                return service.Delete().Result;
            }
            catch (Exception ex)
            {
                var processName = System.Diagnostics.Process.GetCurrentProcess().ProcessName;
                ProviderFactory.KenticoLogger.LogError("DeleteExpiredMailingLists task", $"[{processName}] {ex.ToString()}");
                return ex.ToString();
            }
        }
    }
}