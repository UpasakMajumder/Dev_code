using Kadena.WebAPI.Contracts;
using Kadena.WebAPI.Infrastructure.Communication;
using System.Web.Http;
using System.Web.Http.Results;
using Newtonsoft.Json;
using System;

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
            var serviceResponse = service.Test();

            var response = new BaseResponse<string>()
            {
                Success = true,
                Payload = serviceResponse
            };

            return this.Json<object>(response);
        }
    }
}
