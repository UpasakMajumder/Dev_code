using Kadena.WebAPI.Contracts;
using System.Web.Http;
using System;
using AutoMapper;
using Kadena.WebAPI.Infrastructure;
using Kadena.Dto.MailTemplate.Requests;
using Kadena.Dto.MailTemplate.Responses;
using Kadena.WebAPI.Infrastructure.Filters.Authentication;

namespace Kadena.WebAPI.Controllers
{
    public class MailTemplateController : ApiControllerBase
    {
        private readonly IMailTemplateService service;
        private readonly IMapper mapper;

        public MailTemplateController(IMailTemplateService service, IMapper mapper)
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

        [HttpPost]
        [Route("api/mailtemplate")]
        [IdentityBasicAuthentication]
        public IHttpActionResult GetMailTemplate([FromBody] MailTemplateRequestDto request)
        {
            var result = service.GetMailTemplate(request.TemplateName);
            var resultDto = mapper.Map<MailTemplateDto>(result);
            return ResponseJsonCheckingNull(resultDto, $"Failed to retrieve mail template with id : {request.TemplateName}");
        }
    }
}
