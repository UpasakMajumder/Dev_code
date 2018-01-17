using Kadena.Models.SiteSettings;
using Kadena.Models.SiteSettings.Attributes;
using System;

namespace Kadena.BusinessLogic.Services.SettingsSynchronization
{
    public class SettingsKeyTemplater
    {
        public static string GetTemplateSourceCode(SettingsKey settingsKey)
        {
            var key = settingsKey ?? throw new ArgumentNullException(nameof(settingsKey));

            var codeLines = new[]
            {
                "/// <summary>",
                $"/// { key.KeyDescription }",
                "/// </summary>",
                $"[{ nameof(CategoryAttribute) }(\"{ key.Group.Category.DisplayName }\", \"{ key.Group.Category.Name }\", \"{ key.Group.Category.CategoryParentName }\")]",
                $"[{ nameof(GroupAttribute) }(\"{ key.Group.DisplayName }\")]",
                $"[{ nameof(DefaultValueAttribute) }({ (key.KeyDefaultValue == null ? "null" : $"@\"{ EscapeQuotes(key.KeyDefaultValue) }\"" ) })]",
                $"[{ nameof(EncodedDefinitionAttribute) }(\"{ MetadataEncoder.Encode(key) }\")]",
                $"public const string { key.KeyName } = \"{ key.KeyName }\";"
            };
            var code = string.Join(Environment.NewLine, codeLines);
            return code;
        }

        private static string EscapeQuotes(string input)
        {
            return input.Replace("\"", "\"\"");
        }
    }
}