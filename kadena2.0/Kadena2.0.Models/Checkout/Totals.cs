using System.Collections.Generic;

namespace Kadena.Models.Checkout
{
    public class Totals
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public List<Total> Items { get; set; }
    }
}