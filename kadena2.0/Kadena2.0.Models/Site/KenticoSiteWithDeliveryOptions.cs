namespace Kadena.Models.Site
{
    public class KenticoSiteWithDeliveryOptions : KenticoSite
    {
        public DeliveryOption[] DeliveryOptions { get; set; }
    }
}