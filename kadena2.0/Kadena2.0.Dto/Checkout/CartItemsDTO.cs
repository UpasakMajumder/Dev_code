using System.Collections.Generic;

namespace Kadena.Dto.Checkout
{
    public class CartItemsDTO
    {
        public string Number { get;set;}
        public string ProductionTimeLabel { get; set; }
        public string ShipTimeLabel { get; set; }
        public List<CartItemDTO> Items { get; set; }
        public CartPriceDTO SummaryPrice { get; set; }
        public ButtonLabelsDto ButtonLabels { get; set; }
    }
}
