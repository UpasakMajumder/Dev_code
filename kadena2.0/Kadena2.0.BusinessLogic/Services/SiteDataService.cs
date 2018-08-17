using Kadena.Models.Site;
using Kadena.BusinessLogic.Contracts;
using Kadena.WebAPI.KenticoProviders.Contracts;
using System;
using Kadena2.WebAPI.KenticoProviders.Contracts.KadenaSettings;
using AutoMapper;

namespace Kadena.BusinessLogic.Services
{
    public class SiteDataService : ISiteDataService
    {
        private readonly IKenticoSiteProvider _site;
        private readonly IKadenaSettings _settings;
        private readonly IShoppingCartProvider _shoppingCartProvider;
        private readonly IMapper _mapper;

        public SiteDataService(IKenticoSiteProvider site, IKadenaSettings settings, IShoppingCartProvider shoppingCartProvider, IMapper mapper)
        {
            _site = site ?? throw new ArgumentNullException(nameof(site));
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));
            _shoppingCartProvider = shoppingCartProvider ?? throw new ArgumentNullException(nameof(shoppingCartProvider));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }  

        public ArtworkFtpSettings GetArtworkFtpSettings(int siteId)
        {
            var site = _site.GetKenticoSite(siteId);

            if (site == null)
                throw new ArgumentOutOfRangeException(nameof(siteId));

            bool enabled = _settings.FTPArtworkEnabled(site.Id);

            var result = new ArtworkFtpSettings()
            {
                Enabled = enabled
            };

            if (enabled)
            {
                result.Ftp = new FtpCredentials()
                {
                    Url = _settings.FTPArtworkUrl(site.Id),
                    Login = _settings.FTPArtworkUsername(site.Id),
                    Password = _settings.FTPArtworkPassword(site.Id)
                };
            }

            return result;
        }

        public KenticoSiteWithDeliveryOptions GetKenticoSite(int? siteId, string siteName)
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

            var result = _mapper.Map<KenticoSiteWithDeliveryOptions>(site);

            if(site != null)
            {
                result.DeliveryOptions = _shoppingCartProvider.GetShippingOptions();
            }

            return result;
        }
    }
}