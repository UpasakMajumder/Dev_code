using Kadena2.MicroserviceClients.Contracts;
using System.Threading.Tasks;
using Kadena.Dto.General;
using Kadena.Dto.Order;
using System.Net.Http;
using Kadena2.MicroserviceClients.Clients.Base;

namespace Kadena2.MicroserviceClients.Clients
{
    public class StatisticsClient : ClientBase, IStatisticsClient
    {
        public async Task<BaseResponseDto<OrderStatisticDto>> GetOrderStatistics(string endPoint, string customerName)
        {
            string url = $"{endPoint}?customerName={customerName}";
            using (var client = new HttpClient())
            {
                using (var message = await client.GetAsync(url).ConfigureAwait(false))
                {
                    return await ReadResponseJson<OrderStatisticDto>(message);
                }
            }
        }
    }
}
