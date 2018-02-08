using AutoMapper;
using Kadena.BusinessLogic.Contracts;
using Kadena.Dto.Checkout;
using Kadena.Dto.Checkout.Requests;
using Kadena.Dto.Product;
using Kadena.Dto.SubmitOrder.Requests;
using Kadena.Dto.SubmitOrder.Responses;
using Kadena.Models;
using Kadena.Models.Checkout;
using Kadena.Models.SubmitOrder;
using Kadena.WebAPI.Infrastructure;
using Kadena.WebAPI.Infrastructure.Filters;
using System;
using System.Threading.Tasks;
using System.Web.Http;

namespace Kadena.WebAPI.Controllers
{
    public class ShoppingCartController : ApiControllerBase
    {
        private readonly IShoppingCartService cartService;
        private readonly ISubmitOrderService orderSubmitService;
        private readonly IMapper mapper;

        public ShoppingCartController(IShoppingCartService cartService, ISubmitOrderService orderSubmitService, IMapper mapper)
        {
            if (cartService == null)
            {
                throw new ArgumentNullException(nameof(cartService));
            }

            if (orderSubmitService == null)
            {
                throw new ArgumentNullException(nameof(orderSubmitService));
            }

            if (mapper == null)
            {
                throw new ArgumentNullException(nameof(mapper));
            }

            this.cartService = cartService;
            this.orderSubmitService = orderSubmitService;
            this.mapper = mapper;
        }

        [HttpGet]
        [Route("api/shoppingcart")]
        [CustomerAuthorizationFilter]
        public IHttpActionResult Get()
        {
            var checkoutPage = cartService.GetCheckoutPage();
            var checkoutPageDto = mapper.Map<CheckoutPageDTO>(checkoutPage);
            return ResponseJson(checkoutPageDto);
        }
        
        [HttpGet]
        [Route("api/deliverytotals")]
        [CustomerAuthorizationFilter]
        public async Task<IHttpActionResult> GetDeliveryTotals()
        {
            var deliveryTotals = await cartService.GetDeliveryAndTotals();
            var deliveryTotalsDto = mapper.Map<CheckoutPageDeliveryTotalsDTO>(deliveryTotals);
            return ResponseJson(deliveryTotalsDto);
        }

        [HttpPost]
        [Route("api/deliverytotals")]
        [CustomerAuthorizationFilter]
        public async Task<IHttpActionResult> SetDeliveryAddress([FromBody] DeliveryAddressDTO postedAddress)
        {
            var address = mapper.Map<DeliveryAddress>(postedAddress);
            var deliveryTotals = await cartService.SetDeliveryAddress(address);
            var deliveryTotalsDto = mapper.Map<CheckoutPageDeliveryTotalsDTO>(deliveryTotals);
            return ResponseJson(deliveryTotalsDto);
        }

        [HttpPost]
        [Route("api/shoppingcart/selectshipping")]
        [CustomerAuthorizationFilter]
        public IHttpActionResult SelectShipping([FromBody]ChangeSelectionRequestDto request)
        {
            var result = cartService.SelectShipipng(request.Id);
            var resultDto = mapper.Map<CheckoutPageDTO>(result);
            return ResponseJson(resultDto);
        }

        [HttpPost]
        [Route("api/shoppingcart/selectaddress")]
        [CustomerAuthorizationFilter]
        public IHttpActionResult SelectAddress([FromBody]ChangeSelectionRequestDto request)
        {
            var result = cartService.SelectAddress(request.Id);
            var resultDto = mapper.Map<CheckoutPageDTO>(result);
            return ResponseJson(resultDto);
        }

        [HttpPost]
        [Route("api/shoppingcart/removeitem")]
        [CustomerAuthorizationFilter]
        public IHttpActionResult RemoveItem([FromBody]RemoveItemRequestDto request)
        {
            var result = cartService.RemoveItem(request.Id);
            var resultDto = mapper.Map<CheckoutPageDTO>(result);
            return ResponseJson(resultDto);
        }

        [HttpPost]
        [Route("api/shoppingcart/changequantity")]
        [CustomerAuthorizationFilter]
        public IHttpActionResult ChangeItemQuantity([FromBody]ChangeItemQuantityRequestDto request)
        {
            var result = cartService.ChangeItemQuantity(request.Id, request.Quantity);
            var resultDto = mapper.Map<CheckoutPageDTO>(result);
            return ResponseJson(resultDto);
        }

        [HttpGet]
        [Route("api/shoppingcart/itemspreview")]
        [CustomerAuthorizationFilter]
        public IHttpActionResult ItemsPreview()
        {
            var result = cartService.ItemsPreview();
            var resultDto = mapper.Map<CartItemsPreviewDTO>(result);
            return ResponseJson(resultDto);
        }

        [HttpPost]
        [Route("api/shoppingcart/addtocart")]
        [CustomerAuthorizationFilter]
        [RecalculateShoppingCart]
        public async Task<IHttpActionResult> AddToCart([FromBody] NewCartItemDto item)
        {
            var addItem = mapper.Map<NewCartItem>(item);
            var result = await cartService.AddToCart(addItem);
            var resultDto = mapper.Map<AddToCartResultDto>(result);
            return ResponseJson(resultDto);
        }

        [HttpPost]
        [Route("api/shoppingcart/submit")]
        [CustomerAuthorizationFilter]
        public async Task<IHttpActionResult> Submit([FromBody]SubmitRequestDto request)
        {
            var submitRequest = mapper.Map<SubmitOrderRequest>(request);
            var serviceResponse = await orderSubmitService.SubmitOrder(submitRequest);
            var resultDto = mapper.Map<SubmitOrderResponseDto>(serviceResponse);
            return ResponseJson(resultDto);
        }
    }
}
