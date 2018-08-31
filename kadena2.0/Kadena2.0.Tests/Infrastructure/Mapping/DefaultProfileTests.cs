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
        public static IEnumerable<object[]> GetReportItemViews()
        {
            yield return new object[] {
                new OrderReportViewItem
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
                }
            };
            yield return new object[] {
                new OrderReportViewItem()
            };
        }

        [Theory]
        [MemberData(nameof(GetReportItemViews))]
        public void TableRow(OrderReportViewItem reportView)
        {
            var actualResult = Sut.Map<TableRow[]>(new[] { reportView });

            Assert.Single(actualResult);
            var actualRow = actualResult[0];
            Assert.Equal(reportView.Url, actualRow.Url);
            Assert.Equal(12, actualRow.Items.Length);
            Assert.Equal(reportView.LineNumber, actualRow.Items[0]);
            Assert.Equal(reportView.Site, actualRow.Items[1]);
            Assert.Equal(reportView.Number, actualRow.Items[2]);
            Assert.Equal(reportView.OrderingDate, actualRow.Items[3]);
            Assert.Equal(reportView.User, actualRow.Items[4]);
            Assert.Equal(reportView.Name, actualRow.Items[5]);
            Assert.Equal(reportView.SKU ?? string.Empty, actualRow.Items[6]);
            Assert.Equal(reportView.Quantity, actualRow.Items[7]);
            Assert.Equal(reportView.Price, actualRow.Items[8]);
            Assert.Equal(reportView.Status, actualRow.Items[9]);
            Assert.Equal(reportView.TrackingInfos, actualRow.Items[11]);
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
