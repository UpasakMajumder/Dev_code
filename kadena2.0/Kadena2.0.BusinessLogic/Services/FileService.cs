using System;
using System.Threading.Tasks;
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
        private readonly IPathService pathService;

        public FileService(IFileClient fileClient, IKenticoResourceService resources, IKenticoLogger logger, IPathService pathService)
        {
            _fileClient = fileClient ?? throw new ArgumentNullException(nameof(fileClient));
            _resources = resources ?? throw new ArgumentNullException(nameof(resources));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.pathService = pathService ?? throw new ArgumentNullException(nameof(pathService));
        }

        public async Task<string> GetUrlFromS3(string key)
        {
            var linkResult = await _fileClient.GetShortliveSecureLink(pathService.EnsureFullKey(key));

            if (!linkResult.Success || string.IsNullOrEmpty(linkResult.Payload))
            {
                _logger.LogError("GetUrlFromS3", "Failed to get link for file from S3.");
                return null;
            }

            return linkResult.Payload;
        }
    }
}
