using Kadena.BusinessLogic.Contracts;
using Kadena.Models.OrderHistory;
using Kadena.WebAPI.KenticoProviders.Contracts;
using System;
using System.Threading.Tasks;

namespace Kadena.BusinessLogic.Services.Orders
{
    public class OrderHistoryService : IOrderHistoryService
    {
        private readonly IKenticoResourceService resources;

        public OrderHistoryService(IKenticoResourceService resources)
        {
            this.resources = resources ?? throw new ArgumentNullException(nameof(resources));
        }

        public Task<OrderHistory> GetOrderHistory(string orderId)
        {
            return Task.FromResult(CreateSample());
        }

        private OrderHistory CreateSample()
        {
            var history = CreateOrderHistory();

            history.Message.Text = "These pretzels are out of this world!";

            history.OrderChanges.Items = new[] 
            {
                new OrderChange
                {
                    Category = "Shipping",
                    OldValue = "Overnight",
                    NewValue = "Ground",
                    Date = new DateTime(2018, 2, 21, 1, 2, 3),
                    User = "someone@somewhere.club"
                },
                new OrderChange
                {
                    Category = "Payment Method",
                    OldValue = "PO",
                    NewValue = "Credit Card",
                    Date = new DateTime(2018, 3, 14, 4, 5, 6),
                    User = "someone@somewhere.club"
                },
            };

            history.ItemChanges.Items = new[] 
            {
                new ItemChange
                {
                    ItemDescription = "Item Name #1",
                    ChangeType = "Qty Change",
                    OldValue = "20",
                    NewValue = "10",
                    Date = new DateTime(2018, 1, 10, 7, 8, 9),
                    User = "someone@somewhere.club"
                },
                new ItemChange
                {
                    ItemDescription = "Item Name #2",
                    ChangeType = "Template edited",
                    OldValue = "View Proof",
                    NewValue = "View Proof",
                    Date = new DateTime(2018, 2, 11, 10, 11, 12),
                    User = "someone@somewhere.club"
                },
                new ItemChange
                {
                    ItemDescription = "Item Name #3",
                    ChangeType = "Removed",
                    OldValue = "",
                    NewValue = "",
                    Date = new DateTime(2018, 2, 12, 13, 14, 15),
                    User = "someone@somewhere.club"
                },
            };
            return history;
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
                ColumnHeaderUser = resources.GetResourceString("Kadena.Order.History.OrderChanges.User")
            },
            ItemChanges = new ItemChanges
            {
                Title = resources.GetResourceString("Kadena.Order.History.ItemChanges.Title"),
                ColumnHeaderChangeType = resources.GetResourceString("Kadena.Order.History.ItemChanges.ChangeType"),
                ColumnHeaderDate = resources.GetResourceString("Kadena.Order.History.ItemChanges.Date"),
                ColumnHeaderItemDescription = resources.GetResourceString("Kadena.Order.History.ItemChanges.ItemDescription"),
                ColumnHeaderNewValue = resources.GetResourceString("Kadena.Order.History.ItemChanges.NewValue"),
                ColumnHeaderOldValue = resources.GetResourceString("Kadena.Order.History.ItemChanges.OldValue"),
                ColumnHeaderUser = resources.GetResourceString("Kadena.Order.History.ItemChanges.User")
            }
        };
    }
}
