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
        [CustomerAuthorizationFilter]
        public async Task<IHttpActionResult> Get([FromUri]string orderId)
        {
            var detailPage = await orderDetailService.GetOrderDetail(orderId);
            var detailPageDto = mapper.Map<OrderDetailDTO>(detailPage);
            return ResponseJson(detailPageDto);
        }
    }
}