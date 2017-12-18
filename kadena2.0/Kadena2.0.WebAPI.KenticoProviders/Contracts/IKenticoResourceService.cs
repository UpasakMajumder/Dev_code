using Kadena.Models;
using Kadena.Models.Site;

namespace Kadena.WebAPI.KenticoProviders.Contracts
{
    public interface IKenticoResourceService
    {
        string GetResourceString(string name);

        string GetSettingsKey(string key);

        string GetSettingsKey(string siteName, string key);

        KenticoSite GetKenticoSite();

        KenticoSite GetKenticoSite(int siteId);

        KenticoSite GetKenticoSite(string siteName);

        Currency GetSiteCurrency();

        string GetDefaultSiteCompanyName();

        string GetDefaultSitePersonalName();

        string GetDefaultCustomerCompanyName();

        int GetOrderStatusId(string name);

        string GetSettingsKey(int siteId, string key);

        string GetCurrentSiteDomain();
        string GetContextCultureCode();

        string ResolveMacroString(string macroString);
    }
}
