using CMS.CustomTables;
using CMS.DocumentEngine;
using CMS.Helpers;
using CMS.Membership;
using CMS.SiteProvider;
using Kadena.Models.Product;
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

        public List<ProductLink> GetFavorites(int count)
        {
            var favorites = CustomTableItemProvider.GetItems(CustomTableName)
                 .WhereEquals("ItemSiteID", SiteContext.CurrentSiteID)
                 .WhereEquals("ItemUserID", MembershipContext.AuthenticatedUser.UserID)
                 .OrderByDescending("ItemModifiedWhen");

            if (count > 0)
                 favorites = favorites.TopN(count);

            return favorites.Select(f => CreateProcuct(f.GetIntegerValue("ItemDocumentID", 0)))
                .Where(f => f!=null)
                .ToList();
        }

        private ProductLink CreateProcuct(int documentId)
        {
            var product = DocumentHelper.GetDocuments("KDA.Product").Where(d => d.DocumentID == documentId).FirstOrDefault();

            if (product == null)
                return null;

            return new ProductLink
            {
                Id = documentId,
                ImageUrl = URLHelper.GetAbsoluteUrl(product.GetValue("ProductThumbnail", string.Empty) == string.Empty ? product.GetValue("SKUImagePath", string.Empty) : "/CMSPages/GetFile.aspx?guid=" + product.GetValue("ProductThumbnail")),
                Title = product.DocumentName,
                Url = product.DocumentUrlPath
            };
        }
    }
}
