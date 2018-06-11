using Kadena.Helpers;
using Newtonsoft.Json;
using Xunit;

namespace Kadena.Tests.Infrastructure
{
    public class JsonSerializer
    {
        [Theory]
        [InlineData("L'Oreal", "L'Oreal")]
        [InlineData("L\"Oreal", "L\\\"Oreal")]
        public void SerializeTest(string value, string escaped)
        {
            var json = new
            {
                text = value
            };

            var result = JsonConvert.SerializeObject(json, SerializerConfig.CamelCaseSerializer);

            Assert.Contains(escaped, result);
        }
    }
}
