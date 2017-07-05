using System.Collections.Generic;

namespace Kadena.Dto.Checkout
{
    public class CartItemsDTO
    {
        public string Number { get;set;}
        public List<CartItemDTO> Items { get; set; }
    }
}
