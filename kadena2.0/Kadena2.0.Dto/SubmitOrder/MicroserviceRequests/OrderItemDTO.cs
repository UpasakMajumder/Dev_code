using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Kadena.Dto.SubmitOrder.MicroserviceRequests
{
    public class OrderItemDTO
  {
        public SKUDTO SKU { get; set; }

        public int LineNumber { get; set; }

        public OrderItemTypeDTO Type { get; set; }

        public decimal UnitPrice { get; set; }

        [Range(1, int.MaxValue, ErrorMessage ="Number of units in order's items must be greater than 0")]
        public int UnitCount { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Unit Measure field in order's items is a mandatory field")]
        public string UnitOfMeasure { get; set; }

        public decimal TotalPrice { get; set; }

        public decimal TotalTax { get; set; }

        public MailingListDTO MailingList { get; set; }
        public string DesignFileKey { get; set; }
        public Dictionary<string, string> Attributes { get; set; }
        public ChiliProcessDto ChiliProcess { get; set; }
        public bool SendPriceToErp { get; set; }
        public bool RequiresApproval { get; set; }
    }
}
