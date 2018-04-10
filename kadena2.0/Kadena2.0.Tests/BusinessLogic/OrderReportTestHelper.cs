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

        public static OrderReport CreateTestOrder(int orderNumber, int itemsCount)
        {
            var order = new OrderReport
            {
                Number = orderNumber.ToString(),
                OrderingDate = GetRandomDateTime(),
                ShippingDate = GetRandomDateTime(),
                Site = "test_site",
                Status = "test_status",
                TrackingNumber = random.Next(1, 1000000).ToString(),
                User = "test_user",
                Url = "http://test.com/this/is/test/url/",
                Items = Enumerable.Range(1, itemsCount)
                    .Select(itemNumber => CreateTestLineItem(itemNumber))
                    .ToList()
            };
            return order;
        }

        public static ReportLineItem CreateTestLineItem(int itemNumber)
        {
            return new ReportLineItem
            {
                Name = "Product" + itemNumber,
                SKU = "SKU" + itemNumber,
                Quantity = random.Next(),
                Price = random.Next() / 10
            };
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

        public static List<OrderReport> CreateTestOrders(int ordersCount, int itemsPerOrderCount)
        {
            return Enumerable.Range(1, ordersCount)
                .Select(orderNumber => OrderReportTestHelper.CreateTestOrder(orderNumber, itemsPerOrderCount))
                .ToList();
        }
    }
}
