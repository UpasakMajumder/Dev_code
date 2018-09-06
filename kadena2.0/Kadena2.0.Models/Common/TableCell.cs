namespace Kadena.Models.Common
{
    public class TableCell
    {
        public string Type { get; set; }
        public object Value { get; set; }

        public TableCell()
        {
        }

        public TableCell(object value)
        {
            Value = value ?? string.Empty;
        }
    }
}
