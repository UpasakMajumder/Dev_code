using CMS;
using CMS.Scheduler;
using Kadena.ScheduledTasks.DeleteExpiredMailingLists;

[assembly: RegisterCustomClass("DeleteExpiredMailingLists", typeof(KenticoTask))]

namespace Kadena.ScheduledTasks.DeleteExpiredMailingLists
{
    public class KenticoTask : ITask
    {
        public string Execute(TaskInfo task)
        {
            Services.Resolve<IDeleteExpiredMailingListsService>()
                .Delete().Wait();
            return "done";
        }
    }
}