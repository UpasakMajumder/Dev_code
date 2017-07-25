using System;
using System.ComponentModel.DataAnnotations;

namespace Kadena.Dto.SubmitOrder.MicroserviceRequests
{
    public class OrderItemDTO
  {
        public OrderItemDTO(string itemType)
        {
            switch (itemType)
            {
                case "KDA.POD":
                case "KDA.StaticProduct":
                case "KDA.InventoryProduct":
                    Type = OrderItemTypeDTO.StandardOnStockItem;
                    break;

                case "KDA.TemplatedProduct":
                    Type = OrderItemTypeDTO.TemplatedProduct;
                    break;

                case "KDA.MailingProduct":
                    Type = OrderItemTypeDTO.Mailing;
                    break;
            }
        }

        public SKUDTO SKU { get; set; }

        public int LineNumber { get; set; }

        public OrderItemTypeDTO Type { get; set; }

        public double UnitPrice { get; set; }

        [Range(1, int.MaxValue, ErrorMessage ="Number of units in order's items must be greater than 0")]
        public int UnitCount { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Unit Measure field in order's items is a mandatory field")]
        public string UnitOfMeasure { get; set; }

        [Range(1, double.MaxValue, ErrorMessage = "Item's cost in order's items must be greater than 0")]
        public double TotalPrice { get; set; }

        public double TotalTax { get; set; }

        public MailingListDTO MailingList { get; set; }
        public string ChilliTaskId { get; set; }
        public string ChilliTemplateId { get; set; }
        public string DesignFilePath { get; set; }
  }
}
