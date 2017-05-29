using Kadena.WebAPI.Contracts;
using Kadena.WebAPI.Infrastructure.Communication;
using System.Web.Http;
using System;
using Kadena.Dto.Checkout;
using System.Linq;
using Kadena.WebAPI.Models;
using AutoMapper;

namespace Kadena.WebAPI.Controllers
{
    public class ShoppingCartController : ApiController
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
            var serviceResponse = service.GetCheckoutPage();

            var responseDto = mapper.Map<CheckoutPageDTO>(serviceResponse);

            var response = new BaseResponse<CheckoutPageDTO>() 
            {
                Success = true,
                Payload = responseDto
            };

            return this.Ok<BaseResponse<CheckoutPageDTO>>(response);
        }
    }
}
