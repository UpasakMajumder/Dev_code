using Kadena.WebAPI.Contracts;
using CMS.Helpers;
using CMS.DataEngine;
using CMS.SiteProvider;

namespace Kadena.WebAPI.Services
{
    public class KenticoResourceService : IResourceService
    {
        public string GetResourceString(string name)
        {
            return ResHelper.GetString(name, useDefaultCulture:true);
        }

        public string GetSettingsKey(string key)
        {
            string resourceKey = $"{SiteContext.CurrentSiteName}.{key}";
            return SettingsKeyInfoProvider.GetValue(resourceKey);
        }
    }
}