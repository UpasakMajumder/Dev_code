using Kadena.BusinessLogic.Contracts;
using System.Web.Http;
using System;
using AutoMapper;
using Kadena.WebAPI.Infrastructure;
using Kadena.Dto.TemplatedProduct.Requests;
using Kadena.Models.TemplatedProduct;

namespace Kadena.WebAPI.Controllers
{
    public class MailController : ApiControllerBase
    {
        private readonly IMailService service;
        private readonly IMapper mapper;

        public MailController(IMailService service, IMapper mapper)
        {
            this.service = service ?? throw new ArgumentNullException(nameof(service));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpPost]
        [Route("api/mail/proof")]
        public IHttpActionResult GetMailTemplate([FromBody]EmailProofRequestDto requestDto)
        {
            var request = mapper.Map<EmailProofRequest>(requestDto);
            service.SendProofMail(request);
            return SuccessJson();
        }
    }
}
