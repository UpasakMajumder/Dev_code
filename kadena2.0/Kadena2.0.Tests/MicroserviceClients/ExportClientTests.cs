using Kadena2.MicroserviceClients.Clients;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Kadena.Tests.MicroserviceClients
{
    public class ExportClientTests : KadenaUnitTest<ExportClient>
    {
        [Fact(DisplayName = "ExportClient()")]
        public void ExportClient()
        {
            Assert.Throws<ArgumentNullException>(()=>new ExportClient(null));
        }

        [Fact(DisplayName = "ExportClient.ExportMailingList()")]
        public async Task ExportMailingList()
        {
            var actualResult = await Sut.ExportMailingList(Guid.Empty, string.Empty);

            Assert.NotNull(actualResult);
        }
    }
}
