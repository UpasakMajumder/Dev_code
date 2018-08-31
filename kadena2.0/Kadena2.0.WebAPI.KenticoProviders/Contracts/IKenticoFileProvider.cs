using System.IO;

namespace Kadena.WebAPI.KenticoProviders.Contracts
{
    public interface IKenticoFileProvider
    {
        void CreateFile(string filePath, Stream fileStream);

        string GetFileUrl(string path);
    }
}
