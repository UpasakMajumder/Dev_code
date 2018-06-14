using Kadena.Models;
using Kadena.Models.Site;
using System.Collections.Generic;

namespace Kadena.WebAPI.KenticoProviders.Contracts
{
    public interface IKenticoLocalizationProvider
    {
        Culture[] GetSiteCultures();
        DocumentLocalization[] GetDocumentLocalizationsByAlias(string aliasPath);
        IEnumerable<State> GetStates();
        IEnumerable<Country> GetCountries();
        bool IsCurrentCultureDefault();
        string GetContextCultureCode();
    }
}
