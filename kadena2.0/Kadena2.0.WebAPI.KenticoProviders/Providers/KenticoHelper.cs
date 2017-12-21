using CMS.Helpers;
using Kadena2.WebAPI.KenticoProviders.Contracts;
using System.IO;

namespace Kadena2.WebAPI.KenticoProviders.Providers
{
    public class KenticoHelper : IKenticoHelper
    {
        public string GetMimeType(string path)
        {
            return MimeTypeHelper.GetMimetype(Path.GetExtension(path));
        }

        public bool ValidateHash(string value, string hash)
        {
            return ValidationHelper.ValidateHash(value, hash);
        }
    }
}
