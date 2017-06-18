using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kadena.WebAPI.Models
{
    public class OrderItem
    {
        public int Id { get; set; }
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
        
        public Guid MailingListId { get; set; }

        /// <summary>
        /// Main Chilli template ID
        /// </summary>
        public Guid ChilliTemplateId { get; set; }

        /// <summary>
        /// Selected template instance ID
        /// </summary>
        public Guid ChilliEditorTemplateId { get; set; }

        public string DesignFilePath { get; set; }

        /// <summary>
        /// Indicates if it is necessary to obtain design file path
        /// via calling Template product service
        /// </summary>
        public bool DesignFilePathRequired
        {
            get
            {
                return OrderItemType.Contains("KDA.TemplatedProduct");
            }
        }

        /// <summary>
        /// Template product service's task Id
        /// </summary>
        public string DesignFilePathTaskId { get; set; }

        /// <summary>
        /// indicates if DesignFilePath was already obtained
        /// </summary>
        public bool DesignFilePathObtained { get; set; } = false;
    }
}