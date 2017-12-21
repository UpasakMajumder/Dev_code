using CMS.Helpers;
using Kadena2.WebAPI.KenticoProviders.Contracts;

namespace Kadena2.WebAPI.KenticoProviders.Providers
{
    public class KenticoHelper : IKenticoHelper
    {
        public bool ValidateHash(string value, string hash)
        {
            return ValidationHelper.ValidateHash(value, hash);
        }
    }
}
