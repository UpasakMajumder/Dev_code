namespace Kadena.Dto.TemplatedProduct.MicroserviceRequests
{
    public class SetMailingListRequestDTO
    {
        public string ContainerId { get; set; }
        public string TemplateId { get; set; }
        public string WorkSpaceId { get; set; }
        public bool Use3d { get; set; }
        public string CustomerName { get; set; }
    }
}
