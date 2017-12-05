using Kadena.BusinessLogic.Contracts;
using System.Web.Http;
using System;
using AutoMapper;
using Kadena.WebAPI.Infrastructure;
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

        [HttpGet]
        [Route("api/site/{siteId}/mailtemplate/{templateName}/language/{languageCode}")]
        [Route("api/site/{siteId}/mailtemplate/{templateName}/language")]
        [Route("api/site/{siteId}/mailtemplate/{templateName}")]
        [IdentityBasicAuthentication]
        public IHttpActionResult GetMailTemplate(int siteId, string templateName, string languageCode = "")
        {
            var result = service.GetMailTemplate(siteId, templateName, languageCode);
            var resultDto = mapper.Map<MailTemplateDto>(result);
            return ResponseJsonCheckingNull(resultDto, $"Failed to retrieve mail template or it's localized variant");
        }
    }
}
