using System.Collections.Generic;

namespace Kadena.Dto.OrderManualUpdate.MicroserviceRequests.UpdateShipping
{
    public class UpdateShippingsOrderDto
    {
        public string OrderId { get; set; }
        public IList<UpdateShippingsOrderItemDto> Items { get; set; }
    }
}
