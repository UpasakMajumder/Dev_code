using Kadena.Models.SiteSettings;
using Newtonsoft.Json;
using System;
using System.Text;

namespace Kadena.BusinessLogic.Services.SettingsSynchronization
{
    public class MetadataEncoder
    {
        public static string Encode(SettingsKey key)
        {
            var serialized = JsonConvert.SerializeObject(key);
            var encoded = Convert.ToBase64String(Encoding.UTF8.GetBytes(serialized));
            return encoded;
        }

        public static SettingsKey Decode(string value)
        {
            var decoded = Encoding.UTF8.GetString(Convert.FromBase64String(value));
            var deserialized = JsonConvert.DeserializeObject<SettingsKey>(decoded);
            return deserialized;
        }
    }
}