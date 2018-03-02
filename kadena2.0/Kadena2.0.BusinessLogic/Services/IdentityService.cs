using Kadena.BusinessLogic.Contracts;
using Kadena.Models.SiteSettings;
using Kadena.WebAPI.KenticoProviders.Contracts;
using System;

namespace Kadena.BusinessLogic.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly IKenticoResourceService kenticoResourceService;
        public IdentityService(IKenticoResourceService kenticoResourceService)
        {
            if (kenticoResourceService == null)
            {
                throw new ArgumentNullException(nameof(kenticoResourceService));
            }

            this.kenticoResourceService = kenticoResourceService;
        }

        public Uri GetIdentityProviderUrl()
        {
            var url = kenticoResourceService.GetSettingsKey(Settings.KDA_IdProviderUrl);
            if (string.IsNullOrWhiteSpace(url))
            {
                return null;
            }
            return new Uri(url, UriKind.RelativeOrAbsolute);
        }
    }
}
