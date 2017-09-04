using CMS;
using CMS.Scheduler;
using Kadena.ScheduledTasks.Infrastructure;
using Kadena.ScheduledTasks.UpdateInventoryData;

[assembly: RegisterCustomClass("UpdateInventoryItems", typeof(KenticoTask))]

namespace Kadena.ScheduledTasks.UpdateInventoryData
{
    public class KenticoTask : ITask
    {
        public string Execute(TaskInfo task)
        {
            var service = Services.Resolve<IUpdateInventoryDataService>();
            return service.UpdateInventoryData().Result;
        }
    }
}
