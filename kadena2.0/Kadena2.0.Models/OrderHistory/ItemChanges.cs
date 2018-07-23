namespace Kadena.Models.OrderHistory
{
    public class ItemChanges
    {
        public string Title { get; set; }
        public string ColumnHeaderItemDescription { get; set; }
        public string ColumnHeaderChangeType { get; set; }
        public string ColumnHeaderOldValue { get; set; }
        public string ColumnHeaderNewValue { get; set; }
        public string ColumnHeaderDate { get; set; }
        public string ColumnHeaderUser { get; set; }
        public ItemChange[] Items { get; set; }
    }
}
