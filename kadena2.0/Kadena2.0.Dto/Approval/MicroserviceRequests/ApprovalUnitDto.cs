namespace Kadena.Dto.Approval.MicroserviceRequests
{
    public class ApprovalUnitDto
    {
        public CustomerDto Customer { get; set; }
        public string Note { get; set; }
        public int State { get; set; }
    }
}
