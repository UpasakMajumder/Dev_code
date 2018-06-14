using Kadena.Models;
using Kadena.Models.Site;
using System.Collections.Generic;

namespace Kadena.WebAPI.KenticoProviders.Contracts
{
    public interface IKenticoLocalizationProvider
    {
        LanguageSelectorItem[] GetUrlsForLanguageSelector(string aliasPath, string currentUrl);
        IEnumerable<State> GetStates();
        IEnumerable<Country> GetCountries();
        bool IsCurrentCultureDefault();
        string GetContextCultureCode();
    }
}
