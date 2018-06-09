using CMS;
using CMS.Scheduler;
using Kadena.ScheduledTasks.DeleteExpiredMailingLists;
using Kadena.ScheduledTasks.Infrastructure;
using Kadena.WebAPI.KenticoProviders.Contracts;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

[assembly: RegisterCustomClass("DeleteExpiredMailingLists", typeof(KenticoTask))]

namespace Kadena.ScheduledTasks.DeleteExpiredMailingLists
{
    public class KenticoTask : ITask
    {
        public string Execute(TaskInfo task)
        {
            var service = Services.Resolve<IDeleteExpiredMailingListsService>();
            var kenticoSite = Services.Resolve<IKenticoSiteProvider>();
            var customerSites = kenticoSite.GetSites();
            var tasks = new List<Task<string>>();
            foreach (var customerSite in customerSites)
            {
                tasks.Add(service.Delete(customerSite));
            }

            return Task
                .WhenAll(tasks)
                .ContinueWith(results => CreateErrorMessageFromResponses(results.Result))
                .Result;
        }

        private string CreateErrorMessageFromResponses(string[] responses)
        {
            return string.Join(Environment.NewLine, responses);
        }
    }
}