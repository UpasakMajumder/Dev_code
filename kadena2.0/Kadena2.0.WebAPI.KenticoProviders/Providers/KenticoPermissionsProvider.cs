using CMS.Membership;
using CMS.SiteProvider;
using Kadena2.WebAPI.KenticoProviders.Contracts;

namespace Kadena2.WebAPI.KenticoProviders.Providers
{
    public class KenticoPermissionsProvider : IKenticoPermissionsProvider
    {
        public bool IsAuthorizedPerResource(string resourceName, string permissionName, string siteName)
        {
            return MembershipContext.AuthenticatedUser.IsAuthorizedPerResource(resourceName, permissionName, siteName);
        }

        public bool UserCanSeePrices()
        {
            return UserInfoProvider.IsAuthorizedPerResource("Kadena_Orders", "KDA_SeePrices", SiteContext.CurrentSiteName, MembershipContext.AuthenticatedUser);
        }

        public bool UserCanSeePrices(int siteId, int userId)
        {
            var userinfo = UserInfoProvider.GetUserInfo(userId);
            var site = SiteInfoProvider.GetSiteInfo(siteId);

            if (userinfo == null || site == null)
                return false;

            return UserInfoProvider.IsAuthorizedPerResource("Kadena_Orders", "KDA_SeePrices", site.SiteName, userinfo);
        }

        public bool UserCanSeeAllOrders()
        {
            return UserInfoProvider.IsAuthorizedPerResource("Kadena_Orders", "KDA_SeeAllOrders", SiteContext.CurrentSiteName, MembershipContext.AuthenticatedUser);
        }

        public bool UserCanModifyShippingAddress()
        {
            return UserInfoProvider.IsAuthorizedPerResource("Kadena_User_Settings", "KDA_ModifyShippingAddress",
                SiteContext.CurrentSiteName, MembershipContext.AuthenticatedUser);
        }

        public bool UserCanDownloadHiresPdf(int siteId, int userId)
        {
            var userinfo = UserInfoProvider.GetUserInfo(userId);
            var site = SiteInfoProvider.GetSiteInfo(siteId);

            if (userinfo == null || site == null)
                return false;

            return UserInfoProvider.IsAuthorizedPerResource("Kadena_Orders", "KDA_CanDownloadHiresPdf", site.SiteName, userinfo);
        }        
    }
}
