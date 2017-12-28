using System.Collections.Generic;

namespace Kadena.Dto.RecentOrders
{
    public class OrderDialogTableDto
    {
        public List<string> headers { get; set; }
        public List<List<OrderDialogTableCellDto>> rows { get; set; }
    }
}
