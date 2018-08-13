using AutoMapper;
using CMS.Ecommerce;
using CMS.Helpers;
using CMS.SiteProvider;
using Kadena.Models;
using Kadena.Models.Site;
using Kadena.WebAPI.KenticoProviders.Contracts;
using Kadena2.WebAPI.KenticoProviders.Contracts.KadenaSettings;
using System;
using System.Linq;

namespace Kadena.WebAPI.KenticoProviders
{
    public class KenticoSiteProvider : IKenticoSiteProvider
    {
        private readonly IMapper mapper;
        private readonly IKadenaSettings kadenaSettings;

        public KenticoSiteProvider(IMapper mapper, IKadenaSettings kadenaSettings)
        {
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this.kadenaSettings = kadenaSettings ?? throw new ArgumentNullException(nameof(kadenaSettings));
        }

        public KenticoSite[] GetSites()
        {
            return mapper.Map<KenticoSite[]>(SiteInfoProvider.GetSites().ToArray());
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

        public string GetCurrentSiteDomain()
        {
            return RequestContext.CurrentDomain;
        }

        public string GetCurrentSiteCodeName()
        {
            return SiteContext.CurrentSiteName;
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

        private KenticoSite CreateKenticoSite(SiteInfo site)
        {
            var kenticoSite = mapper.Map<KenticoSite>(site);

            if (kenticoSite != null)
            {
                kenticoSite.OrderManagerEmail = kadenaSettings.OrderNotificationEmail;
                kenticoSite.ErpCustomerId = kadenaSettings.ErpCustomerId;
            }
            return kenticoSite;
        }

        public string GetFullUrl()
        {
            return URLHelper.GetFullApplicationUrl();
        }

        public string GetAbsoluteUrl(string url)
        {
            return URLHelper.GetAbsoluteUrl(url);
        }

        public string GetFormattedPrice(decimal price)
        {
            return CurrencyInfoProvider.GetFormattedPrice(ValidationHelper.GetDouble(price, default(double)), SiteContext.CurrentSiteID, true);
        }
    }
}
