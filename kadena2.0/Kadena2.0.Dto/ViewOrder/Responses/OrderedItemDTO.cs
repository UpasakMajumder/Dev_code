using Kadena.Dto.Checkout;
using Kadena.Dto.Common;
using System.Collections.Generic;

namespace Kadena.Dto.ViewOrder.Responses
{
    public class OrderedItemDTO
    {
        public int Id { get; set; }
        public string Image { get; set; }
        public string TemplatePrefix { get; set; }
        public string Template { get; set; }
        public string MailingListPrefix { get; set; }
        public string MailingList { get; set; }
        public string ShippingDatePrefix { get; set; }
        public string ShippingDate { get; set; }
        public string TrackingIdPrefix { get; set; }
        public string TrackingId { get; set; }
        public string Price { get; set; }
        public string QuantityPrefix { get; set; }
        public string QuantityShippedPrefix { get; set; }
        public int Quantity { get; set; }
        public int QuantityShipped { get; set; }
        public string DownloadPdfURL { get; set; }
        public string ProductStatusPrefix { get; set; }
        public string ProductStatus { get; set; }
        public ButtonDto Preview { get; set; }
        public ButtonDto EmailProof { get; set; }
        public IEnumerable<ItemOptionDto> Options { get; set; }
    }
}