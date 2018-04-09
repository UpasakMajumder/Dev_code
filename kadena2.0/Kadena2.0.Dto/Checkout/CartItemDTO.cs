using Kadena.Dto.Common;
using System.Collections.Generic;

namespace Kadena.Dto.Checkout
{
    public class CartItemDTO
    {
        public int Id { get; set; }
        public string Image { get; set; }
        public string Template { get; set; }
        public bool IsMailingList { get; set; }
        public string MailingList { get; set; }
        public string Delivery { get; set; }
        public string PricePrefix { get; set; }
        public string Price { get; set; }
        public ButtonDto Preview { get; set; }
        public ButtonDto EmailProof { get; set; }
        public bool IsEditable { get; set; }
        public bool IsQuantityEditable { get; set; }
        public string QuantityPrefix { get; set; }
        public int Quantity { get; set; }
        public int StockQuantity { get; set; }
        public string EditorURL { get; set; }
        public string MailingListPrefix { get; set; }
        public string TemplatePrefix { get; set; }
        public string ProductionTime { get; set; }
        public string ShipTime { get; set; }
        public IEnumerable<ItemOptionDto> Options { get; set; }
    }
}
