using AutoMapper;
using CMS.DocumentEngine;
using CMS.Globalization;
using CMS.Localization;
using CMS.Membership;
using CMS.SiteProvider;
using Kadena.Helpers;
using Kadena.Models;
using Kadena.Models.Site;
using Kadena.WebAPI.KenticoProviders.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Kadena.WebAPI.KenticoProviders
{
    public class KenticoLocalizationProvider : IKenticoLocalizationProvider
    {
        private readonly IMapper _mapper;

        public KenticoLocalizationProvider(IMapper mapper)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public IEnumerable<State> GetStates()
        {
            return StateInfoProvider
                .GetStates()
                .Columns("StateDisplayName", "StateId", "StateName", "StateCode", "CountryId")
                .Select<StateInfo, State>(s => _mapper.Map<State>(s));
        }

        public IEnumerable<Country> GetCountries()
        {
            return CountryInfoProvider
                .GetCountries()
                .Columns("CountryDisplayName", "CountryID", "CountryTwoLetterCode")
                .Select<CountryInfo, Country>(s => _mapper.Map<Country>(s));
        }

        public LanguageSelectorItem[] GetUrlsForLanguageSelector(string aliasPath, string currentUrl)
        {
            var urlParameters = UrlHelper.GetQueryFromUrl(currentUrl);

            var siteCultureCodes = CultureSiteInfoProvider.GetSiteCultureCodes(SiteContext.CurrentSiteName);
            var tree = new TreeProvider(MembershipContext.AuthenticatedUser);
            var documents = tree.SelectNodes()
                        .Path(aliasPath, PathTypeEnum.Explicit)
                        .OnSite(SiteContext.CurrentSiteName)
                        .AllCultures();

            var selectorItems = new List<LanguageSelectorItem>(siteCultureCodes.Count);
            foreach (var code in siteCultureCodes)
            {
                var localizedName = CultureInfoProvider.GetCultureInfo(code).CultureShortName;
                var url = "/";
                foreach (var document in documents)
                {
                    if (document.DocumentCulture == code)
                    {
                        // normalize path format
                        url = UrlHelper.GetPathFromUrl(document.DocumentUrlPath);
                        break;
                    }
                }

                url = $"{url}?{urlParameters}";
                url = UrlHelper.SetQueryParameter(url, "lang", code);

                selectorItems.Add(new LanguageSelectorItem
                {
                    Code = code,
                    Language = localizedName,
                    Url = url
                });
            }

            return selectorItems.ToArray();
        }

        public bool IsCurrentCultureDefault()
        {
            return SiteContext.CurrentSite.DefaultVisitorCulture == LocalizationContext.CurrentCulture.CultureCode;
        }

        public string GetContextCultureCode()
        {
            return LocalizationContext.CurrentCulture.CultureCode;
        }
    }
}
