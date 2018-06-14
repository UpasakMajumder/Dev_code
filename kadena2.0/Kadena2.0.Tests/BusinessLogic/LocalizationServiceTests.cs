using Kadena.BusinessLogic.Services;
using Kadena.Models.Site;
using Kadena.WebAPI.KenticoProviders.Contracts;
using Xunit;

namespace Kadena.Tests.BusinessLogic
{
    public class LocalizationServiceTests : KadenaUnitTest<LocalizationService>
    {
        [Fact]
        public void GetUrlsForLanguageSelector_Should()
        {
            var currentUrl = "/some/doc?par1=val1";
            var alias = "/some/doc";

            var cultures = new[] 
            {
                new Culture { Code = "BB", ShortName = "BBName" },
                new Culture { Code = "CC", ShortName = "CCName" },
                new Culture { Code = "DD", ShortName = "DDName" },
            };
            Setup<IKenticoLocalizationProvider, Culture[]>(klp => klp.GetSiteCultures(), cultures);

            var localizations = new[] 
            {
                new DocumentLocalization { CultureCode = "BB", UrlPath = "/some/doc" },
                new DocumentLocalization { CultureCode = "CC", UrlPath = "/some/doc-cc" },
                new DocumentLocalization { CultureCode = "DD", UrlPath = "/some/doc-dd" },
            };
            Setup<IKenticoLocalizationProvider, DocumentLocalization[]>(klp => klp.GetDocumentLocalizationsByAlias("/some/doc"), localizations);

            var expected = new[]
            {
                new LanguageSelectorItem
                {
                    Code = "BB",
                    Language = "BBName",
                    Url =  "/some/doc?par1=val1&lang=BB"
                },
                new LanguageSelectorItem
                {
                    Code = "CC",
                    Language = "CCName",
                    Url =  "/some/doc-cc?par1=val1&lang=CC"
                },
                new LanguageSelectorItem
                {
                    Code = "DD",
                    Language = "DDName",
                    Url =  "/some/doc-dd?par1=val1&lang=DD"
                },
            };

            var result = Sut.GetUrlsForLanguageSelector(alias, currentUrl);

            bool LanguageSelectorItemComparer(LanguageSelectorItem a, LanguageSelectorItem b)
                => a.Code == b.Code
                && a.Language == b.Language
                && a.Url == b.Url;

            Assert.Contains(result, r => LanguageSelectorItemComparer(r, expected[0]));
            Assert.Contains(result, r => LanguageSelectorItemComparer(r, expected[1]));
            Assert.Contains(result, r => LanguageSelectorItemComparer(r, expected[2]));
        }
    }
}
