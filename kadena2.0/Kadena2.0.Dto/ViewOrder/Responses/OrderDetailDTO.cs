namespace Kadena.Dto.ViewOrder.Responses
{
    public class OrderDetailDTO
    {
        public string DateTimeNAString { get; set; }
        public CommonInfoDTO CommonInfo { get; set; }
        public ShippingInfoDTO ShippingInfo { get; set; }
        public PaymentInfoDTO PaymentInfo { get; set; }
        public PricingInfoDTO PricingInfo { get; set; }
        public OrderedItemsDTO OrderedItems { get; set; }
    }
}