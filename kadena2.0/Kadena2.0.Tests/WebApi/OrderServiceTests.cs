using Kadena.BusinessLogic.Contracts;
using Kadena.BusinessLogic.Services.Orders;
using Kadena.Models.Common;
using Kadena.Models.Orders;
using Kadena.Models.SiteSettings;
using Kadena.WebAPI.KenticoProviders.Contracts;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Kadena.Tests.WebApi
{
    public class OrderServiceTests
    {
        Random random = new Random();
        
        [Fact]
        public void Service_ShouldLoadPagingSettings_WhenConfigured()
        {
            var resources = Mock.Of<IKenticoResourceService>(
                res => res.GetSettingsKey(Settings.KDA_RecentOrdersPageCapacity) == "15"
            );
            var dtFormatter = Mock.Of<IDateTimeFormatter>();
            var sut = new OrderService(resources, dtFormatter);

            Assert.Equal(15, sut.OrdersPerPage);
        }

        [Fact]
        public void Service_ShouldUseDefaultPagingSettings_WhenNotConfigured()
        {
            var resources = Mock.Of<IKenticoResourceService>();
            var dtFormatter = Mock.Of<IDateTimeFormatter>();
            var sut = new OrderService(resources, dtFormatter);

            Assert.Equal(OrderService.DefaultOrdersPerPage, sut.OrdersPerPage);
        }

        [Fact]
        public void ConvertOrdersToView_ShouldThrow_WhenArgumentNull()
        {
            var resources = Mock.Of<IKenticoResourceService>();
            var dtFormatter = Mock.Of<IDateTimeFormatter>();
            var sut = new OrderService(resources, dtFormatter);

            Assert.Throws<ArgumentNullException>(() => sut.ConvertOrdersToView(null));
        }

        [Fact]
        public void ConvertOrdersToView_ShouldFlattenOrdersToTable()
        {
            var resources = Mock.Of<IKenticoResourceService>();
            var dtFormatter = Mock.Of<IDateTimeFormatter>();
            var sut = new OrderService(resources, dtFormatter);

            var ordersCount = 3;
            var perOrderItemsCount = 10;
            var allItemsCount = 30;
            var orders = new PagedData<Order> { Data = CreateTestOrders(ordersCount, perOrderItemsCount) };

            var view = sut.ConvertOrdersToView(orders);
            Assert.Equal(allItemsCount, view.Rows.Length);
        }

        [Fact]
        public void ConvertOrdersToView_ShouldMapLineItems_WhenShipped()
        {
            var resources = Mock.Of<IKenticoResourceService>();
            var dtFormatter = new DateTimeFormatterStub();
            var sut = new OrderService(resources, dtFormatter);
            var testOrder = CreateTestOrder(orderNumber: 1, itemsCount: 1);
            var orders = new PagedData<Order>() { Data = new List<Order> { testOrder } };

            var view = sut.ConvertOrdersToView(orders);

            var row = view.Rows[0];
            Assert.Equal(testOrder.Url, row.Url);
            Assert.Equal(testOrder.Site, row.Items[0]);
            Assert.Equal(testOrder.Number, row.Items[1]);
            Assert.Equal(dtFormatter.Format(testOrder.OrderingDate), row.Items[2]);
            Assert.Equal(testOrder.User, row.Items[3]);
            Assert.Equal(testOrder.Items[0].Name, row.Items[4]);
            Assert.Equal(testOrder.Items[0].SKU, row.Items[5]);
            Assert.Equal(testOrder.Items[0].Quantity, row.Items[6]);
            Assert.Equal(testOrder.Items[0].Price, row.Items[7]);
            Assert.Equal(testOrder.Status, row.Items[8]);
            Assert.Equal(dtFormatter.Format(testOrder.ShippingDate.Value), row.Items[9]);
            Assert.Equal(testOrder.TrackingNumber, row.Items[10]);
        }

        [Fact]
        public void ConvertOrdersToView_ShouldMapLineItems_WhenNotShipped()
        {
            var resources = Mock.Of<IKenticoResourceService>();
            var dtFormatter = new DateTimeFormatterStub();
            var sut = new OrderService(resources, dtFormatter);
            var testOrder = CreateTestOrder(orderNumber: 1, itemsCount: 1);
            testOrder.ShippingDate = null;
            var orders = new PagedData<Order>() { Data = new List<Order> { testOrder } };

            var view = sut.ConvertOrdersToView(orders);

            var row = view.Rows[0];
            Assert.Equal(string.Empty, row.Items[9]);
        }

        [Fact]
        public void ConvertOrdersToView_ShouldReusePagination()
        {
            var resources = Mock.Of<IKenticoResourceService>();
            var dtFormatter = new DateTimeFormatterStub();
            var sut = new OrderService(resources, dtFormatter);
            var testOrder = CreateTestOrder(orderNumber: 1, itemsCount: 1);
            var orders = new PagedData<Order>() { Data = new List<Order> { testOrder } };

            var view = sut.ConvertOrdersToView(orders);

            Assert.Equal(orders.Pagination, view.Pagination);
        }

        private List<Order> CreateTestOrders(int ordersCount, int itemsPerOrderCount)
        {
            return Enumerable.Range(1, ordersCount)
                .Select(orderNumber => CreateTestOrder(orderNumber, itemsPerOrderCount))
                .ToList();
        }

        private Order CreateTestOrder(int orderNumber, int itemsCount)
        {
            var order = new Order
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

        private LineItem CreateTestLineItem(int itemNumber)
        {
            return new LineItem
            {
                Name = "Product" + itemNumber,
                SKU = "SKU" + itemNumber,
                Quantity = random.Next(),
                Price = random.Next() / 10
            };
        }

        private DateTime GetRandomDateTime()
        {
            return DateTime.Now
                .AddSeconds(random.Next(1, 30 * 1000 * 1000));
        }

        private class DateTimeFormatterStub : IDateTimeFormatter
        {
            public string FormatString { get; set; } = "yyyyMMdd-HHmmss";

            public string Format(DateTime dt)
            {
                return dt.ToString(FormatString);
            }

            public string GetFormatString()
            {
                return FormatString;
            }

            public string GetFormatString(string cultureCode)
            {
                return GetFormatString();
            }
        }
    }
}
