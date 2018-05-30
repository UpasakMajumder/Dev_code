using System.Web.Http;
using System;
using Kadena.WebAPI.Infrastructure;
using Kadena.WebAPI.Infrastructure.Filters;
using Kadena.BusinessLogic.Contracts.Approval;
using System.Threading.Tasks;
using Kadena.Dto.Approval.Requests;
using Kadena.Helpers.Routes;
using AutoMapper;
using Kadena.Dto.Approval.Responses;

namespace Kadena.WebAPI.Controllers
{
    [CustomerAuthorizationFilter]
    public class ApprovalController : ApiControllerBase
    {
        private readonly IApprovalService service;
        private readonly IMapper mapper;

        public ApprovalController(IApprovalService service, IMapper mapper)
        {
            this.service = service ?? throw new ArgumentNullException(nameof(service));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpPost]
        [Route(Routes.Order.Approve)]
        public async Task<IHttpActionResult> Approve([FromBody]ApprovalRequestDto request)
        {
            var approveResult = await service.ApproveOrder(request.OrderId, request.CustomerId, request.CustomerName);
            var result = mapper.Map<ApprovalResultDto>(approveResult);
            return ResponseJson(result);
        }

        [HttpPost]
        [Route(Routes.Order.Reject)]
        public async Task<IHttpActionResult> Reject([FromBody]ApprovalRequestDto request)
        {
            var rejectResult = await service.RejectOrder(request.OrderId, request.CustomerId, request.CustomerName, request.RejectionNote);
            var result = mapper.Map<ApprovalResultDto>(rejectResult);
            return ResponseJson(result);
        }
    }
}
