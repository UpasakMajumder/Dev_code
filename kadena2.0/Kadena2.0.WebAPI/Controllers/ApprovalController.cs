using System.Web.Http;
using System;
using Kadena.WebAPI.Infrastructure;
using Kadena.WebAPI.Infrastructure.Filters;
using Kadena.BusinessLogic.Contracts.Approval;
using System.Threading.Tasks;
using Kadena.Dto.Approval.Requests;
using Kadena.Helpers.Routes;

namespace Kadena.WebAPI.Controllers
{
    [CustomerAuthorizationFilter]
    public class ApprovalController : ApiControllerBase
    {
        private readonly IApprovalService service;

        public ApprovalController(IApprovalService service)
        {
            this.service = service ?? throw new ArgumentNullException(nameof(service));
        }

        [HttpPost]
        [Route(Routes.Order.Approve)]
        public async Task<IHttpActionResult> Approve([FromBody]ApprovalRequestDto request)
        {
            var result = await service.ApproveOrder(request.OrderId, request.CustomerId, request.CustomerName);
            return ResponseJson(result);
        }

        [HttpPost]
        [Route(Routes.Order.Reject)]
        public async Task<IHttpActionResult> Reject([FromBody]ApprovalRequestDto request)
        {
            var result = await service.RejectOrder(request.OrderId, request.CustomerId, request.CustomerName, request.RejectionNote);
            return ResponseJson(result);
        }
    }
}
