using Kadena.WebAPI.Contracts;
using System.Web.Http;
using System;
using Kadena.Dto.Checkout;
using AutoMapper;
using Kadena.WebAPI.Infrastructure;
using Kadena.WebAPI.Models.SubmitOrder;
using System.Threading.Tasks;
using Kadena.WebAPI.Infrastructure.Filters;
using Kadena.Dto.Checkout.Requests;
using Kadena.Dto.SubmitOrder.Requests;
using Kadena.Dto.SubmitOrder.Responses;

namespace Kadena.WebAPI.Controllers
{
    public class ShoppingCartController : ApiControllerBase
    {
        private readonly IShoppingCartService service;
        private readonly IMapper mapper;

        public ShoppingCartController(IShoppingCartService service, IMapper mapper)
        {
            if (service == null)
            {
                throw new ArgumentNullException(nameof(service));
            }

            if (mapper == null)
            {
                throw new ArgumentNullException(nameof(mapper));
            }

            this.service = service;
            this.mapper = mapper;
        }

        [HttpGet]
        [Route("api/shoppingcart")]
        [AuthorizationFilter]
        public async Task<IHttpActionResult> Get()
        {
            var checkoutPage = await service.GetCheckoutPage();
            var checkoutPageDto = mapper.Map<CheckoutPageDTO>(checkoutPage);
            return ResponseJson(checkoutPageDto);
        }

        [HttpPost]
        [Route("api/shoppingcart/selectshipping")]
        [AuthorizationFilter]
        public async Task<IHttpActionResult> SelectShipping([FromBody]ChangeSelectionRequestDto request)
        {
            var result = await service.SelectShipipng(request.Id);
            var resultDto = mapper.Map<CheckoutPageDTO>(result);
            return ResponseJson(resultDto);
        }

        [HttpPost]
        [Route("api/shoppingcart/selectaddress")]
        [AuthorizationFilter]
        public async Task<IHttpActionResult> SelectAddress([FromBody]ChangeSelectionRequestDto request)
        {
            var result = await service.SelectAddress(request.Id);
            var resultDto = mapper.Map<CheckoutPageDTO>(result);
            return ResponseJson(resultDto);
        }

        [HttpPost]
        [Route("api/shoppingcart/removeitem")]
        [AuthorizationFilter]
        public async Task<IHttpActionResult> RemoveItem([FromBody]RemoveItemRequestDto request)
        {
            var result = await service.RemoveItem(request.Id);
            var resultDto = mapper.Map<CheckoutPageDTO>(result);
            return ResponseJson(resultDto);
        }

        [HttpPost]
        [Route("api/shoppingcart/changequantity")]
        [AuthorizationFilter]
        public async Task<IHttpActionResult> ChangeItemQuantity([FromBody]ChangeItemQuantityRequestDto request)
        {
            var result = await service.ChangeItemQuantity(request.Id, request.Quantity);
            var resultDto = mapper.Map<CheckoutPageDTO>(result);
            return ResponseJson(resultDto);
        }


        [HttpPost]
        [Route("api/shoppingcart/submit")]
        [AuthorizationFilter]
        public async Task<IHttpActionResult> Submit([FromBody]SubmitRequestDto request)
        {
            var submitRequest = mapper.Map<SubmitOrderRequest>(request);
            var serviceResponse = await service.SubmitOrder(submitRequest);
            var resultDto = Mapper.Map<SubmitOrderResponseDto>(serviceResponse);
            return ResponseJson(resultDto);
        }

        [HttpGet]
        [Route("api/shoppingcart/submittable")]
        [AuthorizationFilter]
        public async Task<IHttpActionResult> Submittable()
        {
            var serviceResponse = await service.IsSubmittable();
            var resultDto = Mapper.Map<SubmitOrderResponseDto>(serviceResponse);
            return ResponseJson(resultDto);
        }
    }
}
