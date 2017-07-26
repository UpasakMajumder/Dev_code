using Kadena.WebAPI.Contracts;
using System.Web.Http;
using System;
using Kadena.Dto.Checkout;
using AutoMapper;
using Kadena.WebAPI.Infrastructure;
using Kadena.Models.SubmitOrder;
using System.Threading.Tasks;
using Kadena.WebAPI.Infrastructure.Filters;
using Kadena.Dto.Checkout.Requests;
using Kadena.Dto.SubmitOrder.Requests;
using Kadena.Dto.SubmitOrder.Responses;
using Kadena.Dto.ViewOrder.Responses;

namespace Kadena.WebAPI.Controllers
{
    public class ShoppingCartController : ApiControllerBase
    {
        private readonly IShoppingCartService service;
        private readonly IOrderService orderService;
        private readonly IMapper mapper;

        public ShoppingCartController(IShoppingCartService service, IOrderService orderService, IMapper mapper)
        {
            if (service == null)
            {
                throw new ArgumentNullException(nameof(service));
            }

            if (orderService == null)
            {
                throw new ArgumentNullException(nameof(orderService));
            }

            if (mapper == null)
            {
                throw new ArgumentNullException(nameof(mapper));
            }

            this.service = service;
            this.orderService = orderService;
            this.mapper = mapper;
        }

        [HttpGet]
        [Route("api/shoppingcart")]
        [AuthorizationFilter]
        public IHttpActionResult Get()
        {
            var checkoutPage = service.GetCheckoutPage();
            var checkoutPageDto = mapper.Map<CheckoutPageDTO>(checkoutPage);
            return ResponseJson(checkoutPageDto);
        }

        [HttpGet]
        [Route("api/deliverytotals")]
        [AuthorizationFilter]
        public async Task<IHttpActionResult> GetDeliveryTotals()
        {
            var deliveryTotals = await service.GetDeliveryAndTotals();
            var deliveryTotalsDto = mapper.Map<CheckoutPageDeliveryTotalsDTO>(deliveryTotals);
            return ResponseJson(deliveryTotalsDto);
        }

        [HttpGet]
        [Route("api/orderdetail/{orderId}")]
        [AuthorizationFilter]
        public async Task<IHttpActionResult> Get([FromUri]string orderId)
        {
            var detailPage = await orderService.GetOrderDetail(orderId);
            var detailPageDto = mapper.Map<OrderDetailDTO>(detailPage);
            return ResponseJson(detailPageDto); // TODO refactor using checking null
        }

        [HttpPost]
        [Route("api/shoppingcart/selectshipping")]
        [AuthorizationFilter]
        public IHttpActionResult SelectShipping([FromBody]ChangeSelectionRequestDto request)
        {
            var result = service.SelectShipipng(request.Id);
            var resultDto = mapper.Map<CheckoutPageDTO>(result);
            return ResponseJson(resultDto);
        }

        [HttpPost]
        [Route("api/shoppingcart/selectaddress")]
        [AuthorizationFilter]
        public IHttpActionResult SelectAddress([FromBody]ChangeSelectionRequestDto request)
        {
            var result = service.SelectAddress(request.Id);
            var resultDto = mapper.Map<CheckoutPageDTO>(result);
            return ResponseJson(resultDto);
        }

        [HttpPost]
        [Route("api/shoppingcart/removeitem")]
        [AuthorizationFilter]
        public IHttpActionResult RemoveItem([FromBody]RemoveItemRequestDto request)
        {
            var result = service.RemoveItem(request.Id);
            var resultDto = mapper.Map<CheckoutPageDTO>(result);
            return ResponseJson(resultDto);
        }

        [HttpPost]
        [Route("api/shoppingcart/changequantity")]
        [AuthorizationFilter]
        public IHttpActionResult ChangeItemQuantity([FromBody]ChangeItemQuantityRequestDto request)
        {
            var result = service.ChangeItemQuantity(request.Id, request.Quantity);
            var resultDto = mapper.Map<CheckoutPageDTO>(result);
            return ResponseJson(resultDto);
        }


        [HttpPost]
        [Route("api/shoppingcart/submit")]
        [AuthorizationFilter]
        public async Task<IHttpActionResult> Submit([FromBody]SubmitRequestDto request)
        {
            var submitRequest = mapper.Map<SubmitOrderRequest>(request);
            var serviceResponse = await orderService.SubmitOrder(submitRequest);
            var resultDto = Mapper.Map<SubmitOrderResponseDto>(serviceResponse);
            return ResponseJson(resultDto);
        }
    }
}
