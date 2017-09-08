using Kadena.WebAPI.KenticoProviders.Contracts;
using System.Collections.Generic;
using Kadena.Models.Product;
using CMS.DocumentEngine;
using CMS.SiteProvider;
using CMS.DataEngine;
using CMS.Localization;
using System.Linq;
using CMS.Helpers;

namespace Kadena.WebAPI.KenticoProviders
{
    public class KenticoProductsProvider : IKenticoProductsProvider
    {
        public List<ProductCategoryLink> GetCategories(string path)
        {
            var pages = GetDocuments(path, "KDA.ProductCategory");
            return pages.Select(p => new ProductCategoryLink
            {
                Id = p.DocumentID,
                Title = p.DocumentName,
                Url = p.DocumentUrlPath,
                ImageUrl = URLHelper.GetAbsoluteUrl(p.GetValue("ProductCategoryImage", string.Empty))
            }
            ).ToList();
        }

        public List<ProductLink> GetProducts(string path)
        {
            var pages = GetDocuments(path, "KDA.Product");

            return pages.Select(p => new ProductLink
            {
                Id = p.DocumentID,
                Title = p.DocumentName,
                Url = p.DocumentUrlPath,
                ImageUrl = URLHelper.GetAbsoluteUrl(p.GetValue("ProductThumbnail", string.Empty) == string.Empty ? 
                                                    p.GetValue("SKUImagePath", string.Empty) : 
                                                    "/CMSPages/GetFile.aspx?guid=" + p.GetValue("ProductThumbnail")),
                IsFavorite = false
            }
            ).ToList();
        }

        private DocumentQuery GetDocuments(string path, string className)
        {
            return DocumentHelper.GetDocuments(className)
                            .Path(path, PathTypeEnum.Children)
                            .WhereEquals("ClassName", className)
                            .OnSite(new SiteInfoIdentifier(SiteContext.CurrentSiteID))
                            .Culture(LocalizationContext.CurrentCulture.CultureCode)
                            .CheckPermissions()
                            .NestingLevel(1)
                            .OnCurrentSite();
        }
    }
}
