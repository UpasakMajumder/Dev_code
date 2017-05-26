using Kadena.WebAPI.Contracts;
using System.Web.Http;

namespace Kadena.WebAPI.Controllers
{
    public class ShoppingCartController : ApiController
    {
        private readonly IShoppingCartService service;

        public ShoppingCartController(IShoppingCartService service)
        {
            this.service = service;
        }

        [HttpPost]
        public IHttpActionResult Test()
        {
            if (this.ModelState.IsValid)
            {
                
            }

            return Ok(service.Test());
        }
    }
}
