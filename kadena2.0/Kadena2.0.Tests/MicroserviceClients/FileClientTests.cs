using Xunit;
using Kadena2.MicroserviceClients.Clients;
using System.Threading.Tasks;

namespace Kadena.Tests.MicroserviceClients
{
    public class FileClientTests : KadenaUnitTest<FileClient>
    {
        [Fact(DisplayName = "FileClient.GetFileKey()")]
        public async Task GetFileKey()
        {
            var actualResult = await Sut.GetFileKey(string.Empty, string.Empty, string.Empty);

            Assert.NotNull(actualResult);
        }
    }
}
