using Kadena.WebAPI.Models.RecentOrders;
using System.Threading.Tasks;

namespace Kadena.WebAPI.Contracts
{
    public interface IRecentOrderService
    {
        Task<OrderHead> GetHeaders(int pageNumber);
    }
}
