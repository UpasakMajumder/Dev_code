using Kadena.Models.OrderHistory;
using System.Threading.Tasks;

namespace Kadena.BusinessLogic.Contracts
{
    public interface IOrderHistoryService
    {
        Task<OrderHistory> GetOrderHistory(string orderId);
    }
}
