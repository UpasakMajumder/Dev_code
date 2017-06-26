using System.Collections.Generic;

namespace Kadena.Old_App_Code.Kadena.Orders
{
    public class OrderHistoryDataContainer
    {
        public int totalCount { get; set; }
        public IEnumerable<OrderHistoryData> orders { get; set; }
    }
}