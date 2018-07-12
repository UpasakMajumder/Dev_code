using Kadena.Models.Site;

namespace Kadena.BusinessLogic.Contracts
{
    public interface ILocalizationService
    {
        LanguageSelectorItem[] GetUrlsForLanguageSelector(string aliasPath, string currentUrl);
    }
}
