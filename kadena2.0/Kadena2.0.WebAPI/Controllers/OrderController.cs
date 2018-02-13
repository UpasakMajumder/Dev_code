using AutoMapper;
using Kadena.BusinessLogic.Contracts;
using Kadena.Dto.ViewOrder.Responses;
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
        private readonly IMapper mapper;

        public OrderController(IOrderDetailService orderDetailService, IMapper mapper)
        {
            if (orderDetailService == null)
            {
                throw new ArgumentNullException(nameof(orderDetailService));
            }

            this.orderDetailService = orderDetailService;
            this.mapper = mapper;
        }

        [HttpGet]
        [Route("api/orderdetail/{orderId}")]
        [Route("api/order/{orderId}")]
        public async Task<IHttpActionResult> Get([FromUri]string orderId)
        {
            var detailPage = await orderDetailService.GetOrderDetail(orderId);
            var detailPageDto = mapper.Map<OrderDetailDTO>(detailPage);
            return ResponseJson(detailPageDto);
        }

        [HttpGet]
        [Route("api/order")]
        public IHttpActionResult Get(string dateFrom, string dateTo, string sort, int page = 1)
        {
            // TODO
            return ResponseJson("TODO");
        }

        [HttpGet]
        [Route("api/order/export")]
        public IHttpActionResult Export(string dateFrom, string dateTo)
        {
            // TODO
            return ResponseJson("TODO");
        }
    }
}