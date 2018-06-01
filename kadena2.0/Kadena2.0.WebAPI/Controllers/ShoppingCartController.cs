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
using Kadena.WebAPI.KenticoProviders.Contracts;
using Kadena.Models.CustomerData;
using Kadena.Dto.Checkout.Responses;
using Kadena.Dto.Settings;
using Kadena.Dto.AddToCart;
using Kadena.Models.AddToCart;
using System.ComponentModel.DataAnnotations;
using Kadena.Dto.SubmitOrder.Requests;
using Kadena.Dto.SubmitOrder.Responses;
using Kadena.Models.SubmitOrder;

namespace Kadena.WebAPI.Controllers
{
    public class ShoppingCartController : ApiControllerBase
    {
        private readonly IShoppingCartService service;
        private readonly IMapper mapper;
        private readonly IShoppingCartProvider provider;
        private readonly ISubmitOrderService submitOrderService;

        public ShoppingCartController(
            IShoppingCartService service,
            IMapper mapper,
            IShoppingCartProvider provider,
            ISubmitOrderService submitOrderService)
        {
            this.service = service ?? throw new ArgumentNullException(nameof(service));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this.provider = provider ?? throw new ArgumentNullException(nameof(provider));
            this.submitOrderService = submitOrderService ?? throw new ArgumentNullException(nameof(submitOrderService));
        }

        [HttpGet]
        [Route("api/shoppingcart")]
        [CustomerAuthorizationFilter]
        public async Task<IHttpActionResult> Get()
        {
            var checkoutPage = await service.GetCheckoutPage();
            var checkoutPageDto = mapper.Map<CheckoutPageDTO>(checkoutPage);
            return ResponseJson(checkoutPageDto);
        }

        [HttpGet]
        [Route("api/deliverytotals")]
        [CustomerAuthorizationFilter]
        public async Task<IHttpActionResult> GetDeliveryTotals()
        {
            var deliveryTotals = await service.GetDeliveryAndTotals();
            var deliveryTotalsDto = mapper.Map<CheckoutPageDeliveryTotalsDTO>(deliveryTotals);
            return ResponseJson(deliveryTotalsDto);
        }

        [HttpPost]
        [Route("api/shoppingcart/savetemporaryaddress")]
        [CustomerAuthorizationFilter]
        public IHttpActionResult SaveTemporaryAddress([FromBody] AddressDto postedAddress)
        {
            var address = mapper.Map<DeliveryAddress>(postedAddress);
            var addressId = service.SaveTemporaryAddress(address);
            return ResponseJson(new IdDto { Id = addressId });
        }

        [HttpPost]
        [Route("api/shoppingcart/selectshipping")]
        [CustomerAuthorizationFilter]
        public async Task<IHttpActionResult> SelectShipping([FromBody]ChangeSelectionRequestDto request)
        {
            var deliveryTotals = await service.SelectShipping(request.Id);
            var deliveryTotalsDto = mapper.Map<CheckoutPageDeliveryTotalsDTO>(deliveryTotals);
            return ResponseJson(deliveryTotals);
        }

        [HttpPost]
        [Route("api/shoppingcart/selectaddress")]
        [CustomerAuthorizationFilter]
        public IHttpActionResult SelectAddress([FromBody]ChangeSelectionRequestDto request)
        {
            var result = service.SelectAddress(request.Id);
            var resultDto = mapper.Map<ChangeDeliveryAddressResponseDto>(result);
            return ResponseJson(resultDto);
        }

        [HttpPost]
        [Route("api/shoppingcart/removeitem")]
        [CustomerAuthorizationFilter]
        public IHttpActionResult RemoveItem([FromBody]RemoveItemRequestDto request)
        {
            var result = service.RemoveItem(request.Id);
            var resultDto = mapper.Map<ChangeItemQuantityResponseDto>(result);
            return ResponseJson(resultDto);
        }

        [HttpPost]
        [Route("api/shoppingcart/changequantity")]
        [CustomerAuthorizationFilter]
        public IHttpActionResult ChangeItemQuantity([FromBody]ChangeItemQuantityRequestDto request)
        {
            var result = service.ChangeItemQuantity(request.Id, request.Quantity);
            var resultDto = mapper.Map<ChangeItemQuantityResponseDto>(result);
            return ResponseJson(resultDto);
        }

        [HttpGet]
        [Route("api/shoppingcart/itemspreview")]
        [CustomerAuthorizationFilter]
        public IHttpActionResult ItemsPreview()
        {
            var result = service.ItemsPreview();
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
            var result = await service.AddToCart(addItem);
            var resultDto = mapper.Map<AddToCartResultDto>(result);
            return ResponseJson(resultDto);
        }

        [HttpPost]
        [Route("api/distributor/update")]
        [CustomerAuthorizationFilter]
        public IHttpActionResult UpdateData([FromBody]DistributorDTO request)
        {
            var submitRequest = mapper.Map<Distributor>(request);
            var serviceResponse = provider.UpdateCartQuantity(submitRequest);
            return ResponseJson<string>(serviceResponse);
        }

        [HttpGet]
        [Route("api/getcartdistributordata/{skuID}/{inventoryType}")]
        [CustomerAuthorizationFilter]
        public IHttpActionResult GetCartDistributorData(int skuID, int inventoryType)
        {
            var distributorData = service.GetCartDistributorData(skuID, inventoryType);
            var result = mapper.Map<DistributorCartDto>(distributorData);
            return ResponseJson(result);
        }

        [HttpPost]
        [Route("api/updatedistributorcarts")]
        [CustomerAuthorizationFilter]
        public IHttpActionResult UpdateDistributorCarts([FromBody]DistributorCartDto request)
        {
            var submitRequest = mapper.Map<DistributorCart>(request);
            var serviceResponse = service.UpdateDistributorCarts(submitRequest);
            return ResponseJson(new { cartCount = serviceResponse });
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
