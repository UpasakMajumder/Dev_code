using AutoMapper;
using Kadena.Dto.General;
using Kadena.Dto.RecentOrders;
using Kadena.WebAPI.Contracts;
using Kadena.WebAPI.Infrastructure;
using Kadena.WebAPI.Infrastructure.Filters;
using Kadena.WebAPI.Models.SubmitOrder;
using System.Threading.Tasks;
using System.Web.Http;

namespace Kadena.WebAPI.Controllers
{
    public class RecentOrdersController : ApiControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IRecentOrderService _orderService;

        public RecentOrdersController(IRecentOrderService orderService, IMapper mapper)
        {
            _mapper = mapper;
            _orderService = orderService;
            _orderService.PageCapacityKey = "KDA_RecentOrdersPageCapacity";
        }

        [HttpGet]
        public async Task<IHttpActionResult> GetHeaders()
        {
            var orderHead = await _orderService.GetHeaders();
            var result = _mapper.Map<OrderHeadDto>(orderHead);
            return ResponseJson(result);
        }

        [HttpGet]
        [Route("api/recentorders/getbody/{pageNumber}")]
        public async Task<IHttpActionResult> GetBody(int pageNumber)
        {
            var orderBody = await _orderService.GetBody(pageNumber);
            var result = _mapper.Map<OrderBodyDto>(orderBody);
            return ResponseJson(result);
        }
    }
}