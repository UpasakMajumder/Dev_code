using Kadena.BusinessLogic.Contracts;
using System.Web.Http;
using System;
using AutoMapper;
using Kadena.WebAPI.Infrastructure;
using System.Web.Http.Results;
using System.Threading.Tasks;

namespace Kadena.WebAPI.Controllers
{
    public class PdfController : ApiControllerBase
    {
        private readonly IPdfService service;
        private readonly IMapper mapper;

        public PdfController(IPdfService service, IMapper mapper)
        {
            this.service = service ?? throw new ArgumentNullException(nameof(service));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        [Route("api/pdf/hires/{orderid}/{line}")]
        public async Task<RedirectResult> GetHiresPdf(string orderId, int line, [FromUri]string hash)
        {
            var result = await service.GetHiresPdfRedirectLink(orderId, line, hash);
            return Redirect(result);
        }

        [HttpGet]
        [Route("api/pdf/lowres/{templateid}/{settingsid}")]
        public async Task<RedirectResult> GetLowresPdf(Guid templateId, Guid settingsId, [FromUri]string hash)
        {
            var result = await service.GetLowresPdfRedirectLink(templateId, settingsId, hash);
            return Redirect(result);
        }
    }
}
