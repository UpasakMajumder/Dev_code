using Kadena.Dto.SubmitOrder.MicroserviceRequests;
using System;
using System.Collections.Generic;

namespace Kadena.Dto.Order
{
    public class RecentOrderDto
    {
        public string Id { get; set; }

        public DateTime CreateDate { get; set; }

        public string Status { get; set; }

        public DateTime? ShippingDate { get; set; }

        public decimal TotalCost { get; set; }

        public int ClientId { get; set; }

        public string SiteName { get; set; }

        public IEnumerable<OrderItemDto> Items { get; set; }

        public Dictionary<int, int> StatusAmounts { get; set; }

        public CampaignDTO Campaign { get; set; }
    }
}