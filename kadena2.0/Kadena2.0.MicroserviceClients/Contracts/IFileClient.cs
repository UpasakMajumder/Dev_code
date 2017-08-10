using System.IO;

namespace Kadena2.MicroserviceClients.Contracts
{
    public interface IFileClient
    {
        string UploadToS3(string serviceEndpoint, string siteName, string folderName, string moduleName,
            Stream fileStream, string fileName);

        string GetFileUrl(string serviceEndpoint, string fileName, string moduleName);
    }
}
