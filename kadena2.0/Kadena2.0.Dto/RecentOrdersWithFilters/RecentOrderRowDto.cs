using System.Collections.Generic;

namespace Kadena.Dto.RecentOrders
{
    public class RecentOrderRowDto
    {
        public OrderDialogDto dailog { get; set; }
        public List<OrderTableCellDto> items { get; set; }
    }
}
