using Kadena.ScheduledTasks.Infrastructure;
using Kadena2.MicroserviceClients.Contracts;
using System;

namespace Kadena.ScheduledTasks.DeleteExpiredMailingLists
{
    public interface IDeleteExpiredMailingListsService
    {
        void Delete();
    }

    public class DeleteExpiredMailingListsService : IDeleteExpiredMailingListsService
    {
        private IConfigurationProvider configurationProvider;
        private IKenticoProvider kenticoProvider;
        private IMailingListClient mailingService;

        public Func<DateTime> GetCurrentTime { get; set; } = () => DateTime.Now;

        public DeleteExpiredMailingListsService(IConfigurationProvider configurationProvider, IKenticoProvider kenticoProvider, IMailingListClient mailingService)
        {
            if (configurationProvider == null)
            {
                throw new ArgumentNullException(nameof(configurationProvider));
            }
            if (kenticoProvider == null)
            {
                throw new ArgumentNullException(nameof(kenticoProvider));
            }
            if (mailingService == null)
            {
                throw new ArgumentNullException(nameof(mailingService));
            }

            this.configurationProvider = configurationProvider;
            this.kenticoProvider = kenticoProvider;
            this.mailingService = mailingService;
        }

        public void Delete()
        {
            var customers = kenticoProvider.GetSites();
            var now = GetCurrentTime();

            foreach (var customer in customers)
            {
                var config = configurationProvider.Get<MailingListConfiguration>(customer);
                if (config.DeleteMailingListsPeriod != null)
                {
                    var deleteOlderThan = now.AddDays(-config.DeleteMailingListsPeriod.Value);
                    mailingService. // TODO: add method to microservice client once it is ready
                }
            }
        }
    }
}