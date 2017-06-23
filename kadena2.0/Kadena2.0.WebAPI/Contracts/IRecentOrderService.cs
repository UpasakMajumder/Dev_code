using Kadena.WebAPI.Models.RecentOrders;
using System.Threading.Tasks;

namespace Kadena.WebAPI.Contracts
{
    public interface IRecentOrderService
    {
        string PageCapacityKey { get; set; }

        Task<OrderHead> GetHeaders();

        Task<OrderBody> GetBody(int pageNumber);
    }
}
