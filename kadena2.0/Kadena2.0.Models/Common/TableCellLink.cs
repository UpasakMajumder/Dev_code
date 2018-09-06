namespace Kadena.Models.Common
{
    public class TableCellLink : TableCell
    {
        public string Url { get; set; }

        public TableCellLink(string url)
        {
            Url = url;
        }

        public TableCellLink(string url, object value) : base(value)
        {
            Url = url;
        }
    }
}
