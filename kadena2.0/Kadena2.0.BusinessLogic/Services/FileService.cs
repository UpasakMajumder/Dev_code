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
        private readonly IKenticoLogger _logger;
        private readonly IS3PathService pathService;

        public FileService(IFileClient fileClient, IKenticoLogger logger, IS3PathService pathService)
        {
            _fileClient = fileClient ?? throw new ArgumentNullException(nameof(fileClient));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.pathService = pathService ?? throw new ArgumentNullException(nameof(pathService));
        }

        public async Task<string> GetUrlFromS3(string key)
        {
            var linkResult = await _fileClient.GetShortliveSecureLink(pathService.GetObjectKeyFromPath(key, true));

            if (!linkResult.Success || string.IsNullOrEmpty(linkResult.Payload))
            {
                _logger.LogError("GetUrlFromS3", "Failed to get link for file from S3.");
                return null;
            }

            return linkResult.Payload;
        }
    }
}
