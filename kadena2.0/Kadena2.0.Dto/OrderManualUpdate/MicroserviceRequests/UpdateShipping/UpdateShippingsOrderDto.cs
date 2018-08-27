namespace Kadena.Dto.OrderManualUpdate.MicroserviceRequests.UpdateShipping
{
    public class UpdateShippingsOrderDto
    {
        public string OrderId { get; set; }
        public UpdateShippingsOrderItemDto[] ItemShippingUpdates { get; set; }
    }
}
