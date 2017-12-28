using System.Collections.Generic;

namespace Kadena.Dto.RecentOrders
{
    public class OrderHeadBlockDto
    {
        public List<string> headers { get; set; }
        public PaginationDto PageInfo { get; set; }
        public string NoOrdersMessage { get; set; }
        public IEnumerable<RecentOrderRowDto> rows { get; set; }
    }
}