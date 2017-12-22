using Kadena.Models.Site;
using Kadena.BusinessLogic.Contracts;
using Kadena.WebAPI.KenticoProviders.Contracts;
using System;
using Kadena2.WebAPI.KenticoProviders.Contracts.KadenaSettings;

namespace Kadena.BusinessLogic.Services
{
    public class SiteDataService : ISiteDataService
    {
        private readonly IKenticoSiteProvider _site;
        private readonly IKadenaSettings _settings;

        public SiteDataService(IKenticoSiteProvider site, IKadenaSettings settings)
        {
            if (site == null)
            {
                throw new ArgumentNullException(nameof(site));
            }
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            _site = site;
            _settings = settings;
        }  

        public ArtworkFtpSettings GetArtworkFtpSettings(int siteId)
        {
            var site = _site.GetKenticoSite(siteId);

            if (site == null)
                throw new ArgumentOutOfRangeException(nameof(siteId));

            bool enabled = _settings.FTPArtworkEnabled(site.Name);

            var result = new ArtworkFtpSettings()
            {
                Enabled = enabled
            };

            if (enabled)
            {
                result.Ftp = new FtpCredentials()
                {
                    Url = _settings.FTPArtworkUrl(site.Name),
                    Login = _settings.FTPArtworkUsername(site.Name),
                    Password = _settings.FTPArtworkPassword(site.Name)
                };
            }

            return result;
        }

        public KenticoSite GetKenticoSite(int? siteId, string siteName)
        {
            KenticoSite site = null;

            if (siteId.HasValue)
            {
                site = _site.GetKenticoSite(siteId.Value);
            }

            if (site == null && !string.IsNullOrEmpty(siteName))
            {
                site = _site.GetKenticoSite(siteName);
            }

            return site;
        }
    }
}