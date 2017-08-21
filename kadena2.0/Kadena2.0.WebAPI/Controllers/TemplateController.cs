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
        [Route("api/template/setname/{templateId}/{name}")]
        public async Task<IHttpActionResult> SetName(Guid templateId, string name)
        {
            var result = await _templateService.SetName(templateId, name);
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