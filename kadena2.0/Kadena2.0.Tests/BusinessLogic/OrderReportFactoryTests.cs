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

namespace Kadena.Tests.BusinessLogic
{
    public class OrderReportFactoryTests : KadenaUnitTest<OrderReportFactory>
    {
        [Fact]
        public void FormatCustomer_ShouldBeEmpty_WhenCustomerNotFound()
        {
            var expected = string.Empty;

            var actual = Sut.FormatCustomer(null);

            Assert.Equal(expected, actual);
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

            var actual = Sut.FormatCustomer(customer);

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

            var actual = Sut.FormatCustomer(customer);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void FormatDetailUrl_ShouldGenerateUrl()
        {
            const string detailUrlBase = "test.com/product";
            Setup<IKenticoResourceService, string>(res => res.GetSiteSettingsKey(Settings.KDA_OrderDetailUrl), detailUrlBase);
            Setup<IKenticoDocumentProvider, string>(doc => doc.GetDocumentUrl(It.IsAny<string>(), It.IsAny<bool>()), detailUrlBase);
            var order = new RecentOrderDto
            {
                Id = "1234"
            };
            var expectedUrl = $"{detailUrlBase}?orderID={order.Id}";

            var actualUrl = Sut.FormatDetailUrl(order);

            Assert.Equal(expectedUrl, actualUrl);
        }

        [Fact]
        public void FormatOrderStatus_ShouldMapOrderStatus()
        {
            var microserviceStatus = "some micro status";
            var mappedStatus = "some status";
            Setup<IKenticoOrderProvider, string>(kop => kop.MapOrderStatus(microserviceStatus), mappedStatus);

            var actualStatus = Sut.FormatOrderStatus(microserviceStatus);

            Assert.Equal(mappedStatus, actualStatus);
        }

        public static IEnumerable<object[]> GetTestDataFor_FormatDate_ShouldUseFormatter =>
            new[] 
            {
                new object[] { new DateTime(), "formatted" },
                new object[] { null, string.Empty }
            };

        [Theory]
        [MemberData(nameof(GetTestDataFor_FormatDate_ShouldUseFormatter))]
        public void FormatDate_ShouldUseFormatter(DateTime? date, string formatted)
        {
            Setup<IDateTimeFormatter, string>(dtf => dtf.Format(It.IsAny<DateTime>()), formatted);

            var actual = Sut.FormatDate(date);

            Assert.Equal(formatted, actual);
        }

        [Fact]
        public void CreateTableView_ShouldMapReportViewToTableView()
        {
            var report = new OrderReportView
            {
                Items = new[]
                {
                    new OrderReportViewItem
                    {
                        Name = "name",
                        Number = "number",
                        OrderingDate = "ordDate",
                        Price = 10,
                        Quantity = 2,
                        ShippingDate = "shiDate",
                        Site = "site",
                        SKU = "sku",
                        Status = "status",
                        TrackingNumber = "track",
                        Url = "url",
                        User = "user"
                    }
                }
            };
            var expected = report.Items.First();

            var actualView = Sut.CreateTableView(report);
            var actual = actualView.Rows[0];

            Assert.Equal(expected.Url, actual.Url);
            Assert.Equal(expected.Site, actual.Items[0]);
            Assert.Equal(expected.Number, actual.Items[1]);
            Assert.Equal(expected.OrderingDate, actual.Items[2]);
            Assert.Equal(expected.User, actual.Items[3]);
            Assert.Equal(expected.Name, actual.Items[4]);
            Assert.Equal(expected.SKU, actual.Items[5]);
            Assert.Equal(expected.Quantity, actual.Items[6]);
            Assert.Equal(expected.Price, actual.Items[7]);
            Assert.Equal(expected.Status, actual.Items[8]);
            Assert.Equal(expected.ShippingDate, actual.Items[9]);
            Assert.Equal(expected.TrackingNumber, actual.Items[10]);
        }

        [Fact]
        public void CreateReportView_ShouldMapReportToReportView()
        {
            var report = new OrderReport
            {
                User = "user",
                Url = "url",
                Status = "status",
                Site = "site",
                Number = "number",
                Items = new List<ReportLineItem>
                {
                    new ReportLineItem
                    {
                        Name = "name", Price = 10, Quantity = 2, SKU = "sku", TrackingNumber = "tracking"
                    }
                },
                OrderingDate = new DateTime(),
                ShippingDate = new DateTime()
            };

            Setup<IDateTimeFormatter, string>(dtf => dtf.Format(It.IsAny<DateTime>()), "formatted");

            var actual = Sut.CreateReportView(new[] { report });

            var firstActualItem = actual.Items.First();
            Assert.Equal(report.Url, firstActualItem.Url);
            Assert.Equal(report.Site, firstActualItem.Site);
            Assert.Equal(report.Number, firstActualItem.Number);
            Assert.Equal("formatted", firstActualItem.OrderingDate);
            Assert.Equal(report.User, firstActualItem.User);
            Assert.Equal(report.Items[0].Name, firstActualItem.Name);
            Assert.Equal(report.Items[0].SKU, firstActualItem.SKU);
            Assert.Equal(report.Items[0].Quantity, firstActualItem.Quantity);
            Assert.Equal(report.Items[0].Price, firstActualItem.Price);
            Assert.Equal(report.Status, firstActualItem.Status);
            Assert.Equal("formatted", firstActualItem.ShippingDate);
            Assert.Equal(report.Items[0].TrackingNumber, firstActualItem.TrackingNumber);
        }

        [Fact]
        public void Create_ShouldCreateReportFromOrder()
        {
            var customer = new Customer
            {
                FirstName = "Bruce",
                LastName = "Wayne"
            };
            Setup<IKenticoCustomerProvider, Customer>(kcp => kcp.GetCustomer(It.IsAny<int>()), customer);

            var orderDto = OrderReportTestHelper.CreateTestRecentOrder(1, 1);
            var sut = Sut;

            var orderReport = sut.Create(orderDto);

            // order header
            Assert.Equal(orderDto.Id, orderReport.Number);
            Assert.Equal(orderDto.CreateDate, orderReport.OrderingDate);
            Assert.Equal(orderDto.ShippingDate, orderReport.ShippingDate);
            Assert.Equal(orderDto.SiteName, orderReport.Site);
            Assert.Equal(sut.FormatOrderStatus(orderDto.Status), orderReport.Status);
            Assert.Equal(sut.FormatDetailUrl(orderDto), orderReport.Url);
            Assert.Equal(sut.FormatCustomer(customer), orderReport.User);

            // order items
            var firstItem = orderDto.Items.First();
            Assert.Equal(orderDto.Items.Count(), orderReport.Items.Count);
            Assert.Equal(firstItem.Name, orderReport.Items[0].Name);
            Assert.Equal(firstItem.Quantity, orderReport.Items[0].Quantity);
            Assert.Equal(firstItem.UnitPrice, orderReport.Items[0].Price);
            Assert.Equal(firstItem.SKUNumber, orderReport.Items[0].SKU);
            Assert.Equal(firstItem.TrackingNumber, orderReport.Items[0].TrackingNumber);
        }
    }
}
