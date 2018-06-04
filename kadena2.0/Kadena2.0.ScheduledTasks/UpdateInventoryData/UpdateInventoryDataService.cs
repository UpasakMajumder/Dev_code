using Kadena.ScheduledTasks.Infrastructure;
using Kadena.WebAPI.KenticoProviders.Contracts;
using Kadena2.MicroserviceClients.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kadena.ScheduledTasks.UpdateInventoryData
{
    public class UpdateInventoryDataService : IUpdateInventoryDataService
    {
        private readonly IConfigurationProvider configurationProvider;
        private readonly IInventoryUpdateClient microserviceInventory;
        private readonly IKenticoSiteProvider kenticoSite;
        private readonly IKenticoSkuProvider skuProvider;
        private readonly IKenticoLogger kenticoLog;

        public UpdateInventoryDataService(IConfigurationProvider configurationProvider, 
                                          IInventoryUpdateClient microserviceInventory,
                                          IKenticoSiteProvider kenticoSite,
                                          IKenticoSkuProvider skuProvider,
                                          IKenticoLogger kenticoLog)
        {
            this.configurationProvider = configurationProvider ?? throw new ArgumentOutOfRangeException(nameof(configurationProvider));
            this.microserviceInventory = microserviceInventory ?? throw new ArgumentOutOfRangeException(nameof(microserviceInventory));
            this.kenticoSite = kenticoSite ?? throw new ArgumentOutOfRangeException(nameof(kenticoSite));
            this.skuProvider = skuProvider ?? throw new ArgumentNullException(nameof(skuProvider));
            this.kenticoLog = kenticoLog ?? throw new ArgumentOutOfRangeException(nameof(kenticoLog));
        }

        public async Task<string> UpdateInventoryData()
        {
            var sites = kenticoSite.GetSites();
            var tasks = new List<Task<string>>();

            foreach (var site in sites)
            {
                var configuration = configurationProvider.Get<UpdateInventoryConfiguration>(site.Id);
                var erpId = configuration.ErpClientId;
                var task = UpdateSiteProducts(erpId);
                tasks.Add(task);
            }

            await Task.WhenAll(tasks.ToArray()).ConfigureAwait(false);
            return string.Join(". ", tasks.Select(t => t.Result));
        }


        private async Task<string> UpdateSiteProducts(string customerErpId)
        {
            var products = await microserviceInventory.GetInventoryItems(customerErpId).ConfigureAwait(false);

            if(!products.Success)
            {
                kenticoLog.LogError("UpdateInventory", products.ErrorMessages);
                return products.ErrorMessages;
            }

            if (products.Payload == null || products.Payload.Length == 0)
            {
                return $"Customer with ErpId {customerErpId} done, but no products data were received from microservice and updated";
            }

            foreach (var product in products.Payload.Where(p => p.ClientId == customerErpId))
            {
                skuProvider.SetSkuAvailableQty(product.Id, (int)product.AvailableQty);
            }

            return $"Customer with ErpId {customerErpId} done successfully";
        }
    }
}
