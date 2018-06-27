namespace Kadena.Models.TableSorting
{
    public class OrderBy
    {
        public string Column { get; }
        public string Direction { get; }

        public OrderBy(string column, string direction)
        {
            Column = column;
            Direction = direction;
        }
    }
}
