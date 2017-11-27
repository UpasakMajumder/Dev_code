using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Kadena.Helpers
{
    public static class SerializerConfig
    {
        public static JsonSerializerSettings CamelCaseSerializer { get; private set; } = new JsonSerializerSettings()
        {
            Formatting = Formatting.Indented,
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };
    }
}