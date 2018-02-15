using Kadena.BusinessLogic.Contracts.Orders;
using Kadena.Models.Common;
using System;
using System.Threading.Tasks;

namespace Kadena.BusinessLogic.Services.Orders
{
    public class OrderService : IOrderService
    {
        public Task<FileResult> ExportOrders(DateTime? fromDate, DateTime? toDate, string format)
        {
            return Task.FromResult(new FileResult
            {
                Data = System.Text.UTF8Encoding.UTF8.GetBytes("test file"),
                Mime = "text/plain"
            });
        }

        public Task<TableView> GetOrdersView(DateTime? fromDate, DateTime? toDate, string sort, int page)
        {
            return Task.FromResult(new TableView());
        }
    }
}
