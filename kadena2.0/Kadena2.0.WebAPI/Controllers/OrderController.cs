using AutoMapper;
using Kadena.BusinessLogic.Contracts;
using Kadena.BusinessLogic.Contracts.Orders;
using Kadena.Dto.Common;
using Kadena.Dto.ViewOrder.Responses;
using Kadena.Models.Orders;
using Kadena.WebAPI.Infrastructure;
using Kadena.WebAPI.Infrastructure.Filters;
using System;
using System.Threading.Tasks;
using System.Web.Http;

namespace Kadena.WebAPI.Controllers
{
    [CustomerAuthorizationFilter]
    public class OrderController : ApiControllerBase
    {
        private readonly IOrderDetailService orderDetailService;
        private readonly IOrderService orderService;
        private readonly IMapper mapper;

        public OrderController(IOrderDetailService orderDetailService, IOrderService orderService, IMapper mapper)
        {
            if (orderDetailService == null)
            {
                throw new ArgumentNullException(nameof(orderDetailService));
            }
            if (orderService == null)
            {
                throw new ArgumentNullException(nameof(orderService));
            }
            if (mapper == null)
            {
                throw new ArgumentNullException(nameof(mapper));
            }

            this.orderDetailService = orderDetailService;
            this.orderService = orderService;
            this.mapper = mapper;
        }

        [HttpGet]
        [Route(Routes.Order.Detail)]
        [Route(Routes.Order.DetailLegacy)]
        public async Task<IHttpActionResult> Get([FromUri]string orderId)
        {
            var detailPage = await orderDetailService.GetOrderDetail(orderId);
            var detailPageDto = mapper.Map<OrderDetailDTO>(detailPage);
            return ResponseJson(detailPageDto);
        }

        [HttpGet]
        [Route(Routes.Order.Get)]
        public async Task<IHttpActionResult> Get(DateTime? dateFrom = null, DateTime? dateTo = null, string sort = null, int page = 1)
        {
            var orders = await orderService
                .GetOrders(page, new OrderFilter { FromDate = dateFrom, ToDate = dateTo, Sort = sort });
            var ordersTableView = orderService.ConvertOrdersToView(orders);
            var resultDto = mapper.Map<TableViewDto>(ordersTableView);
            return ResponseJson(resultDto);
        }

        [HttpGet]
        [Route(Routes.Order.Export)]
        public async Task<IHttpActionResult> Export(string format, DateTime? dateFrom = null, DateTime? dateTo = null)
        {
            var export = await orderService
                .GetOrdersExport(format, new OrderFilter { FromDate = dateFrom, ToDate = dateTo });
            return File(export);
        }
    }
}