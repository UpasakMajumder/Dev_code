using AutoMapper;
using CMS.CustomTables;
using CMS.DataEngine;
using CMS.DocumentEngine;
using CMS.Ecommerce;
using CMS.Globalization;
using CMS.Helpers;
using CMS.Localization;
using CMS.MacroEngine;
using CMS.Membership;
using CMS.SiteProvider;
using Kadena.Models;
using Kadena.Models.Checkout;
using Kadena.Models.Product;
using Kadena.Models.Site;
using Kadena.WebAPI.KenticoProviders.Contracts;
using Kadena2.WebAPI.KenticoProviders.Factories;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Kadena.WebAPI.KenticoProviders
{
    public class KenticoProviderService : IKenticoProviderService
    {
        private readonly IMapper _mapper;

        public KenticoProviderService(IMapper mapper)
        {
            if (mapper == null)
            {
                throw new ArgumentNullException(nameof(mapper));
            }

            this._mapper = mapper;
        }

        public IEnumerable<State> GetStates()
        {
            return StateInfoProvider
                .GetStates()
                .Columns("StateId", "StateName", "StateCode", "CountryId")
                .Select<StateInfo, State>(s => _mapper.Map<State>(s));
        }
        public bool IsAuthorizedPerResource(string resourceName, string permissionName, string siteName)
        {
            return MembershipContext.AuthenticatedUser.IsAuthorizedPerResource(resourceName, permissionName, siteName);
        }
        public Site[] GetSites()
        {
            var sites = SiteInfoProvider.GetSites()
                .Select(s => SiteFactory.CreateSite(s))
                .ToArray();
            return sites;
        }
        public IEnumerable<Country> GetCountries()
        {
            return CountryInfoProvider
                .GetCountries()
                .Columns("CountryDisplayName", "CountryID", "CountryTwoLetterCode")
                .Select<CountryInfo, Country>(s => _mapper.Map<Country>(s));
        }
        public string GetCurrentSiteCodeName()
        {
            return SiteContext.CurrentSiteName;
        }
        public bool IsCurrentCultureDefault()
        {
            return SiteContext.CurrentSite.DefaultVisitorCulture == LocalizationContext.CurrentCulture.CultureCode;
        }
    }
}