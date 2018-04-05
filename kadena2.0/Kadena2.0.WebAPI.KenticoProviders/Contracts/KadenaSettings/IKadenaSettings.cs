namespace Kadena2.WebAPI.KenticoProviders.Contracts.KadenaSettings
{
    public interface IKadenaSettings
    {
        string DefaultSiteCompanyName { get; }
        string DefaultSitePersonalName { get; }
        string DefaultCustomerCompanyName { get; }
        string TermsAndConditionsPage { get; }
        string CheckoutPageUrl { get; }
        string ErpCustomerId { get; }
        string OrderNotificationEmail { get; }
        bool FTPArtworkEnabled(int siteId);
        string FTPArtworkUrl(int siteId);
        string FTPArtworkUsername(int siteId);
        string FTPArtworkPassword(int siteId);
    }
}
