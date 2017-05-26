using Kadena.WebAPI.Contracts;
using Kadena.WebAPI.Infrastructure.Communication;
using System.Web.Http;
using System;
using Kadena.Dto.Checkout;
using System.Linq;

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
            //var serviceResponse = service.Test();

            var result = new CheckoutPageDTO()
            {
                DeliveryAddresses = new DeliveryAddressesContainerDTO()
                {
                    AddAddressLabel ="add address",
                    Description = "asdasdasd",
                    Title = "Addresses",

                    items = new[]
                    {
                        new DeliveryAddressDTO()
                        {
                            Checked = true,
                            City = "asdasd",
                            Id = 1,
                            State = "CZ",
                            Street = new[] { "Hlavni" }.ToList(),
                            Zip = "11150"
                        }
                    }.ToList()
                }
            };

            var response = new BaseResponse<CheckoutPageDTO>()
            {
                Success = true,
                Payload = result
            };

            return this.Json<BaseResponse<CheckoutPageDTO>>(response);
        }
    }
}
