using AutoMapper;
using Kadena.Models.Common;
using Kadena.Models.Orders;
using System.Linq;

namespace Kadena.BusinessLogic.Services.Orders
{
    public class OrderReportTableRowToDtoConverter : ITypeConverter<OrderReportViewItem, TableRow>
    {
        public TableRow Convert(OrderReportViewItem source, TableRow destination, ResolutionContext context)
        {
            var trackingInfo = source.TrackingInfos?.FirstOrDefault();

            var items = new OrderReportTableRow
            {
                lineNumber = new TableCell(source.LineNumber),
                site = new TableCell(source.Site),
                orderNumber = new TableCell(source.Number),
                createDate = new TableCell(source.OrderingDate),
                user = new TableCell(source.User),
                name = new TableCell(source.Name),
                sku = new TableCell(source.SKU ?? string.Empty),
                quantity = new TableCell(source.Quantity),
                price = new TableCell(source.Price),
                status = new TableCell(source.Status),
                shippingDate = new TableCell(trackingInfo?.ShippingDate),
                trackingNumber = new TableCellLink(trackingInfo?.Url, trackingInfo?.Id) { Type = "tracking" },
                shippedQuantity = new TableCell(trackingInfo?.QuantityShipped ?? 0),
                shippingMethod = new TableCell(trackingInfo?.ShippingMethod?.ShippingService),
                trackingInfoId = new TableCell(trackingInfo?.ItemId),
            };

            return new TableRow
            {
                Url = source.Url,
                Items = items
            };
        }
    }
}
