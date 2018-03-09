using Kadena.BusinessLogic.Contracts;
using System.IdentityModel.Tokens;
using System.IO;
using System.Xml;
using System;
using Kadena.WebAPI.KenticoProviders.Contracts;
using System.Text;
using System.IdentityModel.Selectors;
using Kadena.Models.SiteSettings;
using Kadena.Models.SSO;

namespace Kadena.BusinessLogic.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly IKenticoLogger logger;
        private readonly IKenticoResourceService kenticoResource;

        public IdentityService(IKenticoLogger logger, IKenticoResourceService kenticoResource)
        {
            if (logger == null)
            {
                throw new ArgumentNullException(nameof(logger));
            }
            if (kenticoResource == null)
            {
                throw new ArgumentNullException(nameof(kenticoResource));
            }
            this.logger = logger;
            this.kenticoResource = kenticoResource;
        }


        public Uri TryAuthenticate(string samlString)
        {
            logger.LogInfo(this.GetType().Name, "SAMLAUTHENTICATE", samlString);
            Saml2SecurityToken token = null;
            try
            {
                token = GetToken(samlString);
            }
            catch (Exception e)
            {
                logger.LogException(this.GetType().Name, e);
                return new Uri("https://en.wikipedia.org/wiki/HTTP_403", UriKind.Absolute);
            }
            // create/update user
            // update roles
            // authenticate in Kentico
            return token != null ? new Uri("/", UriKind.Relative) : new Uri("https://en.wikipedia.org/wiki/HTTP_404", UriKind.Absolute);
        }

        private Saml2SecurityToken GetToken(string tokenString)
        {
            var thumbprint = kenticoResource.GetSettingsKey(Settings.KDA_TrustedCertificateThumbprint);
            var allowedAudienceUri = kenticoResource.GetSettingsKey(Settings.KDA_AllowedAudienceUri);

            var issuer = new ConfigurationBasedIssuerNameRegistry();
            issuer.AddTrustedIssuer(thumbprint, "IdPIssuer");

            var audience = new AudienceRestriction(AudienceUriMode.Always);
            audience.AllowedAudienceUris.Add(new Uri(allowedAudienceUri, UriKind.RelativeOrAbsolute));

            var handler = new KadenaSaml2SecurityTokenHandler
            {
                Configuration = new SecurityTokenHandlerConfiguration
                {
                    AudienceRestriction = audience,
                    IssuerNameRegistry = issuer
                }
            };
            using (var xmlReader = XmlReader.Create(new StringReader(Encoding.UTF8.GetString(Convert.FromBase64String(tokenString)))))
            {
                if (!xmlReader.ReadToFollowing("saml:Assertion"))
                {
                    throw new ArgumentException("Assertion not found!", nameof(tokenString));
                }
                return handler.ReadToken(xmlReader) as Saml2SecurityToken;
            }
        }
    }
}
