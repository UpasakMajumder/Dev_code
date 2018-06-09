using Kadena.Models.Site;
using Kadena.ScheduledTasks.Infrastructure;
using Kadena.WebAPI.KenticoProviders.Contracts;
using Kadena2.MicroserviceClients.Clients;
using System;
using System.Threading.Tasks;

namespace Kadena.ScheduledTasks.DeleteExpiredMailingLists
{
    public class DeleteExpiredMailingListsService : IDeleteExpiredMailingListsService
    {
        private IConfigurationProvider configurationProvider;

        public DeleteExpiredMailingListsService(IConfigurationProvider configurationProvider)
        {
            this.configurationProvider = configurationProvider ?? throw new ArgumentNullException(nameof(configurationProvider));
        }

        public async Task<string> Delete(KenticoSite site)
        {
            var now = DateTime.Now;
            var config = configurationProvider.Get<MailingListConfiguration>(site.Id);
            if (config.DeleteMailingListsPeriod != null)
            {
                var deleteOlderThan = now.AddDays(-config.DeleteMailingListsPeriod.Value);
                var mailingListClient = new MailingListClient(new StaticMicroProperties(site.Name));
                var result = await mailingListClient.RemoveMailingList(deleteOlderThan).ConfigureAwait(false);

                if (!result.Success)
                {
                    return $"Failure for {site} - { result.ErrorMessages}";
                }
            }

            return $"{site} - Done";
        }
    }
}