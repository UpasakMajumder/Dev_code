using System;

namespace Kadena.Models.OrderHistory
{
    public class ItemChange
    {
        public string ItemDescription { get; set; }
        public string ChangeType { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
        public DateTime Date { get; set; }
        public string User { get; set; }
    }
}
