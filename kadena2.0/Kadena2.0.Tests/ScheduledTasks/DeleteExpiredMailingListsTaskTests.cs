using CMS.Scheduler;
using Kadena.ScheduledTasks.DeleteExpiredMailingLists;
using Kadena.WebAPI.KenticoProviders.Contracts;
using System;
using Xunit;

namespace Kadena.Tests.ScheduledTasks
{
    public class DeleteExpiredMailingListsTaskTests : KadenaUnitTest<KenticoTask>
    {
        [Theory(DisplayName = "KenticoTask()")]
        [ClassData(typeof(DeleteExpiredMailingListsTaskTests))]
        public void DialogService(IKenticoSiteProvider site)
        {
            Assert.Throws<ArgumentNullException>(() => new KenticoTask(site));
        }
    }
}
