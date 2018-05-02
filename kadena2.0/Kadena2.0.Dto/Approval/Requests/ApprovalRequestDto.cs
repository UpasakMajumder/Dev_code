using System.ComponentModel.DataAnnotations;

namespace Kadena.Dto.Approval.Requests
{
    public class ApprovalRequestDto
    {
        [Required]
        public string OrderId { get; set; }
        [Required]
        public int CustomerId { get; set; }
        [Required]
        public string CustomerName { get; set; }
        public string RejectionNote { get; set; }
    }
}
