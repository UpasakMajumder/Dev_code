namespace Kadena.Models.Common
{
    public class TableView
    {
        public string[] Headers { get; set; } = new string[0];
        public TableRow[] Rows { get; set; } = new TableRow[0];
        public Pagination Pagination { get; set; } = new Pagination();
    }
}
