using CMS;
using CMS.Scheduler;
using DryIoc;
using Kadena.BusinessLogic.Contracts;
using Kadena.Models.Site;
using Kadena.ScheduledTasks.DeleteExpiredMailingLists;
using Kadena.WebAPI.KenticoProviders.Contracts;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

[assembly: RegisterCustomClass("DeleteExpiredMailingLists", typeof(KenticoTask))]

namespace Kadena.ScheduledTasks.DeleteExpiredMailingLists
{
    public class KenticoTask : ITask
    {
        private readonly IKenticoSiteProvider kenticoSiteProvider;

        public KenticoTask() : this(Services.Resolve<IKenticoSiteProvider>())
        {
            Services.Register<IKenticoSiteProvider, SiteProvider>(setup: Setup.Decorator);
            Services.Register<IKenticoResourceService, ResourceService>(setup: Setup.Decorator);
        }

        public KenticoTask(IKenticoSiteProvider kenticoSiteProvider)
        {
            this.kenticoSiteProvider = kenticoSiteProvider ?? throw new ArgumentNullException(nameof(kenticoSiteProvider));
        }

        public string Execute(TaskInfo task)
        {
            var customerSites = kenticoSiteProvider.GetSites();
            var tasks = new List<Task<string>>();
            foreach (var customerSite in customerSites)
            {
                Services.UpdateInstance(customerSite);
                var service = Services.Resolve<IKListService>();
                tasks.Add(service.DeleteExpiredMailingLists());
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