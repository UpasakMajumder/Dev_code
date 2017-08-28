using CMS.SiteProvider;
using Kadena.Models;

namespace Kadena2.WebAPI.KenticoProviders.Factories
{
    public class SiteFactory
    {
        public static Site CreateSite(SiteInfo info)
        {
            if (info == null)
            {
                return null;
            }

            return new Site
            {
                Id = info.SiteID,
                Name = info.SiteName
            };
        }
    }
}