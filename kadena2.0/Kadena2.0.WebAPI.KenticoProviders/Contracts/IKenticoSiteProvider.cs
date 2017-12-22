using Kadena.Models;
using Kadena.Models.Site;

namespace Kadena.WebAPI.KenticoProviders.Contracts
{
    public interface IKenticoSiteProvider
    {
        Site[] GetSites();
        string GetCurrentSiteCodeName();
        KenticoSite GetKenticoSite();
        KenticoSite GetKenticoSite(int siteId);
        KenticoSite GetKenticoSite(string siteName);
        Currency GetSiteCurrency();
        string GetCurrentSiteDomain();
    }
}
