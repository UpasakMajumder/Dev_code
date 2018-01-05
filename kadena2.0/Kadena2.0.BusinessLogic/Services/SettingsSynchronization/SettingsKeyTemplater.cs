using Kadena.Models.SiteSettings;
using Kadena.Models.SiteSettings.Attributes;
using System;

namespace Kadena.BusinessLogic.Services.SettingsSynchronization
{
    public class SettingsKeyTemplater
    {
        SettingsKey key;

        public SettingsKeyTemplater(SettingsKey key)
        {
            this.key = key ?? throw new ArgumentNullException(nameof(key));
        }

        public string GetTemplateCode()
        {
            var codeLines = new[]
            {
                "/// <summary>",
                $"/// { key.KeyDescription }",
                "/// </summary>",
                $"[{ nameof(CategoryAttribute) }(\"{ key.Group.Category.DisplayName }\", \"{ key.Group.Category.Name }\", \"{ key.Group.Category.CategoryParentName }\")]",
                $"[{ nameof(GroupAttribute) }(\"{ key.Group.DisplayName }\")]",
                $"[{ nameof(DefaultValueAttribute) }({ (key.KeyDefaultValue == null ? "null" : $"\"{ key.KeyDefaultValue }\"" ) })]",
                $"[{ nameof(EncodedDefinitionAttribute) }(\"{ MetadataEncoder.Encode(key) }\")]",
                $"public const string { key.KeyName } = \"{ key.KeyName }\";"
            };
            var code = string.Join(Environment.NewLine, codeLines);
            return code;
        }
    }
}