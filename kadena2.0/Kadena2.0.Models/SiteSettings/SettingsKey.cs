namespace Kadena.Models.SiteSettings
{
    public class SettingsKey
    {
        // required
        public string KeyDisplayName { get; set; }
        // required
        public string KeyName { get; set; }
        // required
        public string KeyType { get; set; }

        public string KeyDefaultValue { get; set; }
        public string KeyDescription { get; set; }
        public string KeyEditingControlPath { get; set; }
        public string KeyExplanationText { get; set; }
        public string KeyFormControlSettings { get; set; }
        public string KeyValidation { get; set; }

        public Group Group { get; set; }
    }
}