using Kadena.BusinessLogic.Contracts.Orders;
using Kadena.Dto.Order;
using Kadena.Dto.Order.Failed;
using Kadena.Models.Common;
using Kadena.Models.Orders;
using Kadena.Models.Orders.Failed;
using Kadena.WebAPI.KenticoProviders.Contracts;
using Kadena2.MicroserviceClients.Contracts;
using Kadena2.WebAPI.KenticoProviders.Contracts;
using Newtonsoft.Json;
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
        private readonly IKenticoLogger logger;
        public const int FirstPageNumber = 1;

        public const string OrderFailureStatus = "Error sending to Tibco";

        public OrderResubmissionService(
            IOrderResubmitClient orderResubmitClient,
            IOrderViewClient orderViewClient,
            IKenticoOrderProvider kenticoOrderProvider,
            IKenticoLogger logger)
        {
            this.orderResubmitClient = orderResubmitClient ?? throw new ArgumentNullException(nameof(orderResubmitClient));
            this.orderViewClient = orderViewClient ?? throw new ArgumentNullException(nameof(orderViewClient));
            this.kenticoOrderProvider = kenticoOrderProvider ?? throw new ArgumentNullException(nameof(kenticoOrderProvider));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
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
                PageNumber = page,
                ItemsPerPage = itemsPerPage,
                StatusHistoryContains = (int)OrderStatus.SentToTibcoError
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
                SubmissionAttemptsCount = dto.StatusAmounts[(int)OrderStatus.SentToTibcoError],
                TotalPrice = dto.TotalPrice
            };

        public async Task<OperationResult> ResubmitOrder(string orderId)
        {
            var response = await orderResubmitClient
                .Resubmit(new ResubmitOrderRequestDto { OrderId = orderId })
                .ConfigureAwait(false);
            logger.LogInfo(
                source: nameof(OrderResubmissionService),
                eventCode: nameof(OrderResubmissionService.ResubmitOrder),
                info: JsonConvert.SerializeObject(response));

            return new OperationResult
            {
                Success = response?.Success ?? false,
                ErrorMessage = response?.Error?.Message
            };
        }
    }
}
