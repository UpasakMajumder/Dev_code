using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Kadena.WebAPI.Controllers
{
    public class ShoppingCartController : ApiController
    {
        [HttpPost]
        public IHttpActionResult Test()
        {
            if (this.ModelState.IsValid)
            {
                
            }

            return this.Ok("All is ok");
        }
    }
}
