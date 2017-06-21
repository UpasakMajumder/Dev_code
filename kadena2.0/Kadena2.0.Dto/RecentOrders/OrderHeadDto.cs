using System.Collections.Generic;

namespace Kadena.Dto.RecentOrders
{
    public class OrderHeadDto
    {
        public List<string> Headings { get; set; }
        public PaginationDto Pagination { get; set; }
    }
}
