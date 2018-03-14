using System.IdentityModel.Tokens;

namespace Kadena.BusinessLogic.Contracts.SSO
{
    public interface ISaml2HandlerService
    {
        Saml2SecurityTokenHandler GetTokenHandler();
    }
}
