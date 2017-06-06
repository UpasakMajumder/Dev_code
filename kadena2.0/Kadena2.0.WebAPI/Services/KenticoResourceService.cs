using Kadena.WebAPI.Contracts;
using CMS.Helpers;
using CMS.DataEngine;
using CMS.SiteProvider;
using Kadena.WebAPI.Models;
using CMS.Ecommerce;
using System;
using System.Linq;

namespace Kadena.WebAPI.Services
{
    public class KenticoResourceService : IResourceService
    {
        public string GetResourceString(string name)
        {
            return ResHelper.GetString(name, useDefaultCulture:true);
        }

        public string GetSettingsKey(string key)
        {
            string resourceKey = $"{SiteContext.CurrentSiteName}.{key}";
            return SettingsKeyInfoProvider.GetValue(resourceKey);
        }

        public KenticoSite GetKenticoSite()
        {
            return new KenticoSite()
            {
                Id = SiteContext.CurrentSiteID,
                Name = SiteContext.CurrentSiteName
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

            if (status == null)
                return 0;

            return status.StatusID;
        }
    }
}