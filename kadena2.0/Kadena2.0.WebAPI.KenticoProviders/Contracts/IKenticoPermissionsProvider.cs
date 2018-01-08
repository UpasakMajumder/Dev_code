namespace Kadena2.WebAPI.KenticoProviders.Contracts
{
    public interface IKenticoPermissionsProvider
    {
        bool IsAuthorizedPerResource(string resourceName, string permissionName, string siteName);
        bool UserCanSeePrices();
        bool UserCanSeePrices(int siteId, int userId);
        bool UserCanSeeAllOrders();
        bool UserCanModifyShippingAddress();
        bool UserCanDownloadHiresPdf(int siteId, int userId);
    }
}
