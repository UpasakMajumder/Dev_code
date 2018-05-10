using Kadena.Models.Checkout;
using System;
using System.Collections.Generic;

namespace Kadena.Models.SubmitOrder
{
    public class OrderCartItem
    {
        public OrderItemSku SKU { get; set; }
        public int LineNumber { get; set; }
        public string ProductType { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public string UnitOfMeasureErpCode { get; set; }
        public decimal TotalPrice { get; set; }                
        public Guid MailingListGuid { get; set; }
        public string Artwork { get; set; }                
        public ChiliProcess ChiliProcess { get; set; }
        public bool SendPriceToErp { get; set; }
        public bool RequiresApproval { get; set; }                
        public IEnumerable<ItemOption> Options { get; set; } 
    }
}
