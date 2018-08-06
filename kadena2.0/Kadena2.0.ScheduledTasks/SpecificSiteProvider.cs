using Kadena.Models;
using Kadena.Models.Site;
using Kadena.WebAPI.KenticoProviders.Contracts;
using System;

namespace Kadena.ScheduledTasks
{
    class SpecificSiteProvider : IKenticoSiteProvider
    {
        private readonly IKenticoSiteProvider kenticoSiteProvider;
        private readonly KenticoSite site;

        public SpecificSiteProvider(KenticoSite site, IKenticoSiteProvider kenticoSiteProvider)
        {
            this.site = site ?? throw new ArgumentNullException(nameof(site));
            this.kenticoSiteProvider = kenticoSiteProvider ?? throw new ArgumentNullException(nameof(kenticoSiteProvider));
        }

        public string GetAbsoluteUrl(string url)
        {
            throw new NotImplementedException();
        }

        public string GetCurrentSiteCodeName()
        {
            return site.Name;
        }

        public string GetCurrentSiteDomain()
        {
            throw new NotImplementedException();
        }

        public string GetFormattedPrice(decimal price)
        {
            throw new NotImplementedException();
        }

        public string GetFullUrl()
        {
            throw new NotImplementedException();
        }

        public KenticoSite GetKenticoSite()
        {
            return site;
        }

        public KenticoSite GetKenticoSite(int siteId)
        {
            return kenticoSiteProvider.GetKenticoSite(siteId);
        }

        public KenticoSite GetKenticoSite(string siteName)
        {
            return kenticoSiteProvider.GetKenticoSite(siteName);
        }

        public Currency GetSiteCurrency()
        {
            throw new NotImplementedException();
        }

        public KenticoSite[] GetSites()
        {
            return kenticoSiteProvider.GetSites();
        }
    }
}
