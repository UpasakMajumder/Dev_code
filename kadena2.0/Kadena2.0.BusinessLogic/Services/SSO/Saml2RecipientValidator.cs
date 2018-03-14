using Kadena.BusinessLogic.Contracts.SSO;
using Kadena.Helpers;
using Kadena.WebAPI.KenticoProviders.Contracts;
using System;

namespace Kadena.BusinessLogic.Services.SSO
{
    public class Saml2RecipientValidator : ISaml2RecipientValidator
    {
        private IKenticoSiteProvider kenticoSiteProvider;

        public Saml2RecipientValidator(IKenticoSiteProvider kenticoSiteProvider)
        {
            if (kenticoSiteProvider == null)
            {
                throw new ArgumentNullException(nameof(kenticoSiteProvider));
            }
            this.kenticoSiteProvider = kenticoSiteProvider;
        }

        public bool ValidateRecipient(Uri recipientUri)
        {
            var baseUri = new Uri(kenticoSiteProvider.GetFullUrl(), UriKind.Absolute);
            var targetUri = new Uri(baseUri, UrlHelper.GetACSUri());
            return recipientUri.Equals(targetUri);
        }
    }
}
