using Kadena.WebAPI.Contracts;
using System.Web.Http;
using System;
using Kadena.WebAPI.Infrastructure;
using Kadena.Dto.CreditCard._3DSi.Requests;

namespace Kadena.WebAPI.Controllers
{
    public class CreditCard3dsiController : ApiControllerBase
    {
        private readonly ICreditCardService service;

        public CreditCard3dsiController(ICreditCardService service)
        {
            if (service == null)
            {
                throw new ArgumentNullException(nameof(service));
            }

            this.service = service;
        }

        [HttpPost]
        [Route("api/3dsi/approveSubmission")]
        public IHttpActionResult ApproveSubmission([FromBody]ApproveRequestDto request)
        {
            return null;
        }

        [HttpPost]
        [Route("api/3dsi/saveToken")]
        public IHttpActionResult SaveToken([FromBody]SaveTokenRequestDto request)
        {
            return null;
        }
    }
}
