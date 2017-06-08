using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kadena.WebAPI.Models
{
    public class CartItems
    {
        public string Number { get; set; }
        public List<CartItem> Items { get;set;}
    }
}