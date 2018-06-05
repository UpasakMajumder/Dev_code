namespace Kadena.Dto.Approval.MicroserviceRequests
{
    public class ApprovalRequestDto
    {
        public int ApproversCount { get; set; }
        public string OrderId { get; set; }
        public ApprovalUnitDto[] Approvals { get; set; }
    }
}
