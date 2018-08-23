namespace Kadena.Dto.Site.Responses
{
    public class SiteDataResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string ErpCustomerId { get; set; }
        public string OrderManagerEmail { get; set; }
        public string SiteDomain { get; set; }
        public DeliveryOptionDto[] DeliveryOptions { get; set; }
    }
}
