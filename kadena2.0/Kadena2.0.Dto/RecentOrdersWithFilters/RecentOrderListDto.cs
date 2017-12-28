using System.Collections.Generic;

namespace Kadena.Dto.RecentOrders
{
    public class RecentOrderListDto
    {
        public int TotalCount { get; set; }

        public IEnumerable<RecentOrderRowDto> Orders { get; set; }
    }
}