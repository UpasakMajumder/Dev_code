using Kadena.Models.Site;
using Kadena.WebAPI.Contracts;
using Kadena.WebAPI.KenticoProviders.Contracts;
using System;

namespace Kadena.WebAPI.Services
{
    public class SiteDataService : ISiteDataService
    {
        private readonly IKenticoResourceService _kentico;

        public SiteDataService(IKenticoResourceService kentico)
        {
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

        public KenticoSite GetKenticoSite(int siteId)
        {
            return _kentico.GetKenticoSite(siteId);
        }
    }
}