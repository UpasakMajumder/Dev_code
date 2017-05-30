using Kadena.WebAPI.Contracts;
using System.Web.Http;
using System;
using Kadena.Dto.Checkout;
using AutoMapper;
using Kadena.WebAPI.Infrastructure;

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
        public IHttpActionResult Get()
        {
            var checkoutPage = service.GetCheckoutPage();
            var checkoutPageDto = mapper.Map<CheckoutPageDTO>(checkoutPage);
            return ResponseJson(checkoutPageDto);
        }
    }
}
