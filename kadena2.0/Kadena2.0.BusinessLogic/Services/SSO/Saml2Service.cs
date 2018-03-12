using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using Kadena.BusinessLogic.Contracts.SSO;
using Kadena.WebAPI.KenticoProviders.Contracts;

namespace Kadena.BusinessLogic.Services.SSO
{
    public class Saml2Service : ISaml2Service
    {
        private readonly IKenticoLogger logger;
        private readonly ISaml2HandlerService tokenHandlerService;

        public Saml2Service(IKenticoLogger logger, ISaml2HandlerService tokenHandlerService)
        {
            if (logger == null)
            {
                throw new ArgumentNullException(nameof(logger));
            }
            if (tokenHandlerService == null)
            {
                throw new ArgumentNullException(nameof(tokenHandlerService));
            }
            this.logger = logger;
            this.tokenHandlerService = tokenHandlerService;
        }

        public Dictionary<string, IEnumerable<string>> GetAttributes(string samlString)
        {
            Saml2SecurityToken token = null;
            try
            {
                token = GetSecurityToken(samlString);
            }
            catch (Exception e)
            {
                logger.LogException(this.GetType().Name, e);
                return null;
            }

            var attributeStatement = token.Assertion.Statements
                .Select(s => s as Saml2AttributeStatement)
                .FirstOrDefault(s => s != null);

            if (attributeStatement == null)
            {
                logger.LogError(this.GetType().Name, "SAML token is incomplete. AttributeStatement not found.");
                return null;
            }

            return attributeStatement.Attributes.ToDictionary(a => a.Name, a => a.Values.AsEnumerable());
        }

        private Saml2SecurityToken GetSecurityToken(string tokenString)
        {
            var tokenHandler = tokenHandlerService.GetTokenHandler();
            var decodedTokenString = Encoding.UTF8.GetString(Convert.FromBase64String(tokenString));
            using (var stringReader = new StringReader(decodedTokenString))
            {
                using (var xmlReader = XmlReader.Create(stringReader))
                {
                    if (!xmlReader.ReadToFollowing("saml:Assertion"))
                    {
                        throw new ArgumentException("SAML token is incomplete. Assertion not found!", nameof(tokenString));
                    }
                    var token = tokenHandler.ReadToken(xmlReader) as Saml2SecurityToken;
                    tokenHandler.ValidateToken(token);
                    return token;
                }
            }
        }
    }
}
