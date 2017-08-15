using CMS.Scheduler;

namespace Kadena.ScheduledTasks.DeleteExpiredMailingLists
{
    public class KenticoTask : ITask
    {
        public string Execute(TaskInfo task)
        {
            Services.Resolve<IDeleteExpiredMailingListsService>().Delete();
            return "done";
        }
    }
}