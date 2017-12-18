using Kadena.Models;
using Kadena.Models.Site;
using System.Collections.Generic;

namespace Kadena.WebAPI.KenticoProviders.Contracts
{
    public interface IKenticoProviderService
    {
        IEnumerable<State> GetStates();

        IEnumerable<Country> GetCountries();

        bool IsAuthorizedPerResource(string resourceName, string permissionName, string siteName);

        Site[] GetSites();

        string GetCurrentSiteCodeName();

        bool IsCurrentCultureDefault();
    }
}
