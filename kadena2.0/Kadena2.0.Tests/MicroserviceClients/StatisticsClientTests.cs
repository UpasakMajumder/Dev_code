using Kadena2.MicroserviceClients.Clients;
using Xunit;
using System.Threading.Tasks;

namespace Kadena.Tests.MicroserviceClients
{
    public class StatisticsClientTests : KadenaUnitTest<StatisticsClient>
    {
        [Fact(DisplayName = "StatisticsClient.GetOrderStatistics()")]
        public async Task GetOrderStatistics()
        {
            var actualResult = await Sut.GetOrderStatistics();

            Assert.NotNull(actualResult);
        }
    }
}
