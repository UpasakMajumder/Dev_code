using Kadena.Models.Site;
using Kadena.Models.SiteSettings;
using Kadena.ScheduledTasks.Infrastructure;
using Kadena.WebAPI.KenticoProviders.Contracts;
using Kadena2.MicroserviceClients.Clients;
using Kadena2.MicroserviceClients.Contracts;
using System;
using System.Threading.Tasks;

namespace Kadena.ScheduledTasks.DeleteExpiredMailingLists
{
    public class DeleteExpiredMailingListsService : IDeleteExpiredMailingListsService
    {
        private readonly IKenticoResourceService resourceService;
        private readonly IMailingListClient mailingListClient;
        private readonly IKenticoSiteProvider siteProvider;

        public DeleteExpiredMailingListsService(IKenticoResourceService resourceService, IMailingListClient mailingListClient, IKenticoSiteProvider siteProvider)
        {
            this.resourceService = resourceService ?? throw new ArgumentNullException(nameof(resourceService));
            this.mailingListClient = mailingListClient ?? throw new ArgumentNullException(nameof(mailingListClient));
            this.siteProvider = siteProvider ?? throw new ArgumentNullException(nameof(siteProvider));
        }

        public async Task<string> Delete()
        {
            var expirationDays = resourceService.GetSettingsKey<int>(Settings.KDA_MailingList_DeleteExpiredAfter);
            var deleteOlderThan = DateTime.Now.AddDays(-expirationDays);
            var result = await mailingListClient.RemoveMailingList(deleteOlderThan).ConfigureAwait(false);

            var site = siteProvider.GetKenticoSite();
            if (!result.Success)
            {
                return $"Failure for {site} - { result.ErrorMessages}";
            }

            return $"{site} - Done";
        }
    }
}