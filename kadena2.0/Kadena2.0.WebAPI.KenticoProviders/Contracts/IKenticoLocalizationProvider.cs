using Kadena.Models.Site;

namespace Kadena.WebAPI.KenticoProviders.Contracts
{
    public interface IKenticoLocalizationProvider
    {
        LanguageSelectorItem[] GetUrlsForLanguageSelector(string aliasPath);
    }
}
