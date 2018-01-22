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
        private readonly IMapper mapper;

        public CreditCard3dsiController(ICreditCardService service, IMapper mapper)
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
        [Route("api/3dsi/approveSubmission")]
        [RequestLogFilter("3DSi approval")]
        public IHttpActionResult ApproveSubmission(ApproveRequestDto request)
        {
           var success = service.VerifySubmissionId(request.SubmissionID);
           return Ok(success ? ApproveResponseDto.SubmissionApproved : ApproveResponseDto.SubmissionDenied);
        }

        [HttpPost]
        [Route("api/3dsi/saveToken")]
        public async Task<IHttpActionResult> SaveToken(SaveTokenRequestDto request)
        {
            var saveTokenData = mapper.Map<SaveTokenData>(request);
            var success = await service.SaveToken(saveTokenData);
            return Ok(success ? SaveTokenResponseDto.ResultApproved : SaveTokenResponseDto.ResultFailed);
        }


        [HttpGet]
        [Route("api/shoppingcart/creditcardSaved")]
        public IHttpActionResult CreditcardSaved(string submissionId)
        {
            var success = service.CreditcardSaved(submissionId);
            return ResponseJson<bool>(success);
        }
    }
}
