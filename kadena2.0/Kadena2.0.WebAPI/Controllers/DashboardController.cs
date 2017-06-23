using AutoMapper;
using Kadena.Dto.RecentOrders;
using Kadena.WebAPI.Contracts;
using Kadena.WebAPI.Infrastructure;
using System.Threading.Tasks;
using System.Web.Http;

namespace Kadena.WebAPI.Controllers
{
    public class DashboardController : ApiControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IOrderListService _orderService;

        public DashboardController(IOrderListService orderService, IMapper mapper)
        {
            _mapper = mapper;
            _orderService = orderService;
            _orderService.PageCapacityKey = "KDA_DashboardOrdersPageCapacity";
        }

        [HttpGet]
        public async Task<IHttpActionResult> GetOrderHeaders()
        {
            var orderHead = await _orderService.GetHeaders();
            var result = _mapper.Map<OrderHeadDto>(orderHead);
            return ResponseJson(result);
        }

        [HttpGet]
        public async Task<IHttpActionResult> GetOrderBody()
        {
            var orderBody = await _orderService.GetBody(1);
            var result = _mapper.Map<OrderBodyDto>(orderBody);
            return ResponseJson(result);
        }
    }
}