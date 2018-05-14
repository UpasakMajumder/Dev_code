using Kadena.AmazonFileSystemProvider;
using Kadena.Models.SiteSettings;
using Kadena.WebAPI.KenticoProviders.Contracts;
using Kadena2.MicroserviceClients;
using Kadena2.MicroserviceClients.Contracts;
using System;
using System.IO;

namespace Kadena.BusinessLogic.Services
{
    public class PathService : IS3PathService
    {
        private readonly IS3PathService s3PathService;
        private readonly IKenticoResourceService resourceService;
        private readonly IKenticoCustomItemProvider customItemProvider;
        private readonly IFileClient fileClient;
        private readonly IKenticoSiteProvider siteProvider;

        private string _environmentFolder = null;
        private string _defaultSpecialFolder = null;
        private const string _environmentClass = "KDA.Environment";

        public PathService(IS3PathService s3PathService, IKenticoResourceService resourceService, IKenticoCustomItemProvider customItemProvider,
            IFileClient fileClient, IKenticoSiteProvider siteProvider)
        {
            this.s3PathService = s3PathService ?? throw new ArgumentNullException(nameof(s3PathService));
            this.resourceService = resourceService ?? throw new ArgumentNullException(nameof(resourceService));
            this.customItemProvider = customItemProvider ?? throw new ArgumentNullException(nameof(customItemProvider));
            this.fileClient = fileClient ?? throw new ArgumentNullException(nameof(fileClient));
            this.siteProvider = siteProvider ?? throw new ArgumentNullException(nameof(siteProvider));
        }

        private string EnvironmentFolder
        {
            get
            {
                if (_environmentFolder == null)
                {
                    var environmentId = resourceService.GetSiteSettingsKey<int>(Settings.KDA_EnvironmentId);
                    var environment = customItemProvider.GetItem<Models.Common.Environment>(environmentId, _environmentClass);

                    if (environment == null)
                    {
                        throw new NullReferenceException($"Environment with id '{environmentId}' can't be found.");
                    }

                    if (string.IsNullOrWhiteSpace(environment.AmazonS3Folder))
                    {
                        _environmentFolder = string.Empty;
                    }
                    else
                    {
                        _environmentFolder = $"{environment.AmazonS3Folder.Trim('/')}/";
                    }
                }
                return _environmentFolder;
            }
        }

        private string DefaultSpecialFolder
        {
            get
            {
                if (_defaultSpecialFolder == null)
                {
                    _defaultSpecialFolder = $"{EnvironmentFolder}media/";
                }
                return _defaultSpecialFolder;
            }
        }
        
        public string CurrentDirectory => s3PathService.CurrentDirectory;

        public string GetObjectKeyFromPath(string path, bool lower)
        {
            var key = s3PathService.GetObjectKeyFromPath(path, lower);
            var system = FileSystem.Create(key);
            if (system != null)
            {
                var site = siteProvider.GetCurrentSiteCodeName();
                var filename = Path.GetFileNameWithoutExtension(key);
                var extension = Path.GetExtension(key);
                var fileKeyResult = fileClient.GetFileKey(system, FileType.Original, site, filename, extension).Result;
                if (!fileKeyResult.Success)
                {
                    throw new InvalidOperationException(fileKeyResult.ErrorMessages);
                }
                return fileKeyResult.Payload;
            }
            if (key.StartsWith(DefaultSpecialFolder))
            {
                return key;
            }
            return $"{DefaultSpecialFolder}{key}";
        }

        public string GetPathFromObjectKey(string objectKey, bool absolute, bool directory, bool lower)
        {
            if (objectKey == null)
            {
                return null;
            }
            string nonEnvPath = objectKey;
            if (nonEnvPath.StartsWith(DefaultSpecialFolder))
            {
                nonEnvPath = nonEnvPath.Substring(DefaultSpecialFolder.Length);
            }
            return s3PathService.GetPathFromObjectKey(nonEnvPath, absolute, directory, lower);
        }

        public string GetValidPath(string path, bool lower)
        {
            return s3PathService.GetValidPath(path, lower);
        }
    }
}
