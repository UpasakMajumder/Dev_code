using Kadena.BusinessLogic.Contracts;
using System.IdentityModel.Tokens;
using System.IO;
using System.Xml;
using System;
using System.Linq;
using Kadena.WebAPI.KenticoProviders.Contracts;
using System.Text;
using System.IdentityModel.Selectors;
using Kadena.Models.SiteSettings;
using Kadena.Models.SSO;
using System.Collections.Generic;
using Kadena.Dto.SSO;
using AutoMapper;

namespace Kadena.BusinessLogic.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly IKenticoLogger logger;
        private readonly IKenticoResourceService kenticoResource;
        private readonly IKenticoUserProvider kenticoUser;
        private readonly IMapper mapper;

        public IdentityService(IKenticoLogger logger, IKenticoResourceService kenticoResource, IKenticoUserProvider kenticoUser, IMapper mapper)
        {
            if (logger == null)
            {
                throw new ArgumentNullException(nameof(logger));
            }
            if (kenticoResource == null)
            {
                throw new ArgumentNullException(nameof(kenticoResource));
            }
            if (kenticoUser == null)
            {
                throw new ArgumentNullException(nameof(kenticoUser));
            }
            if (mapper == null)
            {
                throw new ArgumentNullException(nameof(mapper));
            }
            this.logger = logger;
            this.kenticoResource = kenticoResource;
            this.kenticoUser = kenticoUser;
            this.mapper = mapper;
        }


        public Uri TryAuthenticate(string samlString)
        {
            Saml2SecurityToken token = null;
            try
            {
                token = GetSecuritToken(samlString);
                if (token != null)
                {
                    var attributes = GetAttributes(token);
                    // create/update user
                    // update roles
                    // authenticate in Kentico
                    new Uri("/", UriKind.Relative);
                }
                else
                {
                    new Uri("https://en.wikipedia.org/wiki/HTTP_404", UriKind.Absolute);
                }
            }
            catch (Exception e)
            {
                logger.LogException(this.GetType().Name, e);
            }
            return new Uri("https://en.wikipedia.org/wiki/HTTP_403", UriKind.Absolute);
        }

        private static Dictionary<string, IEnumerable<string>> GetAttributes(Saml2SecurityToken token)
        {
            if (token == null)
            {
                throw new ArgumentNullException(nameof(token));
            }

            var attributeStatement = token.Assertion.Statements
                .Select(s => s as Saml2AttributeStatement)
                .FirstOrDefault(s => s != null);

            if (attributeStatement == null)
            {
                throw new KeyNotFoundException("SAML token is incomplete. AttributeStatement not found.");
            }

            return attributeStatement.Attributes.ToDictionary(a => a.Name, a => a.Values.AsEnumerable());
        }

        private Saml2SecurityToken GetSecuritToken(string tokenString)
        {
            var tokenHandler = GetSecurityTokenHandler();
            var decodedTokenString = Encoding.UTF8.GetString(Convert.FromBase64String(tokenString));
            using (var stringReader = new StringReader(decodedTokenString))
            {
                using (var xmlReader = XmlReader.Create(stringReader))
                {
                    if (!xmlReader.ReadToFollowing("saml:Assertion"))
                    {
                        throw new ArgumentException("Assertion not found!", nameof(tokenString));
                    }
                    return tokenHandler.ReadToken(xmlReader) as Saml2SecurityToken;
                }
            }
        }

        private KadenaSaml2SecurityTokenHandler GetSecurityTokenHandler()
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
            return handler;
        }
    }
}
