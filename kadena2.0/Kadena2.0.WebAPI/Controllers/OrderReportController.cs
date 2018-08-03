using AutoMapper;
using Kadena.BusinessLogic.Contracts;
using Kadena.BusinessLogic.Contracts.Orders;
using Kadena.Dto.Common;
using Kadena.Helpers.Routes;
using Kadena.Models.ModuleAccess;
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
        private readonly IModuleAccessService moduleAccessService;
        private readonly IMapper mapper;

        public OrderReportController(
            IOrderReportService orderService,
            IModuleAccessService moduleAccessService,
            IMapper mapper)
        {
            this.orderService = orderService ?? throw new ArgumentNullException(nameof(orderService));
            this.moduleAccessService = moduleAccessService ?? throw new ArgumentNullException(nameof(moduleAccessService));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        [Route(Routes.OrderReport.Get)]
        public async Task<IHttpActionResult> Get(
            DateTime? dateFrom = null, DateTime? dateTo = null, string sort = null, int page = 1)
        {
            CheckAccess(KnownPageTypes.OrdersReport);
            var ordersTableView = await orderService.ConvertOrdersToView(page, new OrderFilter { FromDate = dateFrom, ToDate = dateTo, OrderByExpression = sort });
            var resultDto = mapper.Map<TableViewDto>(ordersTableView);
            return ResponseJson(resultDto);
        }

        [HttpGet]
        [Route(Routes.OrderReport.Export)]
        public async Task<IHttpActionResult> Export(DateTime? dateFrom = null, DateTime? dateTo = null)
        {
            CheckAccess(KnownPageTypes.OrdersReport);

            var export = await orderService
                .GetOrdersExport(new OrderFilter { FromDate = dateFrom, ToDate = dateTo });
            return File(export);
        }

        private void CheckAccess(string pageType)
        {
            if (!moduleAccessService.HasUserAccessToPageType(pageType))
            {
                throw new Exception("Unauthorized access");
            }
        }
    }
}