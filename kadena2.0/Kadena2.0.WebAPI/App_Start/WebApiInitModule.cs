using CMS.Core;
using System;
using System.Web.Http;

[assembly: CMS.RegisterModule(typeof(Kadena.WebAPI.WebApiInitModule))]

namespace Kadena.WebAPI
{
    public class WebApiInitModule : CMS.DataEngine.Module
    {
        public WebApiInitModule() 
            : base("KadenaWebAPI")
        {
        }

        public WebApiInitModule(ModuleMetadata metadata, bool isInstallable = false) 
            : base(metadata, isInstallable)
        {
        }

        public WebApiInitModule(string moduleName, bool isInstallable = false)
            : base(moduleName, isInstallable)
        {
        }

        protected override void OnInit()
        {
            base.OnInit();

            // Workaround ok Kentico Ci - to supress exception during 'ContinuousIntegration.exe -r'
            try
            {
                WebApiConfig.Configure(GlobalConfiguration.Configuration);
            }
            catch(InvalidOperationException)
            {

            }
        }
    }
}