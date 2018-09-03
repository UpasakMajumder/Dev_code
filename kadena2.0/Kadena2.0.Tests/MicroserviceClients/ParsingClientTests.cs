using Kadena2.MicroserviceClients.Clients;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Kadena.Tests.MicroserviceClients
{
    public class ParsingClientTests : KadenaUnitTest<ParsingClient>
    {
        [Fact(DisplayName = "ParsingClient()")]
        public void ParsingClient()
        {
            Assert.Throws<ArgumentNullException>(() => new ParsingClient(null));
        }

        [Fact(DisplayName = "ParsingClient.GetHeaders()")]
        public async Task GetHeaders()
        {
            var actualResult = await Sut.GetHeaders(string.Empty);

            Assert.NotNull(actualResult);
        }
    }
}
