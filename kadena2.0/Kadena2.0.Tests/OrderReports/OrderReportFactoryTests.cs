using Kadena.BusinessLogic.Contracts;
using Kadena.BusinessLogic.Factories;
using Kadena.Dto.Order;
using Kadena.Models;
using Kadena.Models.Orders;
using Kadena.Models.SiteSettings;
using Kadena.WebAPI.KenticoProviders.Contracts;
using Kadena2.WebAPI.KenticoProviders.Contracts;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Kadena.Tests.OrderReports
{
    public class OrderReportFactoryTests
    {
        class OrderReportFactoryBuilder
        {
            IKenticoOrderProvider kenticoOrderProvider;
            IKenticoResourceService kenticoResourceService;
            IKenticoDocumentProvider kenticoDocumentProvider;
            IDateTimeFormatter dateTimeFormatter;
            IKenticoUserProvider kenticoUserProvider;

            public OrderReportFactory Build()
            {
                return new OrderReportFactory(
                    kenticoOrderProvider ?? Mock.Of<IKenticoOrderProvider>(),
                    kenticoUserProvider ?? Mock.Of<IKenticoUserProvider>(),
                    dateTimeFormatter ?? Mock.Of<IDateTimeFormatter>(),
                    kenticoResourceService ?? Mock.Of<IKenticoResourceService>(),
                    kenticoDocumentProvider ?? Mock.Of<IKenticoDocumentProvider>());
            }

            public OrderReportFactoryBuilder WithDateTimeFormatter(IDateTimeFormatter dateTimeFormatter)
            {
                this.dateTimeFormatter = dateTimeFormatter;
                return this;
            }

            public OrderReportFactoryBuilder WithKenticoDocumentProvider(IKenticoDocumentProvider kenticoDocumentProvider)
            {
                this.kenticoDocumentProvider = kenticoDocumentProvider;
                return this;
            }

            public OrderReportFactoryBuilder WithKenticoResourceService(IKenticoResourceService kenticoResourceService)
            {
                this.kenticoResourceService = kenticoResourceService;
                return this;
            }

            public OrderReportFactoryBuilder WithKenticoOrderProvider(IKenticoOrderProvider kenticoOrderProvider)
            {
                this.kenticoOrderProvider = kenticoOrderProvider;
                return this;
            }

            public OrderReportFactoryBuilder WithKenticoUserProvider(IKenticoUserProvider kenticoUserProvider)
            {
                this.kenticoUserProvider = kenticoUserProvider;
                return this;
            }
        }

        class DateTimeFormatterStub : IDateTimeFormatter
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

        [Fact]
        public void FormatCustomer_ShouldUseName_WhenNameAvailable()
        {
            var customer = new Customer
            {
                FirstName = "Bruce",
                LastName = "Wayne"
            };
            var expected = "Bruce Wayne";

            var sut = new OrderReportFactoryBuilder()
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

            var sut = new OrderReportFactoryBuilder()
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

            var sut = new OrderReportFactoryBuilder()
                .WithKenticoResourceService(resources)
                .WithKenticoDocumentProvider(documents)
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

            var sut = new OrderReportFactoryBuilder()
                .WithKenticoOrderProvider(orderProvider)
                .Build();

            var actualStatus = sut.FormatOrderStatus(microserviceStatus);

            Assert.Equal(mappedStatus, actualStatus);
        }

        [Fact]
        public void CreateTableView_ShouldMapLineItems_WhenShipped()
        {
            var dtFormatter = new DateTimeFormatterStub();
            var sut = new OrderReportFactoryBuilder()
                .WithDateTimeFormatter(dtFormatter)
                .Build();
            var testOrder = OrderReportTestHelper.CreateTestOrder(orderNumber: 1, itemsCount: 1);
            var orders = new List<OrderReport> { testOrder };

            var view = sut.CreateTableView(orders);

            var row = view.Rows[0];
            Assert.Equal(testOrder.Url, row.Url);
            Assert.Equal(testOrder.Site, row.Cells[0]);
            Assert.Equal(testOrder.Number, row.Cells[1]);
            Assert.Equal(dtFormatter.Format(testOrder.OrderingDate), row.Cells[2]);
            Assert.Equal(testOrder.User, row.Cells[3]);
            Assert.Equal(testOrder.Items[0].Name, row.Cells[4]);
            Assert.Equal(testOrder.Items[0].SKU, row.Cells[5]);
            Assert.Equal(testOrder.Items[0].Quantity, row.Cells[6]);
            Assert.Equal(testOrder.Items[0].Price, row.Cells[7]);
            Assert.Equal(testOrder.Status, row.Cells[8]);
            Assert.Equal(dtFormatter.Format(testOrder.ShippingDate.Value), row.Cells[9]);
            Assert.Equal(testOrder.TrackingNumber, row.Cells[10]);
        }

        [Fact]
        public void CreateTableView_ShouldMapLineItems_WhenNotShipped()
        {
            var sut = new OrderReportFactoryBuilder()
                .Build();
            var testOrder = OrderReportTestHelper.CreateTestOrder(orderNumber: 1, itemsCount: 1);
            testOrder.ShippingDate = null;
            var orders = new List<OrderReport> { testOrder };

            var view = sut.CreateTableView(orders);

            var row = view.Rows[0];
            Assert.Equal(string.Empty, row.Cells[9]);
        }

        [Fact]
        public void Create_ShouldMap()
        {
            var customer = new Customer
            {
                FirstName = "Bruce",
                LastName = "Wayne"
            };
            var userProvider = Mock.Of<IKenticoUserProvider>(kup =>
                kup.GetCustomer(It.IsAny<int>()) == customer
            );

            var sut = new OrderReportFactoryBuilder()
                .WithKenticoUserProvider(userProvider)
                .Build();
            var orderDto = OrderReportTestHelper.CreateTestRecentOrder(1, 1);

            var orderReport = sut.Create(orderDto);

            // order header
            Assert.Equal(orderDto.Id, orderReport.Number);
            Assert.Equal(orderDto.CreateDate, orderReport.OrderingDate);
            Assert.Equal(orderDto.ShippingDate, orderReport.ShippingDate);
            Assert.Equal(orderDto.SiteName, orderReport.Site);
            Assert.Equal(sut.FormatOrderStatus(orderDto.Status), orderReport.Status);
            Assert.Equal(orderDto.TrackingNumber, orderReport.TrackingNumber);
            Assert.Equal(sut.FormatDetailUrl(orderDto), orderReport.Url);
            Assert.Equal(sut.FormatCustomer(customer), orderReport.User);

            // order items
            var firstItem = orderDto.Items.First();
            Assert.Equal(orderDto.Items.Count(), orderReport.Items.Count);
            Assert.Equal(firstItem.Name, orderReport.Items[0].Name);
            Assert.Equal(firstItem.Quantity, orderReport.Items[0].Quantity);
            Assert.Equal(firstItem.Price, orderReport.Items[0].Price);
            Assert.Equal(firstItem.SKU, orderReport.Items[0].SKU);
        }

    }
}
