using Kadena.Models.Checkout;
using Kadena.WebAPI.Contracts;
using Kadena.WebAPI.Infrastructure;
using System;
using System.Threading.Tasks;
using System.Web.Http;

namespace Kadena.WebAPI.Controllers
{
    public class TemplateController : ApiControllerBase
    {
        private readonly ITemplateService _templateService;

        public TemplateController(ITemplateService templateService)
        {
            _templateService = templateService;
        }

        [HttpPost]
        [Route("api/template/setname")]
        public async Task<IHttpActionResult> SetName([FromBody] NewCartItem item)
        {
            var result = await _templateService.SetName(item.TemplateId, item.CustomProductName);
            if (result)
            {
                return ResponseJson(result);
            }
            else
            {
                return ErrorJson("Failed request. See logs for more information.");
            }
        }
    }
}