using CMS;
using CMS.CustomTables;
using CMS.DataEngine;
using CMS.IO;
using CMS.MacroEngine;
using Kadena.Old_App_Code.CMSModules.Macros.Kadena;
using Kadena.CustomTables;
using Kadena.AmazonFileSystemProvider;
using System;
using CMS.EventLog;

[assembly: RegisterModule(typeof(Kadena.Old_App_Code.CMSModules.Kadena.KadenaModule))]
namespace Kadena.Old_App_Code.CMSModules.Kadena
{
    public class KadenaModule : Module
    {
        private const string SelectedEnvironment = "KDA_EnvironmentId";

        public KadenaModule()
            : base("Kadena") { }

        protected override void OnInit()
        {
            base.OnInit();

            var s3BucketName = SettingsKeyInfoProvider.GetValue(SettingsKeyNames.AmazonS3BucketName);
            if (!string.IsNullOrWhiteSpace(s3BucketName))
            {
                var environmentId = SettingsKeyInfoProvider.GetIntValue(SelectedEnvironment);
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
                            CustomRootPath = s3BucketName,
                            CustomRootUrl = Path.EnsureSlashes(Path.Combine(environment.AmazonS3Folder ?? string.Empty, "media"))
                        };
                        StorageHelper.MapStoragePath("~/", customAmazonProvider);
                    }
                }
                catch (Exception exc)
                {
                    EventLogProvider.LogException(GetType().Name, "EXCEPTION", exc);
                }
            }
            MacroContext.GlobalResolver.SetNamedSourceData("KadenaNamespace", KadenaMacroNamespace.Instance);
        }
    }
}