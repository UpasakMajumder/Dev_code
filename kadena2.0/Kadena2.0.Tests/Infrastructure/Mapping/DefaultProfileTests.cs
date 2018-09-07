using Kadena.Container.Default;
using Kadena.Dto.Order;
using Kadena.Dto.ViewOrder.MicroserviceResponses;
using Kadena.Models.Common;
using Kadena.Models.Orders;
using Kadena.Models.Shipping;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Kadena.Tests.Infrastructure.Mapping
{
    public class DefaultProfileTests : ProfileTest<MapperDefaultProfile>
    {
        private OrderReportViewItem GetReportItemView() => new OrderReportViewItem
        {
            Name = "name",
            Number = "number",
            OrderingDate = "ordDate",
            Price = 10,
            Quantity = 2,
            ShippingDate = "shippingDate",
            Site = "site",
            SKU = "sku",
            Status = "status",
            TrackingInfos = new[] {
                new TrackingInfo
                {
                    Id ="id",
                    ItemId = "item-id",
                    QuantityShipped = 10,
                    ShippingDate = "time",
                    ShippingMethod = new TrackingInfoShippingMethod
                    {
                        Provider = "provider",
                        ShippingService = "service"
                    }
                }
            },
            Url = "url",
            User = "user"
        };

        [Fact]
        public void OrderReportTableRow()
        {
            var reportView = GetReportItemView();
            var actualResult = Sut.Map<TableRow[]>(new[] { reportView });

            Assert.Single(actualResult);
            var actualRow = actualResult[0];
            Assert.Equal(reportView.Url, actualRow.Url);

            var actualItems = actualRow.Items as OrderReportTableRow;
            Assert.NotNull(actualItems);
            Assert.Equal(reportView.LineNumber, actualItems.lineNumber.Value);
            Assert.Equal(reportView.Site, actualItems.site.Value);
            Assert.Equal(reportView.Number, actualItems.orderNumber.Value);
            Assert.Equal(reportView.OrderingDate, actualItems.createDate.Value);
            Assert.Equal(reportView.User, actualItems.user.Value);
            Assert.Equal(reportView.Name, actualItems.name.Value);
            Assert.Equal(reportView.SKU ?? string.Empty, actualItems.sku.Value);
            Assert.Equal(reportView.Quantity, actualItems.quantity.Value);
            Assert.Equal(reportView.Price, actualItems.price.Value);
            Assert.Equal(reportView.Status, actualItems.status.Value);

            var trackingInfo = reportView.TrackingInfos.First();
            Assert.Equal(trackingInfo.ShippingDate, actualItems.shippingDate.Value);
            Assert.Equal(trackingInfo.Id, actualItems.trackingNumber.Value);
            Assert.Equal(trackingInfo.QuantityShipped, actualItems.shippedQuantity.Value);
            Assert.Equal(trackingInfo.ShippingMethod.ShippingService, actualItems.shippingMethod.Value);
            Assert.Equal(trackingInfo.ItemId, actualItems.trackingInfoId.Value);
        }

        [Fact]
        public void OrderReportTableRow_NoTrackingInfo()
        {
            var reportView = GetReportItemView();
            reportView.TrackingInfos = null;

            var actualResult = Sut.Map<TableRow[]>(new[] { reportView });
            var actualRow = actualResult[0];
            var actualItems = actualRow.Items as OrderReportTableRow;

            Assert.Equal(string.Empty, actualItems.shippingDate.Value);
            Assert.Equal(string.Empty, actualItems.trackingNumber.Value);
            Assert.Equal(0, actualItems.shippedQuantity.Value);
            Assert.Equal(string.Empty, actualItems.shippingMethod.Value);
            Assert.Equal(string.Empty, actualItems.trackingInfoId.Value);
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
                SKUNumber = "somenumber",
                Quantity = 1234,
                UnitPrice = 1.2324m,
                TrackingInfos = new[] {
                    new TrackingInfoDto()
                }
            };

            var actualResult = Sut.Map<OrderReportViewItem>(orderItem);

            Assert.Equal(orderItem.Name, actualResult.Name);
            Assert.Equal(orderItem.SKUNumber, actualResult.SKU);
            Assert.Equal(orderItem.Quantity, actualResult.Quantity);
            Assert.Equal(orderItem.UnitPrice, actualResult.Price);
            Assert.Equal(orderItem.TrackingInfos.Count(), actualResult.TrackingInfos.Count());
        }
    }
}
