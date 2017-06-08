using System.Collections.Generic;

namespace Kadena.WebAPI.Models
{
    public class CartItems
    {
        public string Number { get; set; }
        public List<CartItem> Items { get;set;}
    }
}