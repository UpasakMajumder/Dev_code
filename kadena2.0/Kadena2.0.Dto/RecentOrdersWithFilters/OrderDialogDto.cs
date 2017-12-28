namespace Kadena.Dto.RecentOrders
{
    public class OrderDialogDto
    {
        public OrderDailogLabelDto orderId { get; set; }
        public OrderDailogLabelDto distributor { get; set; }
        public OrderDailogLabelDto pdf { get; set; }
        public OrderDialogTableDto table { get; set; }
    }
}
