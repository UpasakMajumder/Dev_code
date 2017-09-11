using CMS.CustomTables;
using CMS.Membership;
using CMS.SiteProvider;
using Kadena.WebAPI.KenticoProviders.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Kadena.WebAPI.KenticoProviders
{
    public class KenticoFavoritesProvider : IKenticoFavoritesProvider
    {
        private readonly string CustomTableName = "KDA.FavoriteProducts";

        public void SetFavoriteProduct(int productDocumentId)
        {
            var existingRecord = GetFavoriteRecord(SiteContext.CurrentSiteID, MembershipContext.AuthenticatedUser.UserID, productDocumentId );

            if (existingRecord != null)
            {
                existingRecord.Update();
            }
            else
            {
                var newItem = CustomTableItem.New(CustomTableName);
                newItem.SetValue("ItemSiteID", SiteContext.CurrentSiteID);
                newItem.SetValue("ItemUserID", MembershipContext.AuthenticatedUser.UserID);
                newItem.SetValue("ItemDocumentID", productDocumentId);
                newItem.SetValue("ItemOrder", 1);
                newItem.Insert();
            }
        }

        public void UnsetFavoriteProduct(int productDocumentId)
        {
            var existingRecord = GetFavoriteRecord(SiteContext.CurrentSiteID, MembershipContext.AuthenticatedUser.UserID, productDocumentId);

            if (existingRecord != null)
            {
                CustomTableItemProvider.DeleteItem(existingRecord);
            }
        }

        private CustomTableItem GetFavoriteRecord(int siteId, int userId, int productDocumentId)
        {
            return CustomTableItemProvider.GetItems(CustomTableName)
                .WhereEquals("ItemSiteID", siteId)
                .WhereEquals("ItemUserID", userId)
                .WhereEquals("ItemDocumentID", productDocumentId)
                .FirstObject;
        }

        public List<int> CheckFavoriteProductIds(List<int> productIds)
        {
            var favorites = CustomTableItemProvider.GetItems(CustomTableName)
                .WhereEquals("ItemSiteID", SiteContext.CurrentSiteID)
                .WhereEquals("ItemUserID", MembershipContext.AuthenticatedUser.UserID)
                .WhereIn("ItemDocumentID", productIds);

            return favorites.Select(f => Convert.ToInt32(f["ItemDocumentID"])).ToList();
        }
    }
}
