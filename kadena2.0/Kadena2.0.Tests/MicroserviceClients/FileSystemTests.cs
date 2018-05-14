using Xunit;
using Kadena2.MicroserviceClients;

namespace Kadena.Tests.MicroserviceClients
{
    public class FileSystemTests
    {
        [Fact(DisplayName = "FileSystem.Create() | Mailing")]
        public void CreateMailing()
        {
            var expectedResult = FileSystem.Mailing;

            var actualResult = FileSystem.Create("klist/");

            Assert.Equal(expectedResult, actualResult);
        }

        [Theory(DisplayName = "FileSystem.Create() | Not registered")]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("noregisteredpath")]
        public void CreateNotRegisteredSystem(string path)
        {
            var actualResult = FileSystem.Create(path);

            Assert.Null(actualResult);
        }

    }
}
