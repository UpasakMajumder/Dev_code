using CMS.DataEngine;

namespace Kadena.ScheduledTasks.Infrastructure.Kentico
{
    public class KenticoResourceProvider : IKenticoResourceProvider
    {
        public string GetSettingsByKey(string siteName, string key)
        {
            var keyFullName = $"{siteName}.{key}";
            return SettingsKeyInfoProvider.GetValue(keyFullName);
        }
    }
}