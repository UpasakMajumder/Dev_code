using CMS.Helpers;
using CMS.DataEngine;
using CMS.SiteProvider;
using Kadena.WebAPI.KenticoProviders.Contracts;
using CMS.Localization;
using CMS.MacroEngine;

namespace Kadena.WebAPI.KenticoProviders
{
    public class KenticoResourceService : IKenticoResourceService
    {
        public string GetResourceString(string name)
        {
            return ResHelper.GetString(name, LocalizationContext.CurrentCulture.CultureCode);
        }

        public string GetSettingsKey(string key)
        {
            return GetSettingsKey(SiteContext.CurrentSiteName, key);
        }
                        
        public string GetSettingsKey(string siteName, string key)
        {
            string resourceKey = $"{siteName}.{key}";
            return SettingsKeyInfoProvider.GetValue(resourceKey);
        }

        public string GetSettingsKey(int siteId, string key)
        {
            return SettingsKeyInfoProvider.GetValue(key, new SiteInfoIdentifier(siteId));
        }               
        public string ResolveMacroString(string macroString)
        {
            return MacroResolver.Resolve(macroString, new MacroSettings { Culture = LocalizationContext.CurrentCulture.CultureCode });
        }
    }
}