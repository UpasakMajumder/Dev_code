using Kadena.BusinessLogic.Contracts;
using Kadena.Helpers;
using Kadena.Models.Site;
using Kadena.WebAPI.KenticoProviders.Contracts;
using System;
using System.Collections.Generic;

namespace Kadena.BusinessLogic.Services
{
    public class LocalizationService : ILocalizationService
    {
        private readonly IKenticoLocalizationProvider kenticoLocalizationProvider;

        public LocalizationService(IKenticoLocalizationProvider kenticoLocalizationProvider)
        {
            this.kenticoLocalizationProvider = kenticoLocalizationProvider ?? throw new ArgumentNullException(nameof(kenticoLocalizationProvider));
        }

        public LanguageSelectorItem[] GetUrlsForLanguageSelector(string aliasPath, string currentUrl)
        {
            var urlParameters = UrlHelper.GetQueryFromUrl(currentUrl);
            var siteCultures = kenticoLocalizationProvider.GetSiteCultures();
            var documentLocalizations = kenticoLocalizationProvider.GetDocumentLocalizationsByAlias(aliasPath);
            var selectorItems = new List<LanguageSelectorItem>(siteCultures.Length);
            foreach (var culture in siteCultures)
            {
                var url = "/";
                foreach (var document in documentLocalizations)
                {
                    if (document.CultureCode == culture.Code)
                    {
                        // normalize path format
                        url = UrlHelper.GetPathFromUrl(document.UrlPath);
                        break;
                    }
                }

                url = $"{url}?{urlParameters}";
                url = UrlHelper.SetQueryParameter(url, "lang", culture.Code);

                selectorItems.Add(new LanguageSelectorItem
                {
                    Code = culture.Code,
                    Language = culture.ShortName,
                    Url = url
                });
            }

            return selectorItems.ToArray();
        }
    }
}
