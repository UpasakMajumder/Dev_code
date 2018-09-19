namespace Kadena2.WebAPI.KenticoProviders.Classes
{
    public interface IProductClass
    {
        int NodeSKUID { get; set; }
        bool SKUNeedsShipping { get; set; }
        double SKUWeight { get; set; }
    }
}