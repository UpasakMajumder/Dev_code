using Kadena.Dto.General;
using Kadena.Models.Site;
using Kadena.ScheduledTasks.Infrastructure;
using Kadena.WebAPI.KenticoProviders.Contracts;
using Kadena2.MicroserviceClients.Clients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kadena.ScheduledTasks.DeleteExpiredMailingLists
{
    public class DeleteExpiredMailingListsService : IDeleteExpiredMailingListsService
    {
        private IConfigurationProvider configurationProvider;
        private IKenticoSiteProvider kenticoSiteProvider;

        public Func<DateTime> GetCurrentTime { get; set; } = () => DateTime.Now;

        public DeleteExpiredMailingListsService(IConfigurationProvider configurationProvider, IKenticoSiteProvider kenticoSiteProvider)
        {
            if (configurationProvider == null)
            {
                throw new ArgumentNullException(nameof(configurationProvider));
            }
            if (kenticoSiteProvider == null)
            {
                throw new ArgumentNullException(nameof(kenticoSiteProvider));
            }

            this.configurationProvider = configurationProvider;
            this.kenticoSiteProvider = kenticoSiteProvider;
        }

        public async Task<string> Delete()
        {
            var customerSites = kenticoSiteProvider.GetSites();
            var now = GetCurrentTime();
            
            var tasks = new List<Task<BaseResponseDto<object>>>();
            foreach (var customerSite in customerSites)
            {
                var config = configurationProvider.Get<MailingListConfiguration>(customerSite.Id);
                if (config.DeleteMailingListsPeriod != null)
                {
                    var deleteOlderThan = now.AddDays(-config.DeleteMailingListsPeriod.Value);
                    tasks.Add(new MailingListClient(new StaticMicroProperties(customerSite.Name)).RemoveMailingList(deleteOlderThan));
                }
            }

            var results = await Task.WhenAll(tasks).ConfigureAwait(false);
            if (results.Any(r => !r.Success))
            {
                return CreateErrorMessageFromResponses(results, customerSites);
            }

            return "Done";
        }

        private string CreateErrorMessageFromResponses(BaseResponseDto<object>[] responses, Site[] customers)
        {
            var error = new StringBuilder();
            for (int i = 0; i < responses.Length; i++)
            {
                var response = responses[i];
                if (!response.Success)
                {
                    error.AppendLine($"Failure for {customers[i]} - { response.Error?.Message}");
                }
            }

            return error.ToString();
        }
    }
}