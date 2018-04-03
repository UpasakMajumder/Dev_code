using Kadena.BusinessLogic.Contracts.Orders;
using Kadena.Dto.Order;
using Kadena.Models.Common;
using Kadena.Models.Orders.Failed;
using Kadena2.MicroserviceClients.Contracts;
using Kadena2.WebAPI.KenticoProviders.Contracts;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Kadena.BusinessLogic.Services.Orders
{
    public class OrderResubmissionService : IOrderResubmissionService
    {
        private readonly IOrderResubmitClient orderResubmitClient;
        private readonly IOrderViewClient orderViewClient;
        private readonly IKenticoOrderProvider kenticoOrderProvider;
        public const int FirstPageNumber = 1;

        public const string OrderFailureStatus = "Error sending to Tibco";

        public OrderResubmissionService(
            IOrderResubmitClient orderResubmitClient,
            IOrderViewClient orderViewClient,
            IKenticoOrderProvider kenticoOrderProvider)
        {
            this.orderResubmitClient = orderResubmitClient ?? throw new ArgumentNullException(nameof(orderResubmitClient));
            this.orderViewClient = orderViewClient ?? throw new ArgumentNullException(nameof(orderViewClient));
            this.kenticoOrderProvider = kenticoOrderProvider ?? throw new ArgumentNullException(nameof(kenticoOrderProvider));
        }

        public async Task<PagedData<FailedOrder>> GetFailedOrders(int page, int itemsPerPage)
        {
            if (page < FirstPageNumber)
            {
                throw new ArgumentException("Page number must be >= " + FirstPageNumber, nameof(page));
            }

            if (itemsPerPage <= 0)
            {
                throw new ArgumentException("Items per page must be >= 0", nameof(itemsPerPage));
            }

            var filter = new OrderListFilter
            {
                PageNumber = page - 1,
                ItemsPerPage = itemsPerPage,
                // TODO: status filter
            };
            var dto = await orderViewClient.GetOrders(filter).ConfigureAwait(false);
            if (dto?.Success == true && dto.Payload != null)
            {
                var pagedResult = Map(dto.Payload, page, itemsPerPage);
                return pagedResult;
            }

            return PagedData<FailedOrder>.Empty();
        }

        private PagedData<FailedOrder> Map(OrderListDto payload, int page, int itemsPerPage)
        {
            var pageCount = payload.TotalCount / itemsPerPage;
            if (payload.TotalCount % itemsPerPage > 0)
            {
                pageCount++;
            }

            return new PagedData<FailedOrder>
            {
                Pagination = new Pagination
                {
                    CurrentPage = page,
                    RowsOnPage = itemsPerPage,
                    RowsCount = payload.TotalCount,
                    PagesCount = pageCount
                },
                Data = payload.Orders
                    .Select(Map)
                    .ToList()
            };
        }

        private FailedOrder Map(RecentOrderDto dto)
            => new FailedOrder
            {
                Id = dto.Id,
                OrderDate = dto.CreateDate,
                OrderStatus = kenticoOrderProvider.MapOrderStatus(dto.Status),
                SiteName = dto.SiteName,
                SubmissionAttemptsCount = dto.SubmissionAttemptCount,
                TotalPrice = dto.TotalPrice
            };

        public Task ResubmitOrder(string orderId)
        {
            return orderResubmitClient.Resubmit(orderId);
        }
    }
}
