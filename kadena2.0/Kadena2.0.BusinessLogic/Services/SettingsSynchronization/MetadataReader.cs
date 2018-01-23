using Kadena.Models.SiteSettings;
using Kadena.Models.SiteSettings.Attributes;
using Kadena.Models.SiteSettings.Synchronization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Kadena.BusinessLogic.Services.SettingsSynchronization
{
    public class MetadataReader
    {
        private Type settingsType = typeof(Settings);

        public IEnumerable<string> GetNames()
        {
            var definedKeys = settingsType.GetFields(BindingFlags.Public | BindingFlags.Static);
            foreach (var item in definedKeys)
            {
                yield return item.Name;
            }
        }

        public SettingsKey GetSettingsKeyMetadata(string name)
        {
            var field = settingsType.GetField(name);
            if (field == null)
            {
                throw new ArgumentException($"Settings key '{name}' is not defined");
            }

            var attributes = field.GetCustomAttributes();

            // get key definition
            var keyDefinitionAttribute = attributes.FirstOrDefault(ca => ca is EncodedDefinitionAttribute) as EncodedDefinitionAttribute;
            if (keyDefinitionAttribute == null)
            {
                throw new SettingsSynchronizationException($"Definition attribute is missing for key { field.Name }");
            }
            var key = MetadataEncoder.Decode(keyDefinitionAttribute.Definition);

            // apply definition overrides
            var defaultValueAttribute = attributes.FirstOrDefault(ca => ca is DefaultValueAttribute) as DefaultValueAttribute;
            if (defaultValueAttribute != null)
            {
                key.KeyDefaultValue = defaultValueAttribute.DefaultValue;
            }

            // get key placement
            var categoryAttribute = attributes.FirstOrDefault(ca => ca is CategoryAttribute) as CategoryAttribute;
            if (categoryAttribute == null)
            {
                throw new SettingsSynchronizationException($"Category attribute is missing for key { field.Name }");
            }
            var category = new Category(categoryAttribute.DisplayName, categoryAttribute.Name, categoryAttribute.ParentName);

            var groupAttribute = attributes.FirstOrDefault(ca => ca is GroupAttribute) as GroupAttribute;
            if (groupAttribute == null)
            {
                throw new SettingsSynchronizationException($"Group attribute is missing for key { field.Name }");
            }
            var group = new Group(groupAttribute.DisplayName, category);

            key.Group = group;

            return key;
        }
    }
}