using System.Collections.Generic;
using Kadena.Models.SiteSettings;

namespace Kadena.BusinessLogic.Contracts
{
    public interface ISettingsSynchronizationService
    {
        void AddNewKeys(IEnumerable<SettingsKey> keysToAdd);
        string GetSettingsKeyCode(string keyName);
        IEnumerable<SettingsKey> GetSettingsToAdd();
        void Synchronize();
    }
}