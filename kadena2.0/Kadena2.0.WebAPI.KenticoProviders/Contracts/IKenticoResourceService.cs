namespace Kadena.WebAPI.KenticoProviders.Contracts
{
    public interface IKenticoResourceService
    {
        string GetResourceString(string name);
        string GetSettingsKey(string key);
        string GetSettingsKey(string siteName, string key);        
        string GetSettingsKey(int siteId, string key);        
        string ResolveMacroString(string macroString);
        string GetStorageRootPath(string path);
    }
}
