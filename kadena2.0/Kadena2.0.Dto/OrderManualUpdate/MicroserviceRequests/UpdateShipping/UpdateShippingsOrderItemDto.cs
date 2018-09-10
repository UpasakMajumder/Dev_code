namespace Kadena.Dto.OrderManualUpdate.MicroserviceRequests.UpdateShipping
{
    public class UpdateShippingsOrderItemDto
    {
        public int LineNumber { get; set; }
        public UpdateShippingsOrderItemShippingDto[] Shippings { get; set; }
    }
}
