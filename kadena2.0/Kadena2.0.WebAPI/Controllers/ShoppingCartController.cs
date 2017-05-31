using Kadena.WebAPI.Contracts;
using System.Web.Http;
using System;
using Kadena.Dto.Checkout;
using AutoMapper;
using Kadena.WebAPI.Infrastructure;
using Kadena.WebAPI.Infrastructure.Requests;

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
        //[Route("api/shoppingcart")]
        public IHttpActionResult Get()
        {
            var checkoutPage = service.GetCheckoutPage();
            var checkoutPageDto = mapper.Map<CheckoutPageDTO>(checkoutPage);
            return ResponseJson(checkoutPageDto);
        }

        [HttpPost]
        //[Route("api/shoppingcart/selectshipping")]
        public IHttpActionResult SelectShipping([FromBody]ChangeSelectionRequestDto request)
        {
            var result = service.SelectShipipng(request.Id);
            var resultDto = mapper.Map<CheckoutPageDTO>(result);
            return ResponseJson(resultDto);
        }

        [HttpPost]
        //[Route("api/shoppingcart/selectaddress")]
        public IHttpActionResult SelectAddress([FromBody]ChangeSelectionRequestDto request)
        {
            var result = service.SelectAddress(request.Id);
            var resultDto = mapper.Map<CheckoutPageDTO>(result);
            return ResponseJson(resultDto);
        }

        [HttpPost]
        //[Route("api/shoppingcart/submit")]
        public IHttpActionResult Submit([FromBody]SubmitRequestDto request)
        {
            throw new NotImplementedException();
        }

    }
}
