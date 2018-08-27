namespace Kadena.Dto.OrderManualUpdate.MicroserviceRequests.UpdateShipping
{
    public class UpdateShippingsOrderItemShippingDto
    {
        public int QuantityShipped { get; set; }
        public string TrackingNumber { get; set; }
        public string ShippingDate { get; set; }
        public UpdateShippingsOrderItemShippingMethod ShippingMethod { get; set; }
    }
}
