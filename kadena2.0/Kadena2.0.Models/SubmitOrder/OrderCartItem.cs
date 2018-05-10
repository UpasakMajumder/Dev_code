using Kadena.Models.Checkout;
using System;
using System.Collections.Generic;

namespace Kadena.Models.SubmitOrder
{
    public class OrderCartItem
    {
        public int Id { get; set; }
        public string ProductType { get; set; }
        public int SKUID { get; set; }
        public string SKUNumber { get; set; }
        public string SKUName { get; set; }
        public int LineNumber { get; set; }
        public Guid MailingListGuid { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
        public int Quantity { get; set; }
        public Guid ChiliTemplateId { get; set; }
        public Guid ProductChiliPdfGeneratorSettingsId { get; set; }
        public string UnitOfMeasureErpCode { get; set; }
        public ChiliProcess ChiliProcess { get; set; }
        public bool SendPriceToErp { get; set; }
        public bool RequiresApproval { get; set; }
        public bool HiResPdfAllowed { get; set; }
        public string Artwork { get; set; } 
        public IEnumerable<ItemOption> Options { get; set; } 
    }
}
