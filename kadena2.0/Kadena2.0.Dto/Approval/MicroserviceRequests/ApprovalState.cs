using System.ComponentModel.DataAnnotations;

namespace Kadena.Dto.Approval.MicroserviceRequests
{
    public enum ApprovalState
    {
        [Display(Name = "ApproveOrder")]
        Approved = 200,

        [Display(Name = "RejectOrder")]
        ApprovalRejected = 300
    }
}
