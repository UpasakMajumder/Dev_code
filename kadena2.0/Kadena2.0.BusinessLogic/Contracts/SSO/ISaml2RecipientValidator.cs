using System;

namespace Kadena.BusinessLogic.Contracts.SSO
{
    public interface ISaml2RecipientValidator
    {
        bool ValidateRecipient(Uri recipientUri);
    }
}
