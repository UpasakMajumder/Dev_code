using Kadena.Dto.ViewOrder.MicroserviceResponses;
using System;
using System.Collections.Generic;

namespace Kadena.Dto.Order
{
    public class OrderItemDto
    {
        public string LineNumber { get; set; }

        public string Name { get; set; }

        public int Quantity { get; set; }

        public string SKUNumber { get; set; }

        public decimal UnitPrice { get; set; }

        public int QuantityShipped { get; set; }

        public DateTime? ShippingDate { get; set; }

        public IEnumerable<TrackingInfoDto> TrackingInfos { get; set; }
    }
}