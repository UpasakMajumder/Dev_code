using CMS;
using CMS.EventLog;
using CMS.Helpers;
using CMS.Scheduler;
using CMS.SiteProvider;
using Kadena.ScheduledTasks.GenerateOrders;
using Kadena.ScheduledTasks.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
[assembly: RegisterCustomClass("GenerateOrders", typeof(KenticoTask))]

namespace Kadena.ScheduledTasks.GenerateOrders
{
    class KenticoTask: ITask
    {
        public string Execute(TaskInfo taskInfo)
        {
                var service = Services.Resolve<IOrderCreationService>();
                var taskData = taskInfo.TaskData.Split('|');
                var openCampaign = ValidationHelper.GetInteger(taskData[0], 0);
                var campaignClosingUserID = ValidationHelper.GetInteger(taskData[1], 0);
                var orderResponse = service.GenerateOrder(openCampaign, campaignClosingUserID);
                return orderResponse;
        }
    }
}
