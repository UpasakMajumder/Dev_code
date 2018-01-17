using System.Collections.Generic;
using Kadena.Models.SiteSettings;
using Kadena.Models.SiteSettings.Synchronization;

namespace Kadena.BusinessLogic.Contracts
{
    public interface ISettingsSynchronizationService
    {
        void AddNewKeys(IEnumerable<SettingsKey> keysToAdd);
        string GetSettingsKeySourceCode(string keyName);
        IList<SettingsKey> GetSettingsToAdd();
        SynchronizationResult Synchronize();
    }
}