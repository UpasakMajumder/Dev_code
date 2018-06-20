using Kadena.Models.OrderDetail;

namespace Kadena.Models.Orders
{
    public class OrderUpdateResult
    {
        public TitleValuePair<string>[] Pricinginfo { get; set; }
        public ItemUpdateResult[] OrdersPrice { get; set; }
    }
}
