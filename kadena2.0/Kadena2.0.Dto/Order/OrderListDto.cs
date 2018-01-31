using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Kadena.Dto.Order
{
    [DataContract]
    public class OrderListDto
    {
        [DataMember(Name = "totalCount")]
        public int TotalCount { get; set; }

        [DataMember(Name = "orders")]
        public IEnumerable<RecentOrderDto> Orders { get; set; }
    }
}