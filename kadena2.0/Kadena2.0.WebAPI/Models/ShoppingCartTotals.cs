using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kadena.WebAPI.Models
{
    public class ShoppingCartTotals
    {
        public double TotalItemsPrice { get; set; }
        public double TotalShipping { get; set; }
        public double TotalTax { get; set; }
        public double TotalPrice { get; set; }

        public double Subtotal
        {
            get
            {
                return TotalItemsPrice + TotalShipping;
            }
        }
    }
}