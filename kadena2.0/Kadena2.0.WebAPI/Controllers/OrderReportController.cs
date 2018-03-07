using AutoMapper;
using Kadena.BusinessLogic.Contracts.Orders;
using Kadena.Dto.Common;
using Kadena.Models.Orders;
using Kadena.WebAPI.Infrastructure;
using Kadena.WebAPI.Infrastructure.Filters;
using System;
using System.Threading.Tasks;
using System.Web.Http;

namespace Kadena.WebAPI.Controllers
{
    [CustomerAuthorizationFilter]
    public class OrderReportController : ApiControllerBase
    {
        private readonly IOrderReportService orderService;
        private readonly IMapper mapper;

        public OrderReportController(IOrderReportService orderService, IMapper mapper)
        {
            if (orderService == null)
            {
                throw new ArgumentNullException(nameof(orderService));
            }
            if (mapper == null)
            {
                throw new ArgumentNullException(nameof(mapper));
            }

            this.orderService = orderService;
            this.mapper = mapper;
        }

        [HttpGet]
        [Route(Routes.OrderReport.Get)]
        public async Task<IHttpActionResult> Get(
            DateTime? dateFrom = null, DateTime? dateTo = null, string sort = null, int page = 1)
        {
            var orders = await orderService
                .GetOrders(page, new OrderFilter { FromDate = dateFrom, ToDate = dateTo, Sort = sort });
            var ordersTableView = orderService.ConvertOrdersToView(orders);
            var resultDto = mapper.Map<TableViewDto>(ordersTableView);
            return ResponseJson(resultDto);
        }

        [HttpGet]
        [Route(Routes.OrderReport.Export)]
        public async Task<IHttpActionResult> Export(DateTime? dateFrom = null, DateTime? dateTo = null)
        {
            var export = await orderService
                .GetOrdersExport(new OrderFilter { FromDate = dateFrom, ToDate = dateTo });
            return File(export);
        }
    }
}