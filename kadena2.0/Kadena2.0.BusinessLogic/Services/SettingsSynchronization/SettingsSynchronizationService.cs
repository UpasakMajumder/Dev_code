using Kadena.BusinessLogic.Contracts;
using Kadena.Models.SiteSettings;
using Kadena2.WebAPI.KenticoProviders.Providers;
using System;
using System.Collections.Generic;

namespace Kadena.BusinessLogic.Services.SettingsSynchronization
{
    public class SettingsSynchronizationService : ISettingsSynchronizationService
    {
        private readonly IKenticoSettingsProvider settingsProvider;

        public SettingsSynchronizationService(IKenticoSettingsProvider settingsProvider)
        {
            this.settingsProvider = settingsProvider;
        }

        private bool TryFindSettingsKey(out SettingsKey settingsKey, string name)
        {
            try
            {
                settingsKey = settingsProvider.GetKeyByName(name);
            }
            catch (ArgumentException)
            {
                settingsKey = null;
            }
            return settingsKey != null;
        }

        public string GetSettingsKeyCode(string keyName)
        {
            var isValidKey = TryFindSettingsKey(out var key, keyName);
            if (!isValidKey)
            {
                return string.Empty;
            }

            var templater = new SettingsKeyTemplater(key);
            var code = templater.GetTemplateCode();
            return code;
        }

        public void Synchronize()
        {
            var keysToAdd = GetSettingsToAdd();
            AddNewKeys(keysToAdd);
        }

        public void AddNewKeys(IEnumerable<SettingsKey> keysToAdd)
        {
            foreach (var key in keysToAdd)
            {
                var categoryExists = settingsProvider.CategoryExists(key.Group.Category.Name);
                if (!categoryExists)
                {
                    settingsProvider.CreateCategory(key.Group.Category);
                }

                var groupExists = settingsProvider.GroupExists(key.Group.DisplayName, key.Group.Category.Name);
                if (!groupExists)
                {
                    settingsProvider.CreateGroup(key.Group);
                }

                settingsProvider.CreateKey(key);
            }
        }

        public IEnumerable<SettingsKey> GetSettingsToAdd()
        {
            var keysToCreate = new List<SettingsKey>();
            var metadataReader = new MetadataReader();
            foreach (var keyName in metadataReader.GetNames())
            {
                var keyExists = settingsProvider.KeyExists(keyName);
                if (keyExists)
                {
                    continue;
                }

                var newKey = metadataReader.GetSettingsKeyMetadata(keyName);
                keysToCreate.Add(newKey);
            }
            return keysToCreate;
        }
    }
}