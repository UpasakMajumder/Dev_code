using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kadena.Dto.Checkout
{
    public class CartItemsDTO
    {
        public string Number { get;set;}
        public List<CartItemDTO> Items { get; set; }
    }
}
