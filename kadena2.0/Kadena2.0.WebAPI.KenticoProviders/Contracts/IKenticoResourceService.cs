using System;

namespace Kadena.WebAPI.KenticoProviders.Contracts
{
    public interface IKenticoResourceService
    {
        string GetResourceString(string name, string cultureCode);
        string GetResourceString(string name);
        string GetPerSiteResourceString(string name, string site = "");
        string GetSiteSettingsKey(string key);
        T GetSiteSettingsKey<T>(string key) where T : IConvertible;

        /// <summary>
        /// Gets Settings key
        /// </summary>
        /// <param name="key">Name of key</param>
        /// <param name="siteId">Id of site. 0 means global key</param>
        T GetSettingsKey<T>(string key, int siteId = 0) where T : IConvertible;
        string ResolveMacroString(string macroString);
        string GetLogonPageUrl();
        string GetMassUnit();
    }
}
