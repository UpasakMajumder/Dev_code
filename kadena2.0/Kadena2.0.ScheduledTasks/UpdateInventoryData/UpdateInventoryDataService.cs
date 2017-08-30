using Kadena.ScheduledTasks.Infrastructure;
using Kadena.WebAPI.KenticoProviders.Contracts;
using Kadena2.MicroserviceClients.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kadena.ScheduledTasks.UpdateInventoryData
{
    public class UpdateInventoryDataService : IUpdateInventoryDataService
    {
        private readonly IConfigurationProvider configurationProvider;
        private readonly IInventoryUpdateClient microserviceInventory;
        private readonly IKenticoProviderService kenticoProvider;
        private readonly IKenticoResourceService kenticoResources;

        public UpdateInventoryDataService(IConfigurationProvider configurationProvider, IInventoryUpdateClient microserviceInventory, IKenticoProviderService kenticoProvider, IKenticoResourceService kenticoResources)
        {
            if (configurationProvider == null)
            {
                throw new ArgumentOutOfRangeException(nameof(configurationProvider));
            }

            if (microserviceInventory == null)
            {
                throw new ArgumentOutOfRangeException(nameof(microserviceInventory));
            }

            if (kenticoProvider == null)
            {
                throw new ArgumentOutOfRangeException(nameof(kenticoProvider));
            }

            if (kenticoResources == null)
            {
                throw new ArgumentOutOfRangeException(nameof(kenticoResources));
            }

            this.configurationProvider = configurationProvider;
            this.microserviceInventory = microserviceInventory;
            this.kenticoProvider = kenticoProvider;
            this.kenticoResources = kenticoResources;
        }

        public async Task<string> UpdateInventoryData()
        {
            var serviceEndpoint = configurationProvider.Get<UpdateInventoryConfiguration>(string.Empty).InventoryUpdateServiceEndpoint;
            var sites = kenticoProvider.GetSites();
            var tasks = new List<Task<string>>();

            foreach (var site in sites)
            {
                var erpId = kenticoResources.GetSettingsKey(site.Name, "KDA_ErpCustomerId");
                var task = UpdateSiteProducts(serviceEndpoint, erpId);
                tasks.Add(task);
            }

            await Task.WhenAll(tasks.ToArray()).ConfigureAwait(false);
            return string.Join(". ", tasks.Select(t => t.Result));
        }


        private async Task<string> UpdateSiteProducts(string serviceEndpoint, string customerErpId)
        {
            var products = await microserviceInventory.GetInventoryItems(serviceEndpoint, customerErpId).ConfigureAwait(false);
            return $"{customerErpId} done";
        }
    }
}
