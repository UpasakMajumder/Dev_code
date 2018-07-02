using Kadena.Models.SiteSettings;
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
        private readonly IKenticoResourceService kenticoResources;

        public UpdateInventoryDataService(IConfigurationProvider configurationProvider,
                                          IInventoryUpdateClient microserviceInventory,
                                          IKenticoSiteProvider kenticoSite,
                                          IKenticoSkuProvider skuProvider,
                                          IKenticoLogger kenticoLog,
                                          IKenticoResourceService kenticoResources)
        {
            this.configurationProvider = configurationProvider ?? throw new ArgumentOutOfRangeException(nameof(configurationProvider));
            this.microserviceInventory = microserviceInventory ?? throw new ArgumentOutOfRangeException(nameof(microserviceInventory));
            this.kenticoSite = kenticoSite ?? throw new ArgumentOutOfRangeException(nameof(kenticoSite));
            this.skuProvider = skuProvider ?? throw new ArgumentNullException(nameof(skuProvider));
            this.kenticoLog = kenticoLog ?? throw new ArgumentOutOfRangeException(nameof(kenticoLog));
            this.kenticoResources = kenticoResources ?? throw new ArgumentOutOfRangeException(nameof(kenticoResources));
        }

        public async Task<string> UpdateInventoryData()
        {
            var customerErpId = kenticoResources.GetSiteSettingsKey<string>(Settings.KDA_ErpCustomerId);
            var products = await microserviceInventory.GetInventoryItems(customerErpId).ConfigureAwait(false);

            if (!products.Success)
            {
                kenticoLog.LogError("UpdateInventory", products.ErrorMessages);
                return products.ErrorMessages;
            }

            if (products.Payload == null || products.Payload.Length == 0)
            {
                return $"Customer with ErpId {customerErpId} done, but no products data were received from microservice and updated.";
            }

            foreach (var product in products.Payload.Where(p => p.ClientId == customerErpId))
            {
                skuProvider.SetSkuAvailableQty(product.Id, (int)product.AvailableQty);
            }

            return $"Customer with ErpId {customerErpId} done successfully.";
        }
    }
}
