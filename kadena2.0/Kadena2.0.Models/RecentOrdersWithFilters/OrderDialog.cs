namespace Kadena.Models.RecentOrders
{
    public class OrderDialog
    {
        public OrderDailogLabel orderId { get; set; }
        public OrderDailogLabel distributor { get; set; }
        public OrderDailogLabel pdf { get; set; }
        public OrderDialogTable table { get; set; }
    }
}
