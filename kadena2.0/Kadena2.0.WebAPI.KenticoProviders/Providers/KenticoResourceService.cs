using CMS.Helpers;
using CMS.DataEngine;
using CMS.SiteProvider;
using Kadena.Models;
using CMS.Ecommerce;
using System.Linq;
using Kadena.WebAPI.KenticoProviders.Contracts;
using Kadena.Models.Site;
using CMS.Localization;
using System;
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

        public KenticoSite GetKenticoSite()
        {
            return GetKenticoSite(SiteContext.CurrentSiteID);
        }

        public KenticoSite GetKenticoSite(string siteName)
        {
            var site = SiteInfoProvider.GetSiteInfo(siteName);
            return CreateKenticoSite(site);
        }

        public KenticoSite GetKenticoSite(int siteId)
        {
            var site = SiteInfoProvider.GetSiteInfo(siteId);
            return CreateKenticoSite(site);
        }

        private KenticoSite CreateKenticoSite(SiteInfo site)
        {
            if (site == null)
                return null;

            return new KenticoSite()
            {
                Id = site.SiteID,
                Name = site.SiteName,
                DisplayName = site.DisplayName,
                ErpCustomerId = GetSettingsKey(site.SiteName, "KDA_ErpCustomerId"),
                SiteDomain = site.DomainName,
                OrderManagerEmail = GetSettingsKey(site.SiteID, "KDA_OrderNotificationEmail")
            };
        }

        public Currency GetSiteCurrency()
        {
            var currency = ECommerceContext.CurrentCurrency;

            return new Currency()
            {
                Code = currency.CurrencyCode,
                Id = currency.CurrencyID
            };
        }

        public string GetDefaultSiteCompanyName()
        {
            return GetSettingsKey("KDA_CustomerFullName");
        }

        public string GetDefaultSitePersonalName()
        {
            return GetSettingsKey("KDA_CustomerPersonalName");
        }

        public int GetOrderStatusId(string name)
        {
            var status = OrderStatusInfoProvider.GetOrderStatuses().Where(s => s.StatusName == name).FirstOrDefault();
            return status?.StatusID ?? 0;
        }

        public string GetDefaultCustomerCompanyName()
        {
            return GetSettingsKey("KDA_ShippingAddress_DefaultCompanyName");
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

        public string GetCurrentSiteDomain()
        {
            return RequestContext.CurrentDomain;
        }

        public string GetContextCultureCode()
        {
            return LocalizationContext.CurrentCulture.CultureCode;
        }

        public string ResolveMacroString(string macroString)
        {
            return MacroResolver.Resolve(macroString, new MacroSettings { Culture = LocalizationContext.CurrentCulture.CultureCode });
        }
    }
}