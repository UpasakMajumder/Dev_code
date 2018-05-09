using CMS;
using CMS.CustomTables;
using CMS.DataEngine;
using CMS.EventLog;
using CMS.IO;
using Kadena.AmazonFileSystemProvider;
using Kadena.BusinessLogic.Contracts;
using Kadena.Container.Default;
using Kadena.CustomTables;
using Kadena.Models.SiteSettings;
using System;

[assembly: RegisterModule(typeof(Kadena.Old_App_Code.CMSModules.S3StorageProviderModule))]
namespace Kadena.Old_App_Code.CMSModules
{
    public class S3StorageProviderModule : Module
    {
        public S3StorageProviderModule()
            : base("S3StorageProvider") { }

        protected override void OnInit()
        {
            base.OnInit();

            var s3BucketName = SettingsKeyInfoProvider.GetValue(Settings.KDA_AmazonS3BucketName);
            if (!string.IsNullOrWhiteSpace(s3BucketName))
            {
                var environmentId = SettingsKeyInfoProvider.GetIntValue(Settings.KDA_EnvironmentId);
                try
                {
                    var environment = CustomTableItemProvider.GetItem<EnvironmentItem>(environmentId);
                    if (environment != null)
                    {
                        if (!string.IsNullOrWhiteSpace(environment.AmazonS3ExcludedPaths))
                        {
                            var excludedPaths = environment.AmazonS3ExcludedPaths.Split(';');
                            foreach (var path in excludedPaths)
                            {
                                StorageHelper.UseLocalFileSystemForPath(path);
                            }
                        }
                        var customAmazonProvider = new StorageProvider("CustomAmazon", "Kadena.AmazonFileSystemProvider", true)
                        {
                            CustomRootPath = s3BucketName
                        };
                        PathHelper.PathService = DIContainer.Resolve<IPathService>();
                        StorageHelper.MapStoragePath("~/", customAmazonProvider);
                        EventLogProvider.LogInformation(GetType().Name, "STORAGECONFIG", $"Data storage was mapped to Amazon S3 bucket '{s3BucketName}' with {customAmazonProvider.ExternalStorageName}.");
                    }
                }
                catch (Exception exc)
                {
                    EventLogProvider.LogException(GetType().Name, "EXCEPTION", exc);
                }
            }
        }
    }
}