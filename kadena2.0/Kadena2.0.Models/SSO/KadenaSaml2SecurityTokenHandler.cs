using Kadena.Helpers;
using System.IdentityModel.Tokens;

namespace Kadena.Models.SSO
{
    public class KadenaSaml2SecurityTokenHandler : Saml2SecurityTokenHandler
    {
        protected override void ValidateConfirmationData(Saml2SubjectConfirmationData confirmationData)
        {
            if (confirmationData.Recipient != UrlHelper.GetACSUri())
            {
                throw new SecurityTokenException("Recipient is not confirmed.");
            }
        }
    }
}
