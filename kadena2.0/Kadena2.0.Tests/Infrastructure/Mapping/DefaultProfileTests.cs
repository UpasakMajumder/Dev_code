using Kadena.Container.Default;
using Kadena.Dto.Order;
using Kadena.Models.Common;
using Kadena.Models.Orders;
using System;
using Xunit;

namespace Kadena.Tests.Infrastructure.Mapping
{
    public class DefaultProfileTests : ProfileTest<MapperDefaultProfile>
    {
        [Fact]
        public void TableRow()
        {
            var expectedRow = new OrderReportViewItem
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
            };

            var actualResult = Sut.Map<TableRow[]>(new[] { expectedRow });

            Assert.Single(actualResult);
            var actualRow = actualResult[0];
            Assert.Equal(expectedRow.Url, actualRow.Url);
            Assert.Equal(11, actualRow.Items.Length);
            Assert.Equal(expectedRow.Site, actualRow.Items[0]);
            Assert.Equal(expectedRow.Number, actualRow.Items[1]);
            Assert.Equal(expectedRow.OrderingDate, actualRow.Items[2]);
            Assert.Equal(expectedRow.User, actualRow.Items[3]);
            Assert.Equal(expectedRow.Name, actualRow.Items[4]);
            Assert.Equal(expectedRow.SKU, actualRow.Items[5]);
            Assert.Equal(expectedRow.Quantity, actualRow.Items[6]);
            Assert.Equal(expectedRow.Price, actualRow.Items[7]);
            Assert.Equal(expectedRow.Status, actualRow.Items[8]);
            Assert.Equal(expectedRow.ShippingDate, actualRow.Items[9]);
            Assert.Equal(expectedRow.TrackingNumber, actualRow.Items[10]);
        }

        [Fact]
        public void OrderReportView_FromRecentOrderDto()
        {
            var order = new RecentOrderDto
            {
                Id = "someId",
                Status = "someStatus",
            };

            var actualResult = Sut.Map<OrderReportViewItem>(order);

            Assert.Equal(order.Id, actualResult.Number);
            Assert.Equal(order.SiteName, actualResult.Site);
        }

        [Fact]
        public void OrderReportView_FromOrderItemDto()
        {
            var orderItem = new OrderItemDto
            {
                Name = "somename",
                SKUNumber= "somenumber",
                Quantity = 1234,
                UnitPrice = 1.2324m,
                TrackingNumber = "somenumber"
            };

            var actualResult = Sut.Map<OrderReportViewItem>(orderItem);

            Assert.Equal(orderItem.Name, actualResult.Name);
            Assert.Equal(orderItem.SKUNumber, actualResult.SKU);
            Assert.Equal(orderItem.Quantity, actualResult.Quantity);
            Assert.Equal(orderItem.UnitPrice, actualResult.Price);
            Assert.Equal(orderItem.TrackingNumber, actualResult.TrackingNumber);
        }
    }
}
