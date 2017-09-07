using CMS.CustomTables;
using CMS.Membership;
using CMS.PortalEngine.Web.UI;
using CMS.SiteProvider;
using Kadena.Models.Favorites;
using Kadena.WebAPI.KenticoProviders.Contracts;
using System;
using System.Linq;

namespace Kadena.WebAPI.KenticoProviders
{
    public class KenticoFavoritesProvider : IKenticoFavoritesProvider
    {
        public void SetFavoriteProduct(int productDocumentId)
        {
            var existingRecord = GetFavoriteRecord(SiteContext.CurrentSiteID, MembershipContext.AuthenticatedUser.UserID, productDocumentId );

            CustomTableItemProvider.SetItem
        }

        public void UnsetFavoriteProduct(int productDocumentId)
        {
            var existingRecord = GetFavoriteRecord(SiteContext.CurrentSiteID, MembershipContext.AuthenticatedUser.UserID, productDocumentId);

            if (existingRecord != null)
            {
                CustomTableItemProvider.DeleteItem(existingRecord);
            }
        }

        public CustomTableItem GetFavoriteRecord(int siteId, int userId, int productDocumentId)
        {
            return CustomTableItemProvider.GetItems("KDA.FavoriteProducts")
                .WhereEquals("ItemSiteID", siteId)
                .WhereEquals("ItemUserID", userId)
                .WhereEquals("ItemDocumentID", productDocumentId)
                .FirstObject;
        }
    }
}
