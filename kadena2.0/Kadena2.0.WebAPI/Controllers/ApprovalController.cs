using System.Web.Http;
using System;
using AutoMapper;
using Kadena.WebAPI.Infrastructure;
using Kadena.WebAPI.Infrastructure.Filters;
using Kadena.BusinessLogic.Contracts.Approval;
using System.Threading.Tasks;

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
        [Route("api/approval/aprove")]
        public async Task<IHttpActionResult> Approve(string orderId, int customerId, string customerName)
        {
            var result = await service.ApproveOrder(orderId, customerId, customerName);
            return ResponseJson(result);
        }

        [HttpPost]
        [Route("api/approval/reject")]
        public async Task<IHttpActionResult> Reject(string orderId, int customerId, string customerName, string rejectionNote)
        {
            var result = await service.RejectOrder(orderId, customerId, customerName, rejectionNote);
            return ResponseJson(result);
        }
    }
}
