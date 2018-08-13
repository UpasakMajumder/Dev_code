using Kadena.BusinessLogic.Contracts;
using Kadena.Dto.ViewOrder.MicroserviceResponses.History;
using Kadena.Models.OrderHistory;
using Kadena.WebAPI.KenticoProviders.Contracts;
using Kadena2.MicroserviceClients.Contracts;
using Kadena2.WebAPI.KenticoProviders.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kadena.BusinessLogic.Services.Orders
{
    public class OrderHistoryService : IOrderHistoryService
    {
        private readonly IKenticoResourceService resources;
        private readonly IOrderManualUpdateClient orderHistoryClient;
        private readonly IOrderViewClient orderViewClient;
        private readonly IKenticoOrderProvider kenticoOrderProvider;
        private readonly IKenticoUserProvider kenticoUserProvider;

        private enum RecordType
        {
            StatusUpdated = 2,
            DataUpdated = 5,
        }

        public OrderHistoryService(
            IKenticoResourceService resources,
            IOrderManualUpdateClient orderHistoryClient,
            IOrderViewClient orderViewClient,
            IKenticoUserProvider kenticoUserProvider,
            IKenticoOrderProvider kenticoOrderProvider)
        {
            this.resources = resources ?? throw new ArgumentNullException(nameof(resources));
            this.orderHistoryClient = orderHistoryClient ?? throw new ArgumentNullException(nameof(orderHistoryClient));
            this.orderViewClient = orderViewClient ?? throw new ArgumentNullException(nameof(orderViewClient));
            this.kenticoOrderProvider = kenticoOrderProvider ?? throw new ArgumentNullException(nameof(kenticoOrderProvider));
            this.kenticoUserProvider = kenticoUserProvider ?? throw new ArgumentNullException(nameof(kenticoUserProvider));
        }

        public async Task<OrderHistory> GetOrderHistory(string orderId)
        {
            var detailTask = orderViewClient.GetOrderByOrderId(orderId);
            var changesTask = orderHistoryClient.Get(orderId);
            await Task.WhenAll(detailTask, changesTask);

            var detail = detailTask.Result.Payload;
            var changes = changesTask.Result.Payload;

            var history = CreateOrderHistory();
            history.Message.Text = string.Join(", ", detail.Approvals?.Select(a => a.Note) ?? Enumerable.Empty<string>());

            HandleChanges(changes, history);

            return history;
        }

        private void HandleChanges(List<OrderHistoryUpdateDto> changes, OrderHistory history)
        {
            for (int i = 1; i < changes.Count; i++)
            {
                if (StatusChangeHandler(changes[i], changes.Take(i), history)) continue;
                if (ItemQuantityChangeHandler(changes[i], changes.Take(i), history)) continue;
            }
        }

        private bool ItemQuantityChangeHandler(OrderHistoryUpdateDto change, IEnumerable<OrderHistoryUpdateDto> previousChanges, OrderHistory history)
        {
            if (change.RecordTypeId != (int)RecordType.DataUpdated)
            {
                return false;
            }

            var previousItems = previousChanges
                .Where(pc => pc.Items?.Length > 0)
                .Last()
                .Items;

            var itemChanges = new List<ItemChange>();
            foreach (var item in change.Items)
            {
                var previousItem = previousItems.First(p => p.LineNumber == item.LineNumber);
                if (previousItem.Quantity != item.Quantity)
                {
                    var wasRemoved = item.Quantity == 0;
                    itemChanges.Add(new ItemChange
                    {
                        ItemDescription = item.ItemName,
                        ChangeType = wasRemoved
                            ? resources.GetResourceString("Kadena.Order.History.OrderChanges.Category.ItemRemoved")
                            : resources.GetResourceString("Kadena.Order.History.OrderChanges.Category.QuantityChange"),
                        Date = new DateTime(change.ServiceRecordDateTimeCreated),
                        User = GetChangedByUserInfo(change.UserId),
                        OldValue = wasRemoved ? "" : previousItem.Quantity.ToString(),
                        NewValue = wasRemoved ? "" : item.Quantity.ToString(),
                    });
                }
            }

            history.ItemChanges.Items.AddRange(itemChanges);

            return itemChanges.Count > 0;
        }

        private bool StatusChangeHandler(OrderHistoryUpdateDto change, IEnumerable<OrderHistoryUpdateDto> previousChanges, OrderHistory history)
        {
            if (change.RecordTypeId != (int)RecordType.StatusUpdated)
            {
                return false;
            }

            var oldStatus = previousChanges.Last().Status;
            var newStatus = change.Status;

            history.OrderChanges.Items.Add(new OrderChange
            {
                OldValue = kenticoOrderProvider.MapOrderStatus(oldStatus),
                NewValue = kenticoOrderProvider.MapOrderStatus(newStatus),
                Category = resources.GetResourceString("Kadena.Order.History.OrderChanges.Category.Status"),
                Date = new DateTime(change.ServiceRecordDateTimeCreated),
                User = GetChangedByUserInfo(change.UserId)
            });

            return true;
        }

        private string GetChangedByUserInfo(string changeUserId)
        {
            if (!int.TryParse(changeUserId, out var userId))
            {
                return string.Empty;
            }

            return kenticoUserProvider.GetUserByUserId(userId)?.Email ?? string.Empty;
        }

        private OrderHistory CreateOrderHistory() => new OrderHistory
        {
            Title = resources.GetResourceString("Kadena.Order.History.Title"),
            Message = new Models.Common.TitledMessage
            {
                Title = resources.GetResourceString("Kadena.Order.History.Message.Title")
            },
            OrderChanges = new OrderChanges
            {
                Title = resources.GetResourceString("Kadena.Order.History.OrderChanges.Title"),
                ColumnHeaderCategory = resources.GetResourceString("Kadena.Order.History.OrderChanges.Category"),
                ColumnHeaderDate = resources.GetResourceString("Kadena.Order.History.OrderChanges.Date"),
                ColumnHeaderNewValue = resources.GetResourceString("Kadena.Order.History.OrderChanges.NewValue"),
                ColumnHeaderOldValue = resources.GetResourceString("Kadena.Order.History.OrderChanges.OldValue"),
                ColumnHeaderUser = resources.GetResourceString("Kadena.Order.History.OrderChanges.User"),
                Items = new List<OrderChange>()
            },
            ItemChanges = new ItemChanges
            {
                Title = resources.GetResourceString("Kadena.Order.History.ItemChanges.Title"),
                ColumnHeaderChangeType = resources.GetResourceString("Kadena.Order.History.ItemChanges.ChangeType"),
                ColumnHeaderDate = resources.GetResourceString("Kadena.Order.History.ItemChanges.Date"),
                ColumnHeaderItemDescription = resources.GetResourceString("Kadena.Order.History.ItemChanges.ItemDescription"),
                ColumnHeaderNewValue = resources.GetResourceString("Kadena.Order.History.ItemChanges.NewValue"),
                ColumnHeaderOldValue = resources.GetResourceString("Kadena.Order.History.ItemChanges.OldValue"),
                ColumnHeaderUser = resources.GetResourceString("Kadena.Order.History.ItemChanges.User"),
                Items = new List<ItemChange>()
            }
        };
    }
}
