using CMS;
using CMS.CustomTables;
using CMS.DataEngine;
using CMS.IO;
using CMS.MacroEngine;
using Kadena.Old_App_Code.CMSModules.Macros.Kadena;
using Kadena.CustomTables;
using Kadena.AmazonFileSystemProvider;

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
                var environment = CustomTableItemProvider.GetItem<EnvironmentItem>(environmentId);
                var customAmazonProvider = new StorageProvider("CustomAmazon", "Kadena.AmazonFileSystemProvider", true)
                {
                    CustomRootPath = s3BucketName,
                    CustomRootUrl = environment.AmazonS3Folder
                };
                StorageHelper.MapStoragePath("~/", customAmazonProvider);
            }
            MacroContext.GlobalResolver.SetNamedSourceData("KadenaNamespace", KadenaMacroNamespace.Instance);
        }
    }
}