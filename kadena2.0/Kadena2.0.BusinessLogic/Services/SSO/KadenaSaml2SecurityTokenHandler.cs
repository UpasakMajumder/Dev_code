using Kadena.BusinessLogic.Contracts.SSO;
using System;
using System.IdentityModel.Tokens;

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
    }
}
