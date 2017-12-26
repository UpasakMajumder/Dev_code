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
        bool FTPArtworkEnabled(string siteName);
        string FTPArtworkUrl(string siteName);
        string FTPArtworkUsername(string siteName);
        string FTPArtworkPassword(string siteName);
    }
}
