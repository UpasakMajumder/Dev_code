using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public bool IsEditable { get; set; }
        public string QuantityPrefix { get; set; }
        public int Quantity { get; set; }
        
    }
}
