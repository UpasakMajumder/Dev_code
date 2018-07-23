using Kadena.Dto.Common;
using Kadena.Dto.ViewOrder.Responses;
using Kadena.Models.OrderHistory;
using System.Linq;

namespace Kadena.BusinessLogic.Factories
{
    public interface IOrderHistoryFactory
    {
        OrderHistoryDto Create(OrderHistory orderHistory);
    }

    public class OrderHistoryFactory : IOrderHistoryFactory
    {
        private const string CellTypeText = "text";
        private const string CellTypeDate = "date";
        private const string CellTypeLink = "link";

        public OrderHistoryDto Create(OrderHistory oh) => new OrderHistoryDto
        {
            Title = oh.Title,
            Message = new TitledMessageDto
            {
                Title = oh.Message.Title,
                Text = oh.Message.Text
            },
            OrderChanges = Map(oh.OrderChanges),
            ItemChanges = Map(oh.ItemChanges)
        };

        private OrderHistoryChangesDto Map(ItemChanges itemChanges) => new OrderHistoryChangesDto
        {
            Title = itemChanges.Title,
            Headers = new[] 
            {
                itemChanges.ColumnHeaderItemDescription,
                itemChanges.ColumnHeaderChangeType,
                itemChanges.ColumnHeaderOldValue,
                itemChanges.ColumnHeaderNewValue,
                itemChanges.ColumnHeaderDate,
                itemChanges.ColumnHeaderUser
            },
            Items = itemChanges.Items
                .Select(Map)
                .ToArray()
        };

        private TableCellDto[] Map(ItemChange ic) =>
            new[]
            {
                new TableCellDto { Text = ic.ItemDescription, Type = CellTypeText },
                new TableCellDto { Text = ic.ChangeType, Type = CellTypeText },
                new TableCellDto { Text = ic.OldValue, Type = CellTypeText },
                new TableCellDto { Text = ic.NewValue, Type = CellTypeText },
                new TableCellDto { Text = ic.Date.ToString(), Type = CellTypeDate },
                new TableCellDto { Text = ic.User, Type = CellTypeText },
            };

        private OrderHistoryChangesDto Map(OrderChanges orderChanges) => new OrderHistoryChangesDto
        {
            Title = orderChanges.Title,
            Headers = new[]
            {
                orderChanges.ColumnHeaderCategory,
                orderChanges.ColumnHeaderOldValue,
                orderChanges.ColumnHeaderNewValue,
                orderChanges.ColumnHeaderDate,
                orderChanges.ColumnHeaderUser
            },
            Items = orderChanges.Items
                .Select(Map)
                .ToArray()
        };

        private TableCellDto[] Map(OrderChange oc) =>
            new[] 
            {
                new TableCellDto { Text = oc.Category, Type = CellTypeText },
                new TableCellDto { Text = oc.OldValue, Type = CellTypeText },
                new TableCellDto { Text = oc.NewValue, Type = CellTypeText },
                new TableCellDto { Text = oc.Date.ToString(), Type = CellTypeDate },
                new TableCellDto { Text = oc.User, Type = CellTypeText },
            };
    }
}
