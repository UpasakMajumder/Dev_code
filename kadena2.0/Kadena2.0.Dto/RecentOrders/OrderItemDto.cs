using System.Runtime.Serialization;
using Newtonsoft.Json;
namespace Kadena.Dto.RecentOrders
{
    public class OrderItemDto
    {
        public string Name { get; set; }

        public string Quantity { get; set; }
    }
}