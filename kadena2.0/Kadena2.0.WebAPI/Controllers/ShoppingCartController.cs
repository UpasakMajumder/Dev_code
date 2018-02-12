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

namespace Kadena.WebAPI.Controllers
{
    public class ShoppingCartController : ApiControllerBase
    {
        private readonly IShoppingCartService service;
        private readonly IMapper mapper;
        private readonly IShoppingCartProvider provider;

        public ShoppingCartController(IShoppingCartService service, IMapper mapper, IShoppingCartProvider provider)
        {
            if (service == null)
            {
                throw new ArgumentNullException(nameof(service));
            }

            if (mapper == null)
            {
                throw new ArgumentNullException(nameof(mapper));
            }
            if (provider == null)
            {
                throw new ArgumentNullException(nameof(provider));
            }

            this.service = service;
            this.mapper = mapper;
            this.provider = provider;
        }

        [HttpGet]
        [Route("api/shoppingcart")]
        [CustomerAuthorizationFilter]
        public IHttpActionResult Get()
        {
            var checkoutPage = service.GetCheckoutPage();
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
        [Route("api/deliverytotals")]
        [CustomerAuthorizationFilter]
        public async Task<IHttpActionResult> SetDeliveryAddress([FromBody] DeliveryAddressDTO postedAddress)
        {
            var address = mapper.Map<DeliveryAddress>(postedAddress);
            var deliveryTotals = await service.SetDeliveryAddress(address);
            var deliveryTotalsDto = mapper.Map<CheckoutPageDeliveryTotalsDTO>(deliveryTotals);
            return ResponseJson(deliveryTotalsDto);
        }

        [HttpPost]
        [Route("api/shoppingcart/selectshipping")]
        [CustomerAuthorizationFilter]
        public IHttpActionResult SelectShipping([FromBody]ChangeSelectionRequestDto request)
        {
            service.SelectShipipng(request.Id);
            return ResponseJson<object>(null);
        }

        [HttpPost]
        [Route("api/shoppingcart/selectaddress")]
        [CustomerAuthorizationFilter]
        public IHttpActionResult SelectAddress([FromBody]ChangeSelectionRequestDto request)
        {
            var result = service.SelectAddress(request.Id);
            var resultDto = mapper.Map<CheckoutPageDTO>(result);
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
        public async Task<IHttpActionResult> AddToCart([FromBody] NewCartItemDto item)
        {
            var addItem = mapper.Map<NewCartItem>(item);
            var result = await service.AddToCart(addItem);
            var resultDto = mapper.Map<AddToCartResultDto>(result);
            return ResponseJson(resultDto);
        }
        [HttpPost]
        [Route("api/distributor/update")]
        public IHttpActionResult UpdateData([FromBody]DistributorDTO request)
        {
            var submitRequest = mapper.Map<Distributor>(request);
            var serviceResponse = provider.UpdateCartQuantity(submitRequest);
            if (serviceResponse.Item2)
            {
                return ResponseJson<string>(serviceResponse.Item1);
            }
            else
            {
                return ResponseJsonCheckingNull<string>(null, serviceResponse.Item1);
            }
        }
    }
}
