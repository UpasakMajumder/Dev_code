using CMS.Helpers;
using CMS.DataEngine;
using CMS.SiteProvider;
using Kadena.WebAPI.KenticoProviders.Contracts;
using CMS.Localization;
using CMS.MacroEngine;
using System;
using CMS.Membership;
using CMS.Ecommerce;
using Kadena.Models.SiteSettings;
using Kadena.Models.SiteSettings.ErpMapping;
using Newtonsoft.Json;

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

        public string GetCustomerErpId()
        {
            var json = GetSiteSettingsKey(Settings.Kadena_ErpMapping);
            var value = JsonConvert.DeserializeObject<ErpSelectorValue>(json);
            return value?.CustomerErpId;
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
            SettingsKeyInfo settingsKey = null;

            if (siteId > 0)
            {
                settingsKey = SettingsKeyInfoProvider.GetSettingsKeyInfo(key, new SiteInfoIdentifier(siteId));
            }

            // If setting key on some site is just inherited from global,
            // SettingsKeyInfoProvider(name, siteIdentifier) will not find correct value.
            // So if KeyID == 0, try to take global value
            if (siteId == 0 || ((settingsKey?.KeyID ?? 0) == 0))
            {
                settingsKey = SettingsKeyInfoProvider.GetSettingsKeyInfo(key);
            }

            if (settingsKey == null || settingsKey.KeyID == 0)
            {
                throw new ArgumentOutOfRangeException(nameof(key), $"Settings key not found : {key}");
            }

            var value = settingsKey.KeyValue;

            if (value == null)
            {
                return default(T);
            }

            try
            {
                return (T)Convert.ChangeType(value, typeof(T));
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to convert key '{key}' = '{value}' to type '{typeof(T).Name}'", ex);
            }
        }

        public string ResolveMacroString(string macroString)
        {
            return MacroResolver.Resolve(macroString, new MacroSettings { Culture = LocalizationContext.CurrentCulture.CultureCode });
        }

        public string GetLogonPageUrl()
        {
            return URLHelper.ResolveUrl(AuthenticationHelper.GetSecuredAreasLogonPage(new SiteInfoIdentifier(SiteContext.CurrentSiteID)));
        }

        public string GetMassUnit()
        {
            return ECommerceSettings.MassUnit();
        }
    }
}