namespace Kadena.Models.OrderHistory
{
    public class OrderChanges
    {
        public string Title { get; set; }
        public string ColumnHeaderCategory { get; set; }
        public string ColumnHeaderOldValue { get; set; }
        public string ColumnHeaderNewValue { get; set; }
        public string ColumnHeaderDate { get; set; }
        public string ColumnHeaderUser { get; set; }
        public OrderChange[] Items { get; set; }
    }
}
