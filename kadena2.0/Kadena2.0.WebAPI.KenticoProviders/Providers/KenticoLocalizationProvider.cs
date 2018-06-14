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

        public Culture[] GetSiteCultures()
        {
            return CultureSiteInfoProvider
                .GetSiteCultures(SiteContext.CurrentSiteName)
                .Select(c => _mapper.Map<Culture>(c))
                .ToArray();
        }

        public DocumentLocalization[] GetDocumentLocalizationsByAlias(string aliasPath)
        {
            var tree = new TreeProvider(MembershipContext.AuthenticatedUser);
            return tree.SelectNodes()
                .Path(aliasPath, PathTypeEnum.Explicit)
                .OnSite(SiteContext.CurrentSiteName)
                .AllCultures()
                .ToList()
                .Select(node => _mapper.Map<DocumentLocalization>(node))
                .ToArray();
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
