using Kadena.Dto.Order;
using Kadena.Models.Orders;
using System;
using System.Collections.Generic;
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
                .Select(orderNumber => CreateTestRecentOrder(orderNumber, itemsPerOrderCount))
                .ToArray();
        }

        public static RecentOrderDto CreateTestRecentOrder(int orderNumber, int itemsCount)
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

        public static OrderItemDto CreateTestRecentOrderItem(int itemNumber)
        {
            return new OrderItemDto
            {
                Name = "order item " + itemNumber,
                Quantity = random.Next(100)
            };
        }
    }
}
