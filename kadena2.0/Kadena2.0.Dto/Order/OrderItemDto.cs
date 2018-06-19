using Kadena.Dto.ViewOrder.MicroserviceResponses;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Kadena.Dto.Order
{
    [DataContract]
    public class OrderItemDto
    {
        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "quantity")]
        public int Quantity { get; set; }

        [DataMember(Name = "skuNumber")]
        public string SKUNumber { get; set; }

        [DataMember(Name = "unitPrice")]
        public decimal UnitPrice { get; set; }

        [DataMember(Name = "quantityShipped")]
        public int QuantityShipped { get; set; }

        [DataMember(Name = "shippingDate")]
        public DateTime? ShippingDate { get; set; }

        public IEnumerable<TrackingInfoDto> TrackingInfos { get; set; }
    }
}