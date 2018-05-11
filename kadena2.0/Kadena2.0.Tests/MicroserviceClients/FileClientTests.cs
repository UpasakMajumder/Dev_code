using Xunit;
using Kadena2.MicroserviceClients.Clients;
using Kadena2.MicroserviceClients;
using System.Threading.Tasks;

namespace Kadena.Tests.MicroserviceClients
{
    public class FileClientTests : KadenaUnitTest<FileClient>
    {
        [Fact(DisplayName = "FileClient.GetFileKey()")]
        public async Task GetFileKey()
        {
            var actualResult = await Sut.GetFileKey(FileSystem.Mailing, FileType.Original, string.Empty, string.Empty, string.Empty);

            Assert.NotNull(actualResult);
        }
    }
}
