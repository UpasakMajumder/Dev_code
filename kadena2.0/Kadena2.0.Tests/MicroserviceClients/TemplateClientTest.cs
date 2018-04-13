using Kadena.MicroserviceClients.Clients;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Kadena.Tests.MicroserviceClients
{
    public class TemplateClientTest : KadenaUnitTest<TemplatedClient>
    {
        [Fact(DisplayName = "TemplateClient.GetPreview()")]
        public async Task GetPreview()
        {
            var actualResult = await Sut.GetPreview(Guid.Empty, Guid.Empty);

            Assert.NotNull(actualResult);
        }
    }
}
