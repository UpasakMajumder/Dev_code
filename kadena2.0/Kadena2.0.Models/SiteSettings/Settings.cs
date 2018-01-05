using Kadena.Models.SiteSettings.Attributes;

namespace Kadena.Models.SiteSettings
{
    public class Settings
    {
        /// <summary>
        /// Please describe me
        /// </summary>
        [CategoryAttribute("Everything else", "Everything_Else", "Kadena")]
        [GroupAttribute("General")]
        [DefaultValueAttribute(null)]
        [EncodedDefinitionAttribute("eyJLZXlEZWZhdWx0VmFsdWUiOm51bGwsIktleURlc2NyaXB0aW9uIjoiIiwiS2V5RGlzcGxheU5hbWUiOiJEaXNhYmxlZCBNb2R1bGUgVXJsIChLREFfRGlzYWJsZWRNb2R1bGVVcmwpIiwiS2V5RWRpdGluZ0NvbnRyb2xQYXRoIjpudWxsLCJLZXlFeHBsYW5hdGlvblRleHQiOiIiLCJLZXlGb3JtQ29udHJvbFNldHRpbmdzIjpudWxsLCJLZXlOYW1lIjoiS0RBX0Rpc2FibGVkTW9kdWxlVXJsIiwiS2V5VHlwZSI6InN0cmluZyIsIktleVZhbGlkYXRpb24iOm51bGx9")]
        public const string KDA_DisabledModuleUrl = "KDA_DisabledModuleUrl";
    }
}