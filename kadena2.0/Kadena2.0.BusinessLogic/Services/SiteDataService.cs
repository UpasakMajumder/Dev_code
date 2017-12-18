using Kadena.Models.Site;
using Kadena.BusinessLogic.Contracts;
using Kadena.WebAPI.KenticoProviders.Contracts;
using System;

namespace Kadena.BusinessLogic.Services
{
    public class SiteDataService : ISiteDataService
    {
        private readonly IKenticoResourceService _kentico;

        public SiteDataService(IKenticoResourceService kentico)
        {
            if (kentico == null)
            {
                throw new ArgumentNullException(nameof(kentico));
            }

            _kentico = kentico;
        }  

        public ArtworkFtpSettings GetArtworkFtpSettings(int siteId)
        {
            var site = _kentico.GetKenticoSite(siteId);

            if (site == null)
                throw new ArgumentOutOfRangeException(nameof(siteId));

            bool enabled = _kentico.GetSettingsKey(site.Name, "KDA_FTPAW_Enabled").ToLower() == "true";

            var result = new ArtworkFtpSettings()
            {
                Enabled = enabled
            };

            if (enabled)
            {
                result.Ftp = new FtpCredentials()
                {
                    Url = _kentico.GetSettingsKey(site.Name, "KDA_FTPAW_Url"),
                    Login = _kentico.GetSettingsKey(site.Name, "KDA_FTPAW_Username"),
                    Password = _kentico.GetSettingsKey(site.Name, "KDA_FTPAW_Password"),
                };
            }

            return result;
        }

        public KenticoSite GetKenticoSite(int? siteId, string siteName)
        {
            KenticoSite site = null;

            if (siteId.HasValue)
            {
                site = _kentico.GetKenticoSite(siteId.Value);
            }

            if (site == null && !string.IsNullOrEmpty(siteName))
            {
                site = _kentico.GetKenticoSite(siteName);
            }

            return site;
        }
    }
}