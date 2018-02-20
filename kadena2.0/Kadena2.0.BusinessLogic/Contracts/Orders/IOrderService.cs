using Kadena.Models.Common;
using Kadena.Models.Orders;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Kadena.BusinessLogic.Contracts.Orders
{
    public interface IOrderService
    {
        Task<List<Order>> GetOrders(OrderFilter filter, int page);
        Task<List<Order>> GetOrders(string site, int page, OrderFilter filter);

        TableView ConvertOrdersToView(IEnumerable<Order> orders, int currentPage, int totalOrders);

        Task<FileResult> GetOrdersExport(string format, OrderFilter filter);
        Task<FileResult> GetOrdersExport(string site, string format, OrderFilter filter);
    }
}
