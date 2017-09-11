using System.Security.Claims;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;

namespace Kadena.WebAPI.Infrastructure.Filters
{
    public class IdentityBasicAuthenticationAttribute : BasicAuthenticationAttribute
    {
        protected override async Task<IPrincipal> AuthenticateAsync(string userName, string password, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (userName == "root" && password == "r")
            {
                ClaimsIdentity identity = new ClaimsIdentity("Basic");
                await Task.FromResult(new ClaimsPrincipal(identity));
            }

            return null;
        }
    }
}