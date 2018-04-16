using System.IO;

namespace Kadena.WebAPI.KenticoProviders.Contracts
{
    public interface IKenticoFileProvider
    {
        string GetFileUrl(string path);
    }
}
