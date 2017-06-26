using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kadena.WebAPI.Models
{
    public class OrderItem
    {

        public int KenticoSKUId { get; set; }
        public string SKUNumber { get; set; }
        public string SKUName { get; set; }
        
        public int LineNumber { get; set; }

        public string OrderItemType { get; set; }

        public double UnitPrice { get; set; }
        
        public int UnitCount { get; set; }
        
        public string UnitOfMeasure { get; set; }
        
        public double TotalPrice { get; set; }

        public double TotalTax { get; set; }

        /// <summary>
        /// GUID
        /// </summary>
        public Guid MailingListId { get; set; }

        public string DesignFilePath { get; set; }
    }
}