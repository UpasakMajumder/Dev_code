using Kadena.BusinessLogic.Contracts;
using System.Web.Http;
using System;
using Kadena.Dto.Checkout;
using AutoMapper;
using Kadena.WebAPI.Infrastructure;
using System.Threading.Tasks;
using Kadena.WebAPI.Infrastructure.Filters;
using Kadena.Dto.Checkout.Requests;
using Kadena.Models.Checkout;
using Kadena.Models;
using Kadena.Dto.CustomerData;
using Kadena.Models.CustomerData;
using Kadena.Dto.Checkout.Responses;
using Kadena.Dto.Settings;
using System.ComponentModel.DataAnnotations;
using Kadena.Dto.SubmitOrder.Requests;
using Kadena.Dto.SubmitOrder.Responses;
using Kadena.Models.SubmitOrder;

namespace Kadena.WebAPI.Controllers
{
    public class ShoppingCartController : ApiControllerBase
    {
        private readonly IShoppingCartService shoppingCartService;
        private readonly ISubmitOrderService submitOrderService;
        private readonly IMapper mapper;

        public ShoppingCartController(
            IShoppingCartService service,
            ISubmitOrderService submitOrderService,
            IMapper mapper)
        {
            this.shoppingCartService = service ?? throw new ArgumentNullException(nameof(service));
            this.submitOrderService = submitOrderService ?? throw new ArgumentNullException(nameof(submitOrderService));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        [Route("api/shoppingcart")]
        [CustomerAuthorizationFilter]
        public async Task<IHttpActionResult> Get()
        {
            var checkoutPage = await shoppingCartService.GetCheckoutPage();
            var checkoutPageDto = mapper.Map<CheckoutPageDTO>(checkoutPage);
            return ResponseJson(checkoutPageDto);
        }

        [HttpGet]
        [Route("api/deliverytotals")]
        [CustomerAuthorizationFilter]
        public async Task<IHttpActionResult> GetDeliveryTotals()
        {
            var deliveryTotals = await shoppingCartService.GetDeliveryAndTotals();
            var deliveryTotalsDto = mapper.Map<CheckoutPageDeliveryTotalsDTO>(deliveryTotals);
            return ResponseJson(deliveryTotalsDto);
        }

        [HttpPost]
        [Route("api/shoppingcart/savetemporaryaddress")]
        [CustomerAuthorizationFilter]
        public IHttpActionResult SaveTemporaryAddress([FromBody] AddressDto postedAddress)
        {
            var address = mapper.Map<DeliveryAddress>(postedAddress);
            var addressId = shoppingCartService.SaveTemporaryAddress(address);
            return ResponseJson(new IdDto { Id = addressId });
        }

        [HttpPost]
        [Route("api/shoppingcart/selectshipping")]
        [CustomerAuthorizationFilter]
        public async Task<IHttpActionResult> SelectShipping([FromBody]ChangeSelectionRequestDto request)
        {
            var deliveryTotals = await shoppingCartService.SelectShipping(request.Id);
            var deliveryTotalsDto = mapper.Map<CheckoutPageDeliveryTotalsDTO>(deliveryTotals);
            return ResponseJson(deliveryTotals);
        }

        [HttpPost]
        [Route("api/shoppingcart/selectaddress")]
        [CustomerAuthorizationFilter]
        public IHttpActionResult SelectAddress([FromBody]ChangeSelectionRequestDto request)
        {
            var result = shoppingCartService.SelectAddress(request.Id);
            var resultDto = mapper.Map<ChangeDeliveryAddressResponseDto>(result);
            return ResponseJson(resultDto);
        }

        [HttpPost]
        [Route("api/shoppingcart/removeitem")]
        [CustomerAuthorizationFilter]
        public IHttpActionResult RemoveItem([FromBody]RemoveItemRequestDto request)
        {
            var result = shoppingCartService.RemoveItem(request.Id);
            var resultDto = mapper.Map<ChangeItemQuantityResponseDto>(result);
            return ResponseJson(resultDto);
        }

        [HttpPost]
        [Route("api/shoppingcart/changequantity")]
        [CustomerAuthorizationFilter]
        public IHttpActionResult ChangeItemQuantity([FromBody]ChangeItemQuantityRequestDto request)
        {
            var result = shoppingCartService.ChangeItemQuantity(request.Id, request.Quantity);
            var resultDto = mapper.Map<ChangeItemQuantityResponseDto>(result);
            return ResponseJson(resultDto);
        }

        [HttpGet]
        [Route("api/shoppingcart/itemspreview")]
        [CustomerAuthorizationFilter]
        public IHttpActionResult ItemsPreview()
        {
            var result = shoppingCartService.ItemsPreview();
            var resultDto = mapper.Map<CartItemsPreviewDTO>(result);
            return ResponseJson(resultDto);
        }

        [HttpPost]
        [Route("api/shoppingcart/addtocart")]
        [CustomerAuthorizationFilter]
        [RecalculateShoppingCart]
        public async Task<IHttpActionResult> AddToCart([FromBody][Required] NewCartItemDto item)
        {
            var addItem = mapper.Map<NewCartItem>(item);
            var result = await shoppingCartService.AddToCart(addItem);
            var resultDto = mapper.Map<AddToCartResultDto>(result);
            return ResponseJson(resultDto);
        }        

        [HttpPost]
        [Route("api/shoppingcart/submit")]
        [CustomerAuthorizationFilter]
        public async Task<IHttpActionResult> Submit([FromBody]SubmitRequestDto request)
        {
            var submitRequest = mapper.Map<SubmitOrderRequest>(request);
            var serviceResponse = await submitOrderService.SubmitOrder(submitRequest);
            var resultDto = mapper.Map<SubmitOrderResponseDto>(serviceResponse);
            return ResponseJson(resultDto);
        }
    }
}
