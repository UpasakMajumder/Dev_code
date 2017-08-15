using Kadena.WebAPI.KenticoProviders.Contracts;

namespace Kadena.ScheduledTasks.Infrastructure
{
    public class KenticoConfigurationProvider : IConfigurationProvider
    {
        private IKenticoResourceService kenticoService;

        public KenticoConfigurationProvider(IKenticoResourceService kenticoService)
        {
            this.kenticoService = kenticoService;
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
            section.DeleteMailingListsByValidToDateURL = kenticoService.GetSettingsKey(siteName, ""); // TODO
            section.DeleteMailingListsPeriod = StringToInt(kenticoService.GetSettingsKey(siteName, "KDA_MailingList_DeleteExpiredAfter"));
        }
    }
}