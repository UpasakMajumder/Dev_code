using Kadena.Dto.Order;
using Kadena.Models.Orders;

namespace Kadena.BusinessLogic.Factories
{
    public interface IOrderReportFactory
    {
        OrderReport Create(RecentOrderDto orderDto);
    }

    public class OrderReportFactory
    {
        public OrderReport Create(RecentOrderDto orderDto)
        {
            return null;
        }
    }
}
