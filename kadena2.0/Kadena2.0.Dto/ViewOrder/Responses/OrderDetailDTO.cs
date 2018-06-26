using Kadena.Dto.Common;

namespace Kadena.Dto.ViewOrder.Responses
{
    public class OrderDetailDTO
    {
        public string DateTimeNAString { get; set; }
        public CommonInfoDTO CommonInfo { get; set; }
        public ShippingInfoDTO ShippingInfo { get; set; }
        public PaymentInfoDTO PaymentInfo { get; set; }
        public PricingInfoDTO PricingInfo { get; set; }
        public OrderInfoDTO General { get; set; }
        public OrderedItemsDTO OrderedItems { get; set; }
        public OrderActionsDTO Actions { get; set; }
        public DialogButtonDTO<EditOrderDialogDTO> EditOrders { get; set; }
    }
}