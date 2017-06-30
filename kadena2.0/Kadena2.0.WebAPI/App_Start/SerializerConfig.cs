using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Kadena.WebAPI
{
    public static class SerializerConfig
    {
        public static JsonSerializerSettings CamelCaseSerializer { get; private set; } = new JsonSerializerSettings()
        {
            Formatting = Formatting.Indented,
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            DateFormatString = "MMM dd yyyy"
        };
    }
}