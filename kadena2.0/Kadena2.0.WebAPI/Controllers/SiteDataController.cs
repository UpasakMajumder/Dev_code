using Kadena.Dto.Site;
using Kadena.WebAPI.Contracts;
using Kadena.WebAPI.Infrastructure;
using System.Web.Http;

namespace Kadena.WebAPI.Controllers
{
    public class SiteDataController : ApiControllerBase
    {
        private readonly ISiteDataService _service;

        public SiteDataController(ISiteDataService service)
        {
            _service = service;
        }

        [HttpPost]
        [Route("api/sitedata/ordermanageremail")]
        public IHttpActionResult GetOrderManagerEmail([FromBody]SiteDataRequestDto request)
        {
            var result = _service.GetOrderInfoRecepients(request.SiteName);
            return ResponseJson(result);
        }
    }
}