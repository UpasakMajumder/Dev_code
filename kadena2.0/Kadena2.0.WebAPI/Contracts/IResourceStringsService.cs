using Kadena.WebAPI.Models;

namespace Kadena.WebAPI.Contracts
{
    public interface IResourceService
    {
        string GetResourceString(string name);

        string GetSettingsKey(string key);

        KenticoSite GetKenticoSite();
    }
}
