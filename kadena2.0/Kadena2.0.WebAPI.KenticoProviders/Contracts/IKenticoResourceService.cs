using System;

namespace Kadena.WebAPI.KenticoProviders.Contracts
{
    public interface IKenticoResourceService
    {
        string GetResourceString(string name);
        string GetPerSiteResourceString(string name, string site = "");
        string GetSettingsKey(string key);
        string GetSettingsKey(string siteName, string key);        
        string GetSettingsKey(int siteId, string key);        
        string ResolveMacroString(string macroString);
        T GetSettingsKey<T>(string key, int siteId = 0) where T : IConvertible;
        string GetLogonPageUrl();
    }
}
