using Kadena2.MicroserviceClients;
using Xunit;

namespace Kadena.Tests.MicroserviceClients
{
    public class FileModuleTests
    {
        [Theory]
        [InlineData("KList")]
        [InlineData("KProducts")]
        [InlineData("KDesign")]
        public void ParseFileModuleOK(string source)
        {
            // Act
            FileModule module = source;

            // Assert
            Assert.NotNull(module);
            Assert.Equal(module.ToString().ToLower(), source.ToLower());
        }

        [Theory]
        [InlineData("dsasdfadsf")]
        [InlineData("")]
        [InlineData(null)]
        public void ParseFileModuleFail(string source)
        {
            // Act
            FileModule module = source;

            // Assert
            Assert.Null(module);
        }
    }
}
