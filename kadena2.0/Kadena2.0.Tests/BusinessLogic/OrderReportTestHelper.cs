using Kadena.Dto.Order;
using System;
using System.Linq;

namespace Kadena.Tests.BusinessLogic
{
    class OrderReportTestHelper
    {
        static Random random = new Random();

        public static DateTime GetRandomDateTime()
        {
            return DateTime.Now
                .AddSeconds(random.Next(1, 30 * 1000 * 1000));
        }

        public static string GetRandomString()
        {
            return Guid.NewGuid()
                .ToString()
                .Replace("-", "");
        }

        public static RecentOrderDto[] CreateTestRecentOrders(int ordersCount, int itemsPerOrderCount)
        {
            return Enumerable.Range(1, ordersCount)
                .Select(orderNumber => CreateTestRecentOrder(itemsPerOrderCount))
                .ToArray();
        }

        private static RecentOrderDto CreateTestRecentOrder(int itemsCount)
        {
            var order = new RecentOrderDto
            {
                CreateDate = OrderReportTestHelper.GetRandomDateTime(),
                CustomerId = 1,
                Id = GetRandomString(),
                ShippingDate = OrderReportTestHelper.GetRandomDateTime(),
                Status = GetRandomString(),
                TotalPrice = random.Next(1000),
                Items = Enumerable.Range(1, itemsCount)
                    .Select(CreateTestRecentOrderItem)
                    .ToArray()
            };
            return order;
        }

        private static OrderItemDto CreateTestRecentOrderItem(int itemNumber)
        {
            return new OrderItemDto
            {
                Name = "order item " + itemNumber,
                Quantity = random.Next(100)
            };
        }
    }
}
