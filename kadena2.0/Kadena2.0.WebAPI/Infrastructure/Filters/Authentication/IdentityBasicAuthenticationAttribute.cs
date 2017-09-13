using CMS.DataEngine;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;

namespace Kadena.WebAPI.Infrastructure.Filters.Authentication
{
    public class IdentityBasicAuthenticationAttribute : BasicAuthenticationAttribute
    {
        protected override async Task<IPrincipal> AuthenticateAsync(string userName, string password, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            string storedUser = SettingsKeyInfoProvider.GetValue("KDA_WebApiUser");
            string storedPassword = SettingsKeyInfoProvider.GetValue("KDA_WebApiPassword");

            if (userName == storedUser && password == storedPassword)
            {
                ClaimsIdentity identity = new ClaimsIdentity("Basic");
                return await Task.FromResult(new ClaimsPrincipal(identity));
            }

            return null;
        }
    }
}