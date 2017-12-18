using CMS.DocumentEngine;
using CMS.Localization;
using CMS.Membership;
using CMS.SiteProvider;
using Kadena.Models.Site;
using Kadena.WebAPI.KenticoProviders.Contracts;
using System.Collections.Generic;

namespace Kadena.WebAPI.KenticoProviders
{
    public class KenticoLocalizationProvider : IKenticoLocalizationProvider
    {
        public LanguageSelectorItem[] GetUrlsForLanguageSelector(string aliasPath)
        {
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
                var localizedFound = false;
                foreach (var document in documents)
                {
                    if (document.DocumentCulture == code)
                    {
                        localizedFound = true;
                        selectorItems.Add(new LanguageSelectorItem
                        {
                            Code = code,
                            Language = localizedName,
                            Url = document.DocumentUrlPath + "?lang=" + code
                        });
                    }
                }

                if (!localizedFound)
                {
                    selectorItems.Add(new LanguageSelectorItem
                    {
                        Code = code,
                        Language = localizedName,
                        Url = "/?lang=" + code
                    });
                }
            }

            return selectorItems.ToArray();
        }
    }
}
