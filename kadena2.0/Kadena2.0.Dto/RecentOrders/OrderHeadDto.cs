using Kadena.Dto.Common;
using System.Collections.Generic;

namespace Kadena.Dto.RecentOrders
{
    public class OrderHeadDto
    {
        public List<string> Headings { get; set; }
        public PaginationDto PageInfo { get; set; }
        public string NoOrdersMessage { get; set; }
        public List<OrderRowDto> Rows { get; set; }
    }
}
