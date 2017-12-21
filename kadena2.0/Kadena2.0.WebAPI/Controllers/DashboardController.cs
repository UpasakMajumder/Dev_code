using AutoMapper;
using Kadena.Dto.RecentOrders;
using Kadena.BusinessLogic.Contracts;
using Kadena.WebAPI.Infrastructure;
using System.Threading.Tasks;
using System.Web.Http;
using Kadena.WebAPI.Infrastructure.Filters;
using System;

namespace Kadena.WebAPI.Controllers
{
    [CustomerAuthorizationFilter]
    public class DashboardController : ApiControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IOrderListService _orderService;

        public DashboardController(IOrderListServiceFactory orderListServiceFactory, IMapper mapper)
        {
            if (orderListServiceFactory == null)
            {
                throw new ArgumentNullException(nameof(orderListServiceFactory));
            }
            if (mapper == null)
            {
                throw new ArgumentNullException(nameof(mapper));
            }

            _mapper = mapper;
            _orderService = orderListServiceFactory.GetDashboard();
        }

        [HttpGet]
        public async Task<IHttpActionResult> GetOrderHeaders()
        {
            var orderHead = await _orderService.GetHeaders();
            var result = _mapper.Map<OrderHeadDto>(orderHead);
            return ResponseJson(result);
        }

        [HttpGet]
        [Route("api/dashboard/getorderbody/{pageNumber}")]
        public async Task<IHttpActionResult> GetOrderBody(int pageNumber)
        {
            var orderBody = await _orderService.GetBody(pageNumber);
            var result = _mapper.Map<OrderBodyDto>(orderBody);
            return ResponseJson(result);
        }
    }
}