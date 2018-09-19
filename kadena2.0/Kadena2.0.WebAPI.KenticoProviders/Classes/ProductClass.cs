namespace Kadena2.WebAPI.KenticoProviders.Classes
{
    public class ProductClass : IProductClass
    {
        public double SKUWeight { get; set; }
        public bool SKUNeedsShipping { get; set; }
        public int NodeSKUID { get; set; }
    }
}