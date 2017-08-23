using Kadena.WebAPI.Contracts;
using System.Web.Http;
using System;
using Kadena.Dto.Checkout;
using AutoMapper;
using Kadena.WebAPI.Infrastructure;
using System.Threading.Tasks;
using Kadena.WebAPI.Infrastructure.Filters;
using Kadena.Dto.Checkout.Requests;
using Kadena.Models.Checkout;
using Kadena.Dto.Product;

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

        [HttpGet]
        [Route("api/shoppingcart/itemspreview")]
        [AuthorizationFilter]
        public IHttpActionResult ItemsPreview()
        {
            var result = service.ItemsPreview();
            var resultDto = mapper.Map<CartItemsPreviewDTO>(result);
            return ResponseJson(resultDto);
        }

        [HttpPost]
        [Route("api/shoppingcart/addtocart")]
        [AuthorizationFilter]
        public async Task<IHttpActionResult> AddToCart([FromBody] NewCartItemDto item)
        {
            var addItem = mapper.Map<NewCartItem>(item);
            var result = await service.AddToCart(addItem);
            var resultDto = mapper.Map<AddToCartResultDto>(result);
            return ResponseJson(resultDto);
        }
    }
}
