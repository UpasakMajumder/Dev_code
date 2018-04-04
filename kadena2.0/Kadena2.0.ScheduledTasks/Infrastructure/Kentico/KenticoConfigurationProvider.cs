using Kadena.WebAPI.KenticoProviders.Contracts;

namespace Kadena.ScheduledTasks.Infrastructure.Kentico
{
    public class KenticoConfigurationProvider : IConfigurationProvider
    {
        private IKenticoResourceService kenticoResources;

        public KenticoConfigurationProvider(IKenticoResourceService kenticoResources)
        {
            this.kenticoResources = kenticoResources;
        }

        public T Get<T>(int siteId) where T : IConfigurationSection, new()
        {
            dynamic section = new T();
            LoadSection(section, siteId);
            return section;
        }

        private int? StringToInt(string value)
        {
            int parsed;
            if (int.TryParse(value, out parsed))
            {
                return parsed;
            }

            return null;
        }

        private void LoadSection(MailingListConfiguration section, int siteId)
        {
            section.MailingServiceUrl = kenticoResources.GetSettingsKey<string>("KDA_MailingServiceUrl", siteId);
            section.DeleteMailingListsPeriod = StringToInt(kenticoResources.GetSettingsKey<string>("KDA_MailingList_DeleteExpiredAfter", siteId));
        }

        private void LoadSection(UpdateInventoryConfiguration section, int siteId)
        {
            section.ErpClientId = kenticoResources.GetSettingsKey<string>("KDA_ErpCustomerId", siteId);
            section.InventoryUpdateServiceEndpoint = kenticoResources.GetSiteSettingsKey("KDA_InventoryUpdateServiceEndpoint");
        }
    }
}