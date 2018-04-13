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
        [Fact(DisplayName = "OrderReportFactory.FormatCustomer() | Null customer")]
        public void FormatCustomer_ShouldBeEmpty_WhenCustomerNotFound()
        {
            var expected = string.Empty;

            var actual = Sut.FormatCustomer(null);

            Assert.Equal(expected, actual);
        }

        [Fact(DisplayName = "OrderReportFactory.FormatCustomer() | Non null customer")]
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

        [Fact(DisplayName = "OrderReportFactory.FormatCustomer() | Email only customer")]
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

        [Fact(DisplayName = "OrderReportFactory.FormatDetailUrl()")]
        public void FormatDetailUrl_ShouldGenerateUrl()
        {
            var detailUrlBase = "test.com/product";
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

        [Fact(DisplayName = "OrderReportFactory.FormatOrderStatus()")]
        public void FormatOrderStatus_ShouldMapOrderStatus()
        {
            var microserviceStatus = "some micro status";
            var mappedStatus = "some status";
            Setup<IKenticoOrderProvider, string>(kop => kop.MapOrderStatus(microserviceStatus), mappedStatus);

            var actualStatus = Sut.FormatOrderStatus(microserviceStatus);

            Assert.Equal(mappedStatus, actualStatus);
        }

        [Fact(DisplayName = "OrderReportFactory.CreateTableView() | Formatting regular date")]
        public void CreateTableView_ShouldMapLineItems_WhenShipped()
        {
            string format(DateTime dt) => dt.ToString("yyyyMMdd-HHmmss");
            Setup<IDateTimeFormatter, DateTime, string>(s => s.Format(It.IsAny<DateTime>()), format);
            var expected = OrderReportTestHelper.CreateTestOrder(orderNumber: 1, itemsCount: 1);

            var actualView = Sut.CreateTableView(new List<OrderReport> { expected });

            var actual = actualView.Rows[0];
            Assert.Equal(expected.Url, actual.Url);
            Assert.Equal(expected.Site, actual.Items[0]);
            Assert.Equal(expected.Number, actual.Items[1]);
            Assert.Equal(format(expected.OrderingDate), actual.Items[2]);
            Assert.Equal(expected.User, actual.Items[3]);
            Assert.Equal(expected.Items[0].Name, actual.Items[4]);
            Assert.Equal(expected.Items[0].SKU, actual.Items[5]);
            Assert.Equal(expected.Items[0].Quantity, actual.Items[6]);
            Assert.Equal(expected.Items[0].Price, actual.Items[7]);
            Assert.Equal(expected.Status, actual.Items[8]);
            Assert.Equal(format(expected.ShippingDate.Value), actual.Items[9]);
            Assert.Equal(expected.Items[0].TrackingNumber, actual.Items[10]);
        }

        [Fact(DisplayName = "OrderReportFactory.CreateTableView() | Formatting empty date")]
        public void CreateTableView_ShouldMapLineItems_WhenNotShipped()
        {
            var testOrder = OrderReportTestHelper.CreateTestOrder(orderNumber: 1, itemsCount: 1);
            testOrder.ShippingDate = null;

            var actualView = Sut.CreateTableView(new List<OrderReport> { testOrder });

            var actualRow = actualView.Rows[0];
            Assert.Equal(string.Empty, actualRow.Items[9]);
        }

        [Fact(DisplayName = "OrderReportFactory.Create() | Regular report")]
        public void Create_ShouldMap()
        {
            var customer = new Customer
            {
                FirstName = "Bruce",
                LastName = "Wayne"
            };
            Setup<IKenticoUserProvider, Customer>(kup => kup.GetCustomer(It.IsAny<int>()), customer);

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
