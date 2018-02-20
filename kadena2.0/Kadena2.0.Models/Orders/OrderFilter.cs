using System;

namespace Kadena.Models.Orders
{
    public class OrderFilter
    {
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string Sort { get; set; }
    }
}
