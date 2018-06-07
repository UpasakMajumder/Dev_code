namespace Kadena.Dto.ViewOrder.Responses
{
    public class ShippingInfoDTO
    {
        public string Title { get; set; }
        public string Message { get; set; }
        public string DeliveryMethod { get; set; }
        public AddressDto Address { get; set; }
    }
}