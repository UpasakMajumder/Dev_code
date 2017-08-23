using Kadena.Dto.General;
using Kadena.ScheduledTasks.Infrastructure;
using Kadena2.MicroserviceClients.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kadena.ScheduledTasks.DeleteExpiredMailingLists
{
    public class DeleteExpiredMailingListsService
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

        public async Task Delete()
        {
            var customers = kenticoProvider.GetSites();
            var now = GetCurrentTime();

            var tasks = new List<Task<BaseResponseDto<object>>>();
            foreach (var customer in customers)
            {
                var config = configurationProvider.Get<MailingListConfiguration>(customer);
                if (config.DeleteMailingListsPeriod != null)
                {
                    var deleteOlderThan = now.AddDays(-config.DeleteMailingListsPeriod.Value);
                    tasks.Add(mailingService.RemoveMailingList(config.DeleteMailingListsByValidToDateURL, customer, deleteOlderThan));
                }
            }

            var results = await Task.WhenAll(tasks).ConfigureAwait(false);
            if (results.Any(r => !r.Success))
            {
                throw new Exception(CreateErrorMessageFromResponses(results, customers));
            }
        }

        private string CreateErrorMessageFromResponses(BaseResponseDto<object>[] responses, string[] customers)
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