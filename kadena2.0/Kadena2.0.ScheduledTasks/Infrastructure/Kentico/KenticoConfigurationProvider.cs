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
            section.DeleteMailingListsByValidToDateURL = kenticoResources.GetSettingsKey(siteName, "KDA_MailingList_DeleteListByFilterURL");
            section.DeleteMailingListsPeriod = StringToInt(kenticoResources.GetSettingsKey(siteName, "KDA_MailingList_DeleteExpiredAfter"));
        }
    }
}