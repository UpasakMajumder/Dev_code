using System;

namespace Kadena.Models.OrderHistory
{
    public class OrderChange
    {
        public string Category { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
        public DateTime Date { get; set; }
        public string User { get; set; }
    }
}
