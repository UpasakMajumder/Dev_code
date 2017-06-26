﻿namespace Kadena.Dto.ViewOrder.Responses
{
    public class OrderedItemDTO
    {
        public int Id { get; set; }
        public string Image { get; set; }
        public string Template { get; set; }
        public string MailingList { get; set; }
        public string ShippingDate { get; set; }
        public string TrackingId { get; set; }
        public string Price { get; set; }
        public string QuantityPrefix { get; set; }
        public int Quantity { get; set; }
        public string DownloadPdfURL { get; set; }
    }
}