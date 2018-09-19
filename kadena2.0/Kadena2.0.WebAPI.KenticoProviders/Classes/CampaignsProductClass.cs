namespace Kadena2.WebAPI.KenticoProviders.Classes
{
    public class CampaignsProductClass : IProductClass
    {
        public int NodeSKUID { get; set; }
        public bool SKUNeedsShipping { get; set; }
        public double SKUWeight { get; set; }
    }
}