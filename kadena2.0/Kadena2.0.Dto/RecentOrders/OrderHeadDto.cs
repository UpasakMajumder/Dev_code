using System.Collections.Generic;

namespace Kadena.Dto.RecentOrders
{
    public class OrderHeadDto
    {
        public List<string> Headings { get; set; }
        public PaginationDto PageInfo { get; set; }
        public string NoOrdersMessage { get; set; }
    }
}
