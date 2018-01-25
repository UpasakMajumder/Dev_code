using Kadena.BusinessLogic.Contracts;
using System.Web.Http;
using System;
using Kadena.WebAPI.Infrastructure;
using Kadena.Dto.CreditCard._3DSi.Requests;
using Kadena.Dto.CreditCard._3DSi.Responses;
using System.Threading.Tasks;
using AutoMapper;
using Kadena.Models.CreditCard;
using Kadena.WebAPI.Infrastructure.Filters;

namespace Kadena.WebAPI.Controllers
{
    
    public class CreditCard3dsiController : ApiControllerBase
    {
        private readonly ICreditCardService service;
        private readonly ISubmissionService submissions;
        private readonly IMapper mapper;

        public CreditCard3dsiController(ICreditCardService service, ISubmissionService submissions, IMapper mapper)
        {
            if (service == null)
            {
                throw new ArgumentNullException(nameof(service));
            }
            if (submissions == null)
            {
                throw new ArgumentNullException(nameof(submissions));
            }
            if (mapper == null)
            {
                throw new ArgumentNullException(nameof(mapper));
            }

            this.service = service;
            this.submissions = submissions;
            this.mapper = mapper;
        }

        /// <summary>
        /// 3DSi will call this to save token
        /// </summary>
        [HttpPost]
        [Route("api/3dsi/approveSubmission")]
        [RequestLogFilter("3DSi Approve")]
        public IHttpActionResult ApproveSubmission(ApproveRequestDto request)
        {
           var success = submissions.VerifySubmissionId(request.SubmissionID);
           return Ok(success ? ApproveResponseDto.SubmissionApproved : ApproveResponseDto.SubmissionDenied);
        }

        /// <summary>
        /// 3DSi will call this to save token
        /// </summary>
        [HttpPost]
        [Route("api/3dsi/savetoken")]
        [RequestLogFilter("3DSi SaveToken")]
        public async Task<IHttpActionResult> SaveToken(SaveTokenRequestDto request)
        {
            var saveTokenData = mapper.Map<SaveTokenData>(request);
            var success = await service.SaveToken(saveTokenData);
            return Ok(success ? SaveTokenResponseDto.ResultApproved : SaveTokenResponseDto.ResultFailed);
        }

        /// <summary>
        /// FE calls this to find out if card was stored and authorized
        /// </summary>
        [HttpGet]
        [Route("api/3dsi/creditcardSaved")]
        [CustomerAuthorizationFilter]
        public IHttpActionResult CreditcardSaved(string submissionId)
        {
            var success = service.CreditcardSaved(submissionId);
            return ResponseJson<bool>(success);
        }

        /// <summary>
        /// FE calls this to get SubmissionID, which it will use to call 3DSi
        /// </summary>
        [HttpGet]
        [Route("api/3dsi/getsubmissionid")]
        [CustomerAuthorizationFilter]
        public IHttpActionResult GetSubmissionId()
        {
            var submission = submissions.GenerateSubmissionId();
            var responseDto = mapper.Map<GetSubmissionIdResponseDto>(submission);
            return ResponseJson(responseDto);
        }
    }
}
