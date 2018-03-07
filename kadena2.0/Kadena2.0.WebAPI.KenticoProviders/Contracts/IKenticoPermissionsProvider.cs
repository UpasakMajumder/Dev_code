namespace Kadena2.WebAPI.KenticoProviders.Contracts
{
    public interface IKenticoPermissionsProvider
    {
        /// <summary>
        /// Checks if user has permission
        /// </summary>
        /// <param name="resourceName">Code name of the module containing given permission</param>
        /// <param name="permissionName">Permission name</param>
        /// <param name="siteName">Site name</param>
        bool IsAuthorizedPerResource(string resourceName, string permissionName, string siteName);
        bool UserCanSeePrices();
        bool UserCanSeePrices(int siteId, int userId);
        bool UserCanSeeAllOrders();
        bool UserCanModifyShippingAddress();
        bool UserCanDownloadHiresPdf(int siteId, int userId);
    }
}
