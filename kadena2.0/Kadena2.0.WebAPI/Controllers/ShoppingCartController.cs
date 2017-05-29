using Kadena.WebAPI.Contracts;
using Kadena.WebAPI.Infrastructure.Communication;
using System.Web.Http;
using System;
using Kadena.Dto.Checkout;
using System.Linq;
using Kadena.WebAPI.Models;

namespace Kadena.WebAPI.Controllers
{
    public class ShoppingCartController : ApiController
    {
        private readonly IShoppingCartService service;

        public ShoppingCartController(IShoppingCartService service)
        {
            if (service == null)
            {
                throw new ArgumentNullException(nameof(service));
            }

            this.service = service;
        }

        [HttpGet]
        public IHttpActionResult Get()
        {
            var serviceResponse = service.GetCheckoutPage();

            var response = new BaseResponse<CheckoutPage>() // TODO return DTOs !
            {
                Success = true,
                Payload = serviceResponse
            };

            return this.Ok<BaseResponse<CheckoutPage>>(response);
        }
    }
}
