using Kadena.WebAPI.Contracts;
using System.Web.Http;
using System;
using Kadena.WebAPI.Infrastructure;
using Kadena.Dto.CreditCard._3DSi.Requests;
using Kadena.Dto.CreditCard._3DSi.Responses;

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
        public IHttpActionResult ApproveSubmission(ApproveRequestDto request)
        {
           var success = service.VerifySubmissionId(request.SubmissionID);
           return Ok(success ? ApproveResponseDto.SubmissionApproved : ApproveResponseDto.SubmissionDenied);
        }

        [HttpPost]
        [Route("api/3dsi/saveToken")]
        public IHttpActionResult SaveToken(SaveTokenRequestDto request)
        {
            var success = true; // TODO service.SaveToken(request);
            return Ok(success ? SaveTokenResponseDto.ResultApproved : SaveTokenResponseDto.ResultFailed);
        }
    }
}
