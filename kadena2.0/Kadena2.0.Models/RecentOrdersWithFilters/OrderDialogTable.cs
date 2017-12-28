using System.Collections.Generic;

namespace Kadena.Models.RecentOrders
{
    public class OrderDialogTable
    {
        public List<string> headers { get; set; }
        public List<List<OrderDialogTableCell>> rows { get; set; }
    }
}
