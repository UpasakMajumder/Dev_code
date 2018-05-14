using Kadena.Models.Membership;
using System.Collections.Generic;

namespace Kadena2.WebAPI.KenticoProviders.Contracts
{
    public interface IKenticoPermissionsProvider
    {
        /// <summary>
        /// Checks if current user has permission on current site
        /// </summary>
        /// <param name="resourceName">Code name of the module containing given permission</param>
        /// <param name="permissionName">Permission name</param>
        bool CurrentUserHasPermission(string resourceName, string permissionName);

        /// <summary>
        /// Checks if current user has permission on given site
        /// </summary>
        /// <param name="resourceName">Code name of the module containing given permission</param>
        /// <param name="permissionName">Permission name</param>
        /// <param name="siteName">Site name</param>
        bool CurrentUserHasPermission(string resourceName, string permissionName, string siteName);

        /// <summary>
        /// Checks if given user has permission on current site
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="resourceName">Code name of the module containing given permission</param>
        /// <param name="permissionName">Permission name</param>
        /// <returns></returns>
        bool UserHasPermission(int userId, string resourceName, string permissionName);

        /// <summary>
        /// Checks if given user has permission on given site
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="resourceName">Code name of the module containing given permission</param>
        /// <param name="permissionName">Permission name</param>
        /// <param name="siteName"></param>
        /// <returns></returns>
        bool UserHasPermission(int userId, string resourceName, string permissionName, string siteName);
        bool UserCanSeePrices();
        bool UserCanSeePrices(int siteId, int userId);
        bool UserCanSeeAllOrders();
        bool UserCanModifyShippingAddress();
        bool UserCanDownloadHiresPdf(int siteId, int userId);
        IEnumerable<User> GetUsersWithPermission(string resourceName, string permissionName, int siteId);
    }
}
