using Kadena.Dto.General;
using Kadena.Dto.Order;
using System.Threading.Tasks;

namespace Kadena2.MicroserviceClients.Contracts
{
    public interface IStatisticsClient
    {
        /// <summary>
        /// Returns order statistics for current customer (website).
        /// </summary>
        /// <returns></returns>
        Task<BaseResponseDto<OrderStatisticDto>> GetOrderStatistics();
    }
}
