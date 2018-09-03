using Kadena.Models.Site;
using Kadena.WebAPI.KenticoProviders.Contracts;
using System;

namespace Kadena.ScheduledTasks
{
    class SpecificResourceService : IKenticoResourceService
    {
        private readonly IKenticoResourceService kenticoResourceService;
        private readonly KenticoSite site;

        public SpecificResourceService(IKenticoResourceService kenticoResourceService, KenticoSite site)
        {
            this.kenticoResourceService = kenticoResourceService ?? throw new ArgumentNullException(nameof(kenticoResourceService));
            this.site = site ?? throw new ArgumentNullException(nameof(site));
        }

        public string GetLogonPageUrl()
        {
            throw new NotImplementedException();
        }

        public string GetMassUnit()
        {
            throw new NotImplementedException();
        }

        public string GetPerSiteResourceString(string name, string site = "")
        {
            return kenticoResourceService.GetPerSiteResourceString(name, site);
        }

        public string GetResourceString(string name, string cultureCode)
        {
            return kenticoResourceService.GetResourceString(name, cultureCode);
        }

        public string GetResourceString(string name)
        {
            return kenticoResourceService.GetResourceString(name);
        }

        public T GetSettingsKey<T>(string key, int siteId = 0) where T : IConvertible
        {
            return kenticoResourceService.GetSettingsKey<T>(key, siteId);
        }

        public string GetSiteSettingsKey(string key)
        {
            return GetSettingsKey<string>(key, site.Id);
        }

        public T GetSiteSettingsKey<T>(string key) where T : IConvertible
        {
            return GetSettingsKey<T>(key, site.Id);
        }

        public string ResolveMacroString(string macroString)
        {
            return kenticoResourceService.ResolveMacroString(macroString);
        }
    }
}
