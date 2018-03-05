using Kadena.BusinessLogic.Contracts;
using Kadena.BusinessLogic.Services.Orders;
using Kadena.Dto.General;
using Kadena.Dto.Order;
using Kadena.Models;
using Kadena.Models.Common;
using Kadena.Models.Orders;
using Kadena.Models.SiteSettings;
using Kadena.WebAPI.KenticoProviders.Contracts;
using Kadena2.MicroserviceClients.Contracts;
using Kadena2.WebAPI.KenticoProviders.Contracts;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Kadena.Tests.WebApi
{
    public class OrderReportServiceTests
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
            var sut = new OrderReportServiceBuilder()
                .WithResources(resources)
                .Build();

            Assert.Equal(15, sut.OrdersPerPage);
        }

        [Fact]
        public void Service_ShouldUseDefaultPagingSettings_WhenNotConfigured()
        {
            var sut = new OrderReportServiceBuilder()
                .Build();

            Assert.Equal(OrderReportService.DefaultCountOfOrdersPerPage, sut.OrdersPerPage);
        }



        [Fact]
        public void ConvertOrdersToView_ShouldThrow_WhenArgumentNull()
        {
            var sut = new OrderReportServiceBuilder()
                .Build();

            Assert.Throws<ArgumentNullException>(() => sut.ConvertOrdersToView(null));
        }

        [Fact]
        public void ConvertOrdersToView_ShouldFlattenOrdersToTable()
        {
            var sut = new OrderReportServiceBuilder()
                .Build();

            var ordersCount = 3;
            var perOrderItemsCount = 10;
            var allItemsCount = 30;
            var orders = new PagedData<OrderReport> { Data = CreateTestOrders(ordersCount, perOrderItemsCount) };

            var view = sut.ConvertOrdersToView(orders);
            Assert.Equal(allItemsCount, view.Rows.Length);
        }

        [Fact]
        public void ConvertOrdersToView_ShouldMapLineItems_WhenShipped()
        {
            var dtFormatter = new DateTimeFormatterStub();
            var sut = new OrderReportServiceBuilder()
                .WithDateTimeFormatter(dtFormatter)
                .Build();
            var testOrder = CreateTestOrder(orderNumber: 1, itemsCount: 1);
            var orders = new PagedData<OrderReport>() { Data = new List<OrderReport> { testOrder } };

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
            var sut = new OrderReportServiceBuilder()
                .Build();
            var testOrder = CreateTestOrder(orderNumber: 1, itemsCount: 1);
            testOrder.ShippingDate = null;
            var orders = new PagedData<OrderReport>() { Data = new List<OrderReport> { testOrder } };

            var view = sut.ConvertOrdersToView(orders);

            var row = view.Rows[0];
            Assert.Equal(string.Empty, row.Items[9]);
        }

        [Fact]
        public void ConvertOrdersToView_ShouldReusePagination()
        {
            var sut = new OrderReportServiceBuilder()
                .Build();
            var testOrder = CreateTestOrder(orderNumber: 1, itemsCount: 1);
            var orders = new PagedData<OrderReport>() { Data = new List<OrderReport> { testOrder } };

            var view = sut.ConvertOrdersToView(orders);

            Assert.Equal(orders.Pagination, view.Pagination);
        }



        [Fact]
        public void GetOrders_ShouldUseCurrentSite()
        {
            var currentSite = "test_site";
            var siteProvider = Mock.Of<IKenticoSiteProvider>(sp => sp.GetCurrentSiteCodeName() == currentSite);
            var sut = new OrderReportServiceBuilder()
                .WithSiteProvider(siteProvider)
                .BuildSpy<OrderService_GetOrdersForSiteSpy>();
            var page = 2;
            var filter = new OrderFilter();

            sut.GetOrders(page, filter);

            Assert.Equal(currentSite, sut.Site);
            Assert.Equal(page, sut.Page);
            Assert.Equal(filter, sut.Filter);
        }

        [Fact]
        public void GetOrdersForSite_ShouldValidateArgumentsAndThrow_WhenInvalidPage()
        {
            var sut = new OrderReportServiceBuilder()
                .Build();
            var invalidPageNumber = OrderReportService.FirstPageNumber - 1;
            var filter = new OrderFilter();

            Func<Task> action = () => sut.GetOrdersForSite("test_site", invalidPageNumber, filter);

            Assert.ThrowsAsync<ArgumentException>("page", action);
        }

        [Fact]
        public void GetOrdersForSite_ShouldValidateFilter()
        {
            var sut = new OrderReportServiceBuilder()
                .BuildSpy<OrderReportService_ValidateFilterSpy>();

            sut.GetOrdersForSite("test_site", OrderReportService.FirstPageNumber, new OrderFilter());

            Assert.True(sut.ValidateFilterCalled);
        }

        [Fact]
        public void GetOrdersForSite_ShouldPassArgumentsToMicroserviceClient()
        {
            var orderViewClient = new Mock<IOrderViewClient>();
            var sut = new OrderReportServiceBuilder()
                .WithOrderViewClient(orderViewClient.Object)
                .Build();
            var page = 2;
            var currentSite = "test_site";
            var filter = new OrderFilter
            {
                FromDate = new DateTime(),
                ToDate = new DateTime(),
                Sort = $"{OrderReportService.SortableByOrderDate}-{OrderFilter.SortDirection.DESC}"
            };

            sut.GetOrdersForSite(currentSite, page, filter);

            orderViewClient.Verify(ovc
                => ovc.GetOrders(currentSite, null, page, sut.OrdersPerPage,
                    filter.FromDate, filter.ToDate, OrderReportService.SortableByOrderDate,
                    true, null, null), Times.Once());
        }

        [Fact]
        public async Task GetOrdersForSite_ShouldMapResponseFromMicroserviceClient()
        {
            var customer = new Customer
            {
                FirstName = "Bruce",
                LastName = "Wayne"
            };
            var kenticoUserProvider = Mock.Of<IKenticoUserProvider>(
                kup => kup.GetCustomer(It.IsAny<int>()) == customer
            );

            var ordersCount = 2;
            var itemsCount = 5;
            var pagesCount = 1; // based on ordersCount / OrderReportService.DefaultCountOfOrdersPerPage
            var orders = CreateTestRecentOrders(ordersCount, itemsCount);
            var input = new BaseResponseDto<OrderListDto>
            {
                Success = true,
                Payload = new OrderListDto
                {
                    TotalCount = ordersCount,
                    Orders = orders
                }
            };

            var orderViewClient = Mock.Of<IOrderViewClient>(
                ovc => ovc.GetOrders(It.IsAny<string>(), It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<int?>(), 
                    It.IsAny<DateTime?>(), It.IsAny<DateTime?>(), It.IsAny<string>(), It.IsAny<bool>(),
                    It.IsAny<int?>(), It.IsAny<string>()) == Task.FromResult(input)
            );

            var sut = new OrderReportServiceBuilder()
                .WithKenticoUserProvider(kenticoUserProvider)
                .WithOrderViewClient(orderViewClient)
                .Build();

            var site = "test_site";
            var page = 1;
            var filter = new OrderFilter();

            var output = await sut.GetOrdersForSite(site, page, filter);

            // pagination
            Assert.Equal(ordersCount, output.Pagination.RowsCount);
            Assert.Equal(page, output.Pagination.CurrentPage);
            Assert.Equal(pagesCount, output.Pagination.PagesCount);
            Assert.Equal(sut.OrdersPerPage, output.Pagination.RowsOnPage);

            // order header
            Assert.Equal(ordersCount, output.Data.Count);
            Assert.Equal(orders[0].CreateDate, output.Data[0].OrderingDate);
            Assert.Equal(orders[0].Id, output.Data[0].Number);
            Assert.Equal(orders[0].ShippingDate, output.Data[0].ShippingDate);
            Assert.Equal(sut.FormatOrderStatus(orders[0].Status), output.Data[0].Status);
            Assert.Equal(sut.FormatCustomer(customer), output.Data[0].User);
            Assert.Equal(orders[0].SiteName, output.Data[0].Site);
            Assert.Equal(orders[0].TrackingNumber, output.Data[0].TrackingNumber);
            Assert.Equal(sut.FormatDetailUrl(orders[0]), output.Data[0].Url);

            // order items
            var firstItem = orders[0].Items.First();
            Assert.Equal(itemsCount, output.Data[0].Items.Count);
            Assert.Equal(firstItem.Name, output.Data[0].Items[0].Name);
            Assert.Equal(firstItem.Quantity, output.Data[0].Items[0].Quantity);
            Assert.Equal(firstItem.Price, output.Data[0].Items[0].Price);
            Assert.Equal(firstItem.SKU, output.Data[0].Items[0].SKU);
        }



        [Fact]
        public void FormatCustomer_ShouldUseName_WhenNameAvailable()
        {
            var customer = new Customer
            {
                FirstName = "Bruce",
                LastName = "Wayne"
            };
            var expected = "Bruce Wayne";

            var sut = new OrderReportServiceBuilder()
                .Build();

            var actual = sut.FormatCustomer(customer);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void FormatCustomer_ShouldUseEmail_WhenNameNotAvailable()
        {
            var customer = new Customer
            {
                Email = "bruce.wayne@therealbat.com"
            };
            var expected = "bruce.wayne@therealbat.com";

            var sut = new OrderReportServiceBuilder()
                .Build();

            var actual = sut.FormatCustomer(customer);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void FormatDetailUrl_ShouldGenerateUrl()
        {
            var detailUrlBase = "test.com/product";
            var resources = Mock.Of<IKenticoResourceService>(res 
                => res.GetSettingsKey(Settings.KDA_OrderDetailUrl) == detailUrlBase);
            var documents = Mock.Of<IKenticoDocumentProvider>(doc
                => doc.GetDocumentUrl(It.IsAny<string>(), It.IsAny<bool>()) == detailUrlBase);

            var order = new RecentOrderDto
            {
                Id = "1234"
            };
            var expectedUrl = "test.com/product?orderID=1234";

            var sut = new OrderReportServiceBuilder()
                .WithResources(resources)
                .WithDocuments(documents)
                .Build();

            var actualUrl = sut.FormatDetailUrl(order);

            Assert.Equal(expectedUrl, actualUrl);
        }

        [Fact]
        public void FormatOrderStatus_ShouldMapOrderStatus()
        {
            var microserviceStatus = "some micro status";
            var mappedStatus = "some status";
            var orderProvider = Mock.Of<IKenticoOrderProvider>(kop 
                => kop.MapOrderStatus(microserviceStatus) == mappedStatus);

            var sut = new OrderReportServiceBuilder()
                .WithKenticoOrderProvider(orderProvider)
                .Build();

            var actualStatus = sut.FormatOrderStatus(microserviceStatus);

            Assert.Equal(mappedStatus, actualStatus);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void ValidateFilter_ShouldNotThrow_WhenFilterIsEmpty(string sort)
        {
            var sut = new OrderReportServiceBuilder()
                .Build();
            var filterWithEmptySort = new OrderFilter { Sort = sort };

            sut.ValidateFilter(filterWithEmptySort);

            Assert.True(true);
        }

        [Theory]
        [InlineData("wrong-sort-expression")]
        [InlineData("not_supported_property-ASC")]
        public void ValidateFilter_ShouldThrow_WhenFilterIsInvalid(string sort)
        {
            var sut = new OrderReportServiceBuilder()
                .Build();
            var filterWithInvalidSort = new OrderFilter { Sort = sort };

            Action action = () => sut.ValidateFilter(filterWithInvalidSort);

            Assert.Throws<ArgumentException>(action);
        }

        [Fact]
        public void GetOrdersExport_ShouldUseCurrentSite()
        {
            var currentSite = "test_site";
            var siteProvider = Mock.Of<IKenticoSiteProvider>(sp => sp.GetCurrentSiteCodeName() == currentSite);
            var sut = new OrderReportServiceBuilder()
                .WithSiteProvider(siteProvider)
                .BuildSpy<OrderService_GetOrdersExportForSiteSpy>();
            var filter = new OrderFilter();

            sut.GetOrdersExport(filter);

            Assert.Equal(currentSite, sut.Site);
            Assert.Equal(filter, sut.Filter);
        }

        [Fact]
        public void GetOrdersExportForSite_ShouldValidateFilter()
        {
            var sut = new OrderReportServiceBuilder()
                .BuildSpy<OrderReportService_ValidateFilterSpy>();

            sut.GetOrdersExportForSite("test_site", new OrderFilter());

            Assert.True(sut.ValidateFilterCalled);
        }

        [Fact]
        public async Task GetOrdersExportForSite_ShouldCreateExportFileResult()
        {
            var sut = new OrderReportServiceBuilder()
                .Build();

            var result = await sut.GetOrdersExportForSite("test_site", new OrderFilter());

            Assert.Equal("export.xlsx", result.Name);
            Assert.Equal(ContentTypes.Xlsx, result.Name);
            Assert.Equal("export.xlsx", result.Name);
        }

        private RecentOrderDto[] CreateTestRecentOrders(int ordersCount, int itemsPerOrderCount)
        {
            return Enumerable.Range(1, ordersCount)
                .Select(orderNumber => CreateTestRecentOrder(orderNumber, itemsPerOrderCount))
                .ToArray();
        }

        private RecentOrderDto CreateTestRecentOrder(int orderNumber, int itemsCount)
        {
            var order = new RecentOrderDto
            {
                CreateDate = GetRandomDateTime(),
                CustomerId = 1,
                Id = GetRandomString(),
                ShippingDate = GetRandomDateTime(),
                Status = GetRandomString(),
                TotalPrice = random.Next(1000),
                Items = Enumerable.Range(1, itemsCount)
                    .Select(CreateTestRecentOrderItem)
                    .ToArray()
            };
            return order;
        }

        private OrderItemDto CreateTestRecentOrderItem(int itemNumber)
        {
            return new OrderItemDto
            {
                Name = "order item " + itemNumber,
                Quantity = random.Next(100)
            };
        }

        private List<OrderReport> CreateTestOrders(int ordersCount, int itemsPerOrderCount)
        {
            return Enumerable.Range(1, ordersCount)
                .Select(orderNumber => CreateTestOrder(orderNumber, itemsPerOrderCount))
                .ToList();
        }

        private OrderReport CreateTestOrder(int orderNumber, int itemsCount)
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

        private ReportLineItem CreateTestLineItem(int itemNumber)
        {
            return new ReportLineItem
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

        private string GetRandomString()
        {
            return Guid.NewGuid()
                .ToString()
                .Replace("-", "");
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

        private class OrderService_GetOrdersForSiteSpy : OrderReportService
        {
            public OrderService_GetOrdersForSiteSpy(
                IKenticoResourceService kenticoResources, IKenticoSiteProvider kenticoSiteProvider, 
                IDateTimeFormatter dateTimeFormatter, IOrderViewClient orderViewClient,
                IKenticoUserProvider kenticoUserProvider,
                IKenticoDocumentProvider kenticoDocumentProvider,
                IKenticoOrderProvider kenticoOrderProvider)
                : base(kenticoResources, kenticoSiteProvider, dateTimeFormatter, 
                      orderViewClient, kenticoUserProvider, kenticoDocumentProvider,
                      kenticoOrderProvider)
            {
            }

            public string Site { get; private set; }
            public int Page { get; private set; }
            public OrderFilter Filter { get; private set; }

            public override Task<PagedData<OrderReport>> GetOrdersForSite(string site, int page, OrderFilter filter)
            {
                Site = site;
                Page = page;
                Filter = filter;

                return base.GetOrdersForSite(site, page, filter);
            }
        }

        private class OrderService_GetOrdersExportForSiteSpy : OrderReportService
        {
            public OrderService_GetOrdersExportForSiteSpy(
                IKenticoResourceService kenticoResources, IKenticoSiteProvider kenticoSiteProvider,
                IDateTimeFormatter dateTimeFormatter, IOrderViewClient orderViewClient,
                IKenticoUserProvider kenticoUserProvider,
                IKenticoDocumentProvider kenticoDocumentProvider,
                IKenticoOrderProvider kenticoOrderProvider)
                : base(kenticoResources, kenticoSiteProvider, dateTimeFormatter,
                      orderViewClient, kenticoUserProvider, kenticoDocumentProvider,
                      kenticoOrderProvider)
            {
            }

            public string Site { get; private set; }
            public OrderFilter Filter { get; private set; }

            public override Task<FileResult> GetOrdersExportForSite(string site, OrderFilter filter)
            {
                Site = site;
                Filter = filter;

                return base.GetOrdersExportForSite(site, filter);
            }
        }

        private class OrderReportService_ValidateFilterSpy : OrderReportService
        {
            public OrderReportService_ValidateFilterSpy(
                IKenticoResourceService kenticoResources, IKenticoSiteProvider kenticoSiteProvider,
                IDateTimeFormatter dateTimeFormatter, IOrderViewClient orderViewClient,
                IKenticoUserProvider kenticoUserProvider,
                IKenticoDocumentProvider kenticoDocumentProvider,
                IKenticoOrderProvider kenticoOrderProvider)
                : base(kenticoResources, kenticoSiteProvider, dateTimeFormatter,
                      orderViewClient, kenticoUserProvider, kenticoDocumentProvider,
                      kenticoOrderProvider)
            {
            }

            public bool ValidateFilterCalled { get; private set; }

            public override void ValidateFilter(OrderFilter filter)
            {
                ValidateFilterCalled = true;

                base.ValidateFilter(filter);
            }
        }

        private class OrderReportServiceBuilder
        {
            IKenticoResourceService kenticoResources;
            IKenticoSiteProvider kenticoSiteProvider;
            IDateTimeFormatter dateTimeFormatter;
            IOrderViewClient orderViewClient;
            IKenticoUserProvider kenticoUserProvider;
            IKenticoDocumentProvider kenticoDocumentProvider;
            IKenticoOrderProvider kenticoOrderProvider;

            public OrderReportService Build()
            {
                return new OrderReportService(
                    kenticoResources ?? Mock.Of<IKenticoResourceService>(),
                    kenticoSiteProvider ?? Mock.Of<IKenticoSiteProvider>(),
                    dateTimeFormatter ?? new DateTimeFormatterStub(),
                    orderViewClient ?? Mock.Of<IOrderViewClient>(),
                    kenticoUserProvider ?? Mock.Of<IKenticoUserProvider>(),
                    kenticoDocumentProvider ?? Mock.Of<IKenticoDocumentProvider>(),
                    kenticoOrderProvider ?? Mock.Of<IKenticoOrderProvider>()
                );
            }

            public T BuildSpy<T>() where T : OrderReportService 
            {
                return (T)Activator.CreateInstance(typeof(T),
                    kenticoResources ?? Mock.Of<IKenticoResourceService>(),
                    kenticoSiteProvider ?? Mock.Of<IKenticoSiteProvider>(),
                    dateTimeFormatter ?? new DateTimeFormatterStub(),
                    orderViewClient ?? Mock.Of<IOrderViewClient>(),
                    kenticoUserProvider ?? Mock.Of<IKenticoUserProvider>(),
                    kenticoDocumentProvider ?? Mock.Of<IKenticoDocumentProvider>(),
                    kenticoOrderProvider ?? Mock.Of<IKenticoOrderProvider>()
                );
            }

            public OrderReportServiceBuilder WithResources(IKenticoResourceService kenticoResources)
            {
                this.kenticoResources = kenticoResources;
                return this;
            }

            public OrderReportServiceBuilder WithDocuments(IKenticoDocumentProvider kenticoDocumentProvider)
            {
                this.kenticoDocumentProvider = kenticoDocumentProvider;
                return this;
            }

            public OrderReportServiceBuilder WithSiteProvider(IKenticoSiteProvider kenticoSiteProvider)
            {
                this.kenticoSiteProvider = kenticoSiteProvider;
                return this;
            }

            public OrderReportServiceBuilder WithDateTimeFormatter(IDateTimeFormatter dateTimeFormatter)
            {
                this.dateTimeFormatter = dateTimeFormatter;
                return this;
            }

            public OrderReportServiceBuilder WithOrderViewClient(IOrderViewClient orderViewClient)
            {
                this.orderViewClient = orderViewClient;
                return this;
            }

            public OrderReportServiceBuilder WithKenticoUserProvider(IKenticoUserProvider kenticoUserProvider)
            {
                this.kenticoUserProvider = kenticoUserProvider;
                return this;
            }

            public OrderReportServiceBuilder WithKenticoOrderProvider(IKenticoOrderProvider kenticoOrderProvider)
            {
                this.kenticoOrderProvider = kenticoOrderProvider;
                return this;
            }
        }
    }
}
