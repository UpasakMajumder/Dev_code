using System;
using System.Collections.Generic;

namespace Kadena.Dto.ViewOrder.MicroserviceResponses
{
    public class OrderItemDTO
    {
        public int SkuId { get; set; }
        public string Type { get; set; }
        public int Quantity { get; set; }
        public int QuantityShipped { get; set; }
        public string UnitOfMeasure { get; set; }
        public string TrackingId { get; set; }
        public string Name { get; set; }
        public string MailingList { get; set; }
        public string FileKey { get; set; }
        public double TotalPrice { get; set; }
        public int LineNumber { get; set; }
        public Guid TemplateId { get; set; }
        public Dictionary<string, string> Attributes { get; set; }
    }
}
