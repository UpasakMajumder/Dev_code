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

        public T Get<T>(string siteName) where T : IConfigurationSection, new()
        {
            dynamic section = new T();
            LoadSection(section, siteName);
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

        private void LoadSection(MailingListConfiguration section, string siteName)
        {
            section.MailingServiceUrl = kenticoResources.GetSettingsKey(siteName, "KDA_MailingServiceUrl");
            section.DeleteMailingListsPeriod = StringToInt(kenticoResources.GetSettingsKey(siteName, "KDA_MailingList_DeleteExpiredAfter"));
        }

        private void LoadSection(UpdateInventoryConfiguration section, string siteName)
        {
            section.ErpClientId = kenticoResources.GetSettingsKey(siteName, "KDA_ErpCustomerId");
            section.InventoryUpdateServiceEndpoint = kenticoResources.GetSettingsKey("KDA_InventoryUpdateServiceEndpoint");
        }
    }
}