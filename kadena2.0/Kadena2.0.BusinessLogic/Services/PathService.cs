using Kadena.AmazonFileSystemProvider;
using Kadena.BusinessLogic.Contracts;
using Kadena.Models.SiteSettings;
using Kadena.WebAPI.KenticoProviders.Contracts;
using System;

namespace Kadena.BusinessLogic.Services
{
    public class PathService : IPathService
    {
        private readonly IS3PathService s3PathService;
        private readonly IKenticoResourceService resourceService;
        private readonly IKenticoCustomItemProvider customItemProvider;

        private string _environmentFolder = null;
        private string _defaultSpecialFolder = null;
        private const string _environmentClass = "KDA.Environment";

        public PathService(IS3PathService s3PathService, IKenticoResourceService resourceService, IKenticoCustomItemProvider customItemProvider)
        {
            this.s3PathService = s3PathService ?? throw new ArgumentNullException(nameof(s3PathService));
            this.resourceService = resourceService ?? throw new ArgumentNullException(nameof(resourceService));
            this.customItemProvider = customItemProvider ?? throw new ArgumentNullException(nameof(customItemProvider));
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
            return $"{DefaultSpecialFolder}{s3PathService.GetObjectKeyFromPath(path, lower)}";
        }

        public string GetObjectKeyFromPathNonEnvironment(string path, bool lower = true)
        {
            return s3PathService.GetObjectKeyFromPathNonEnvironment(path, lower);
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

        public string EnsureFullKey(string key)
        {
            if (key.StartsWith(DefaultSpecialFolder))
            {
                return key;
            }
            return $"{DefaultSpecialFolder}{key}";
        }
    }
}
