using CMS.Helpers;
using CMS.DataEngine;
using CMS.SiteProvider;
using Kadena.WebAPI.KenticoProviders.Contracts;
using CMS.Localization;
using CMS.MacroEngine;
using System;
using CMS.Membership;

namespace Kadena.WebAPI.KenticoProviders
{
    public class KenticoResourceService : IKenticoResourceService
    {
        public string GetResourceString(string name, string cultureCode)
        {
            return ResHelper.GetString(name, culture: cultureCode);
        }

        public string GetResourceString(string name)
        {
            return ResHelper.GetString(name, LocalizationContext.CurrentCulture.CultureCode);
        }

        public string GetPerSiteResourceString(string name, string site = "")
        {
            if (string.IsNullOrEmpty(site))
            {
                site = SiteContext.CurrentSiteName;
            }

            var fullname = $"{site}.{name}";

            
            var perSiteStringInfo = ResourceStringInfoProvider.GetResourceStringInfo(fullname, LocalizationContext.CurrentCulture.CultureCode);

            if (perSiteStringInfo != null && perSiteStringInfo.StringID > 0)
            {
                return perSiteStringInfo.TranslationText;
            }

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

        public T GetSettingsKey<T>(string key, int siteId = 0) where T : IConvertible
        {
            var value = (siteId > 0)
                ? SettingsKeyInfoProvider.GetValue(key, new SiteInfoIdentifier(siteId))
                : SettingsKeyInfoProvider.GetValue(key);

            return (T)Convert.ChangeType(value, typeof(T));
        }

        public string ResolveMacroString(string macroString)
        {
            return MacroResolver.Resolve(macroString, new MacroSettings { Culture = LocalizationContext.CurrentCulture.CultureCode });
        }

        public string GetLogonPageUrl()
        {
            return URLHelper.ResolveUrl(AuthenticationHelper.GetSecuredAreasLogonPage(new SiteInfoIdentifier(SiteContext.CurrentSiteID)));
        }
    }
}