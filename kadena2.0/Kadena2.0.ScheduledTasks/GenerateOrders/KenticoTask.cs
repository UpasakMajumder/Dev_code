using CMS;
using CMS.Scheduler;
using Kadena.ScheduledTasks.GenerateOrders;
using Kadena.ScheduledTasks.Infrastructure;
using System;

[assembly: RegisterCustomClass("GenerateOrders", typeof(KenticoTask))]

namespace Kadena.ScheduledTasks.GenerateOrders
{
    class KenticoTask: ITask
    {
        public string Execute(TaskInfo taskInfo)
        {
                var service = Services.Resolve<IOrderCreationService>();
                var taskData = taskInfo.TaskData.Split('|');
                var openCampaign = Convert.ToInt32(taskData[0]);
                var campaignClosingUserID = Convert.ToInt32(taskData[1]);
                var orderResponse = service.GenerateOrder(openCampaign, campaignClosingUserID);
                return orderResponse;
        }
    }
}
