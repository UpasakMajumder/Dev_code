using Kadena.BusinessLogic.Contracts.SSO;
using System;
using System.IdentityModel.Tokens;
using System.IO;
using System.Xml;

namespace Kadena.BusinessLogic.Services.SSO
{
    public class KadenaSaml2SecurityTokenHandler : Saml2SecurityTokenHandler
    {
        private readonly ISaml2RecipientValidator recipientValidator;

        public KadenaSaml2SecurityTokenHandler(ISaml2RecipientValidator recipientValidator) : base()
        {
            if (recipientValidator == null)
            {
                throw new ArgumentNullException(nameof(recipientValidator));
            }
            this.recipientValidator = recipientValidator;
        }

        protected override void ValidateConfirmationData(Saml2SubjectConfirmationData confirmationData)
        {
            if (!recipientValidator.ValidateRecipient(confirmationData.Recipient))
            {
                throw new SecurityTokenException($"Recipient {confirmationData.Recipient} is not confirmed.");
            }
        }

        public override SecurityToken ReadToken(string tokenString)
        {
            using (var stringReader = new StringReader(tokenString))
            {
                using (var xmlReader = XmlReader.Create(stringReader))
                {
                    if (!xmlReader.ReadToFollowing("saml:Assertion"))
                    {
                        throw new ArgumentException("SAML token is incomplete. Assertion not found!", nameof(tokenString));
                    }
                    return ReadToken(xmlReader);
                }
            }
        }
    }
}
