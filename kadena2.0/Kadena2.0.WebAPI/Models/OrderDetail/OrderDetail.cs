namespace Kadena.WebAPI.Models.OrderDetail
{
    public class OrderDetail
    {
        public CommonInfo CommonInfo { get; set; }
        public ShippingInfo ShippingInfo { get; set; }
        public PaymentInfo PaymentInfo { get; set; }
        public PricingInfo PricingInfo { get; set; }
        public OrderedItems OrderedItems { get; set; }
    }
}