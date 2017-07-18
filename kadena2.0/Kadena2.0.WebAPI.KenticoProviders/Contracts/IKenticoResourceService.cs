using Kadena.Models;

namespace Kadena.WebAPI.KenticoProviders.Contracts
{
    public interface IKenticoResourceService
    {
        string GetResourceString(string name);

        string GetSettingsKey(string key);

        string GetSettingsKey(string siteName, string key);

        KenticoSite GetKenticoSite();

        Currency GetSiteCurrency();

        string GetDefaultSiteCompanyName();

        string GetDefaultSitePersonalName();

        string GetDefaultCustomerCompanyName();

        int GetOrderStatusId(string name);
    }
}
