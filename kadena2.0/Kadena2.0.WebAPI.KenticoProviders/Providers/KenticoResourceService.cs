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

        public string GetSiteSettingsKey(string key)
        {
            return GetSettingsKey<string>(key, SiteContext.CurrentSiteID);
        }

        public T GetSiteSettingsKey<T>(string key) where T : IConvertible
        {
            return GetSettingsKey<T>(key, SiteContext.CurrentSiteID);
        }        
        
        public T GetSettingsKey<T>(string key, int siteId = 0) where T : IConvertible
        {
            var value = (siteId > 0)
                ? SettingsKeyInfoProvider.GetValue(key, new SiteInfoIdentifier(siteId))
                : SettingsKeyInfoProvider.GetValue(key);

            if (value == null)
            {
                return default(T);
            }

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