using System;
using System.IdentityModel.Selectors;
using System.IdentityModel.Tokens;
using Kadena.BusinessLogic.Contracts.SSO;
using Kadena.Models.SiteSettings;
using Kadena.WebAPI.KenticoProviders.Contracts;

namespace Kadena.BusinessLogic.Services.SSO
{
    public class Saml2HandlerService : ISaml2HandlerService
    {
        private readonly IKenticoResourceService kenticoResource;
        private readonly Saml2SecurityTokenHandler saml2SecurityTokenHandler;

        public Saml2HandlerService(IKenticoResourceService kenticoResource, Saml2SecurityTokenHandler saml2SecurityTokenHandler)
        {
            if (kenticoResource == null)
            {
                throw new ArgumentNullException(nameof(kenticoResource));
            }
            if (saml2SecurityTokenHandler == null)
            {
                throw new ArgumentNullException(nameof(saml2SecurityTokenHandler));
            }
            this.kenticoResource = kenticoResource;
            this.saml2SecurityTokenHandler = saml2SecurityTokenHandler;
        }

        public Saml2SecurityTokenHandler GetTokenHandler()
        {
            var thumbprint = kenticoResource.GetSiteSettingsKey(Settings.KDA_TrustedCertificateThumbprint);
            var allowedAudienceUri = kenticoResource.GetSiteSettingsKey(Settings.KDA_AllowedAudienceUri);

            var issuer = new ConfigurationBasedIssuerNameRegistry();
            issuer.AddTrustedIssuer(thumbprint, "IdPIssuer");

            var audience = new AudienceRestriction(AudienceUriMode.Always);
            audience.AllowedAudienceUris.Add(new Uri(allowedAudienceUri, UriKind.RelativeOrAbsolute));

            saml2SecurityTokenHandler.Configuration = new SecurityTokenHandlerConfiguration
            {
                AudienceRestriction = audience,
                IssuerNameRegistry = issuer
            };

            return saml2SecurityTokenHandler;
        }
    }
}
