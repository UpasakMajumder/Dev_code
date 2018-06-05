using Kadena.BusinessLogic.Contracts;
using Kadena.BusinessLogic.Factories;
using Kadena.Models.Orders;
using Moq;
using System;
using System.Linq;
using Xunit;

namespace Kadena.Tests.BusinessLogic
{
    public class OrderReportFactoryTests : KadenaUnitTest<OrderReportFactory>
    {
        [Fact]
        public void CreateTableView_ShouldMapReportViewToTableView()
        {
            var report = new[]
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
                };
            var expected = report.First();

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
            Setup<IDateTimeFormatter, string>(dtf => dtf.Format(It.IsAny<DateTime>()), "formatted");
            var orderDto = OrderReportTestHelper.CreateTestRecentOrder(1, 1);

            var actual = Sut.CreateReportView(new[] { orderDto });

            var firstActualItem = actual.First();
            Assert.Equal(orderDto.SiteName, firstActualItem.Site);
            Assert.Equal(orderDto.Id, firstActualItem.Number);
            Assert.Equal(orderDto.Items.ToArray()[0].Name, firstActualItem.Name);
            Assert.Equal(orderDto.Items.ToArray()[0].SKUNumber, firstActualItem.SKU);
            Assert.Equal(orderDto.Items.ToArray()[0].Quantity, firstActualItem.Quantity);
            Assert.Equal(orderDto.Items.ToArray()[0].UnitPrice, firstActualItem.Price);
            Assert.Equal(orderDto.Items.ToArray()[0].TrackingNumber, firstActualItem.TrackingNumber);
        }
    }
}
