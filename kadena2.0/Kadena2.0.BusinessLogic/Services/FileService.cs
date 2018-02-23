using System;
using System.Threading.Tasks;
using Kadena.AmazonFileSystemProvider;
using Kadena.BusinessLogic.Contracts;
using Kadena.WebAPI.KenticoProviders.Contracts;
using Kadena2.MicroserviceClients.Contracts;

namespace Kadena.BusinessLogic.Services
{
    public class FileService : IFileService
    {
        private readonly IFileClient _fileClient;
        private readonly IKenticoResourceService _resources;
        private readonly IKenticoLogger _logger;

        public FileService(IFileClient fileClient, IKenticoResourceService resources, IKenticoLogger logger)
        {
            if (fileClient == null)
            {
                throw new ArgumentNullException(nameof(fileClient));
            }
            if (resources == null)
            {
                throw new ArgumentNullException(nameof(resources));
            }
            if (logger == null)
            {
                throw new ArgumentNullException(nameof(logger));
            }
            _fileClient = fileClient;
            _resources = resources;
            _logger = logger;
        }

        public async Task<string> GetUrlFromS3(string key)
        {
            var linkResult = await _fileClient.GetShortliveSecureLink(PathHelper.EnsureFullKey(key));

            if (!linkResult.Success || string.IsNullOrEmpty(linkResult.Payload))
            {
                _logger.LogError("GetUrlFromS3", "Failed to get link for file from S3.");
                return null;
            }

            return linkResult.Payload;
        }
    }
}
