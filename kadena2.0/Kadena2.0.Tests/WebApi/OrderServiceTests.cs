using Kadena.BusinessLogic.Contracts;
using Kadena.BusinessLogic.Services.Orders;
using Kadena.Models.Common;
using Kadena.Models.Orders;
using Kadena.Models.SiteSettings;
using Kadena.WebAPI.KenticoProviders.Contracts;
using Kadena2.MicroserviceClients.Contracts;
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

        public class OrderFilterTests
        {
            [Theory]
            [InlineData("INVALID")]
            [InlineData("-ASC")]
            [InlineData(" -ASC")]
            [InlineData("prop-RANDOM")]
            public void TryParseSort_ShouldBeFalse_WhenSortIsInvalid(string invalidSort)
            {
                var filter = new OrderFilter { Sort = invalidSort };

                OrderFilter.SortFields sortInfo;
                var isValid = filter.TryParseSort(out sortInfo);

                Assert.False(isValid);
            }

            [Theory]
            [InlineData(OrderFilter.SortDirection.ASC, "propA", "propA-ASC")]
            [InlineData(OrderFilter.SortDirection.DESC, "propB", "propB-DESC")]
            public void TryParseSort_ShouldBeTrueAndParsed_WhenSortValid(OrderFilter.SortDirection direction, string property, string validSort)
            {
                var filter = new OrderFilter { Sort = validSort };

                OrderFilter.SortFields sortInfo;
                var isValid = filter.TryParseSort(out sortInfo);

                Assert.True(isValid);
                Assert.Equal(property, sortInfo.Property);
                Assert.Equal(direction, sortInfo.Direction);
            }
        }        

        [Fact]
        public void Service_ShouldLoadPagingSettings_WhenConfigured()
        {
            var resources = Mock.Of<IKenticoResourceService>(
                res => res.GetSettingsKey(Settings.KDA_RecentOrdersPageCapacity) == "15"
            );
            var dtFormatter = Mock.Of<IDateTimeFormatter>();
            var orderViewClient = Mock.Of<IOrderViewClient>();
            var siteProvider = Mock.Of<IKenticoSiteProvider>();
            var sut = new OrderService(resources, siteProvider, dtFormatter, orderViewClient);

            Assert.Equal(15, sut.OrdersPerPage);
        }

        [Fact]
        public void Service_ShouldUseDefaultPagingSettings_WhenNotConfigured()
        {
            var resources = Mock.Of<IKenticoResourceService>();
            var dtFormatter = Mock.Of<IDateTimeFormatter>();
            var orderViewClient = Mock.Of<IOrderViewClient>();
            var siteProvider = Mock.Of<IKenticoSiteProvider>();
            var sut = new OrderService(resources, siteProvider, dtFormatter, orderViewClient);

            Assert.Equal(OrderService.DefaultCountOfOrdersPerPage, sut.OrdersPerPage);
        }

        [Fact]
        public void ConvertOrdersToView_ShouldThrow_WhenArgumentNull()
        {
            var resources = Mock.Of<IKenticoResourceService>();
            var dtFormatter = Mock.Of<IDateTimeFormatter>();
            var orderViewClient = Mock.Of<IOrderViewClient>();
            var siteProvider = Mock.Of<IKenticoSiteProvider>();
            var sut = new OrderService(resources, siteProvider, dtFormatter, orderViewClient);

            Assert.Throws<ArgumentNullException>(() => sut.ConvertOrdersToView(null));
        }

        [Fact]
        public void ConvertOrdersToView_ShouldFlattenOrdersToTable()
        {
            var resources = Mock.Of<IKenticoResourceService>();
            var dtFormatter = Mock.Of<IDateTimeFormatter>();
            var orderViewClient = Mock.Of<IOrderViewClient>();
            var siteProvider = Mock.Of<IKenticoSiteProvider>();
            var sut = new OrderService(resources, siteProvider, dtFormatter, orderViewClient);

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
            var orderViewClient = Mock.Of<IOrderViewClient>();
            var siteProvider = Mock.Of<IKenticoSiteProvider>();
            var sut = new OrderService(resources, siteProvider, dtFormatter, orderViewClient);
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
            var orderViewClient = Mock.Of<IOrderViewClient>();
            var siteProvider = Mock.Of<IKenticoSiteProvider>();
            var sut = new OrderService(resources, siteProvider, dtFormatter, orderViewClient);
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
            var orderViewClient = Mock.Of<IOrderViewClient>();
            var siteProvider = Mock.Of<IKenticoSiteProvider>();
            var sut = new OrderService(resources, siteProvider, dtFormatter, orderViewClient);
            var testOrder = CreateTestOrder(orderNumber: 1, itemsCount: 1);
            var orders = new PagedData<Order>() { Data = new List<Order> { testOrder } };

            var view = sut.ConvertOrdersToView(orders);

            Assert.Equal(orders.Pagination, view.Pagination);
        }

        [Fact]
        public void GetOrders_ShouldUseCurrentSite()
        {
            var resources = Mock.Of<IKenticoResourceService>();
            var dtFormatter = new DateTimeFormatterStub();
            var orderViewClient = new Mock<IOrderViewClient>();
            var currentSite = "test_site";
            var siteProvider = Mock.Of<IKenticoSiteProvider>(sp => sp.GetCurrentSiteCodeName() == currentSite);
            var sut = new OrderService(resources, siteProvider, dtFormatter, orderViewClient.Object);
            var page = 2;
            var filter = new OrderFilter();

            sut.GetOrders(page, filter);

            orderViewClient.Verify(ovc
                => ovc.GetOrders(currentSite, null, page, sut.OrdersPerPage,
                    filter.FromDate, filter.ToDate, null,
                    false, null, null), Times.Once());
        }

        [Fact]
        public void GetOrdersForSite_ShouldValidateArgumentsAndThrow_WhenInvalidPage()
        {
            var resources = Mock.Of<IKenticoResourceService>();
            var dtFormatter = new DateTimeFormatterStub();
            var orderViewClient = Mock.Of<IOrderViewClient>();
            var siteProvider = Mock.Of<IKenticoSiteProvider>();
            var sut = new OrderService(resources, siteProvider, dtFormatter, orderViewClient);
            var invalidPageNumber = OrderService.FirstPageNumber - 1;
            var filter = new OrderFilter();

            Action action = () => sut.GetOrdersForSite("test_site", invalidPageNumber, filter);

            Assert.Throws<ArgumentException>("page", action);
        }

        [Theory]
        [InlineData("wrong-sort-expression")]
        [InlineData("not_supported_property-ASC")]
        public void GetOrdersForSite_ShouldValidateArgumentsAndThrow_WhenInvalidFilter(string sort)
        {
            var resources = Mock.Of<IKenticoResourceService>();
            var dtFormatter = new DateTimeFormatterStub();
            var orderViewClient = Mock.Of<IOrderViewClient>();
            var siteProvider = Mock.Of<IKenticoSiteProvider>();
            var sut = new OrderService(resources, siteProvider, dtFormatter, orderViewClient);
            var filterWithInvalidSort = new OrderFilter { Sort = sort };
            Action action = () => sut.GetOrdersForSite("test_site", OrderService.FirstPageNumber, filterWithInvalidSort);

            Assert.Throws<ArgumentException>("filter", action);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void GetOrdersForSite_ShouldValidateArgumentsAndNotThrow_WhenFilterEmpty(string sort)
        {
            var resources = Mock.Of<IKenticoResourceService>();
            var dtFormatter = new DateTimeFormatterStub();
            var orderViewClient = Mock.Of<IOrderViewClient>();
            var siteProvider = Mock.Of<IKenticoSiteProvider>();
            var sut = new OrderService(resources, siteProvider, dtFormatter, orderViewClient);
            var filterWithEmptySort = new OrderFilter { Sort = sort };

            sut.GetOrdersForSite("test_site", OrderService.FirstPageNumber, filterWithEmptySort);

            Assert.True(true);
        }

        [Fact]
        public void GetOrdersForSite_ShouldPassArgumentsToMicroserviceClient()
        {
            var resources = Mock.Of<IKenticoResourceService>();
            var dtFormatter = new DateTimeFormatterStub();
            var orderViewClient = new Mock<IOrderViewClient>();
            var siteProvider = Mock.Of<IKenticoSiteProvider>();
            var sut = new OrderService(resources, siteProvider, dtFormatter, orderViewClient.Object);
            var page = 2;
            var currentSite = "test_site";
            var filter = new OrderFilter
            {
                FromDate = new DateTime(),
                ToDate = new DateTime(),
                Sort = $"{OrderService.SortableByOrderDate}-{OrderFilter.SortDirection.DESC}"
            };

            sut.GetOrdersForSite(currentSite, page, filter);

            orderViewClient.Verify(ovc 
                => ovc.GetOrders(currentSite, null, page, sut.OrdersPerPage, 
                    filter.FromDate, filter.ToDate, OrderService.SortableByOrderDate, 
                    true, null, null), Times.Once());
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
