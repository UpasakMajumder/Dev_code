using Kadena.Models.Common;
using Kadena.Models.Orders;
using System.Threading.Tasks;

namespace Kadena.BusinessLogic.Contracts.Orders
{
    public interface IOrderService
    {
        Task<PagedData<Order>> GetOrders(OrderFilter filter, int page);
        Task<PagedData<Order>> GetOrdersForSite(string site, int page, OrderFilter filter);

        TableView ConvertOrdersToView(PagedData<Order> orders);

        Task<FileResult> GetOrdersExport(string format, OrderFilter filter);
        Task<FileResult> GetOrdersExportForSite(string site, string format, OrderFilter filter);
    }
}
