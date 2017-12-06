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
            if (service == null)
            {
                throw new ArgumentNullException(nameof(service));
            }

            if (mapper == null)
            {
                throw new ArgumentNullException(nameof(mapper));
            }

            this.service = service;
            this.mapper = mapper;
        }

        [HttpGet]
        [Route("api/pdf/hires/{orderid}/{line}")]
        public async Task<RedirectResult> GetHiresPdf(string orderId, int line)
        {
            var result = await service.GetHiresPdfLink(orderId, line);
            return Redirect(result);
        }
    }
}
