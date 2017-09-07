using Kadena.WebAPI.KenticoProviders.Contracts;
using System.Collections.Generic;
using Kadena.Models.Product;
using CMS.DocumentEngine;
using CMS.Membership;
using CMS.SiteProvider;
using CMS.DataEngine;
using CMS.Localization;
using System.Linq;

namespace Kadena.WebAPI.KenticoProviders
{
    public class KenticoProductsProvider : IKenticoProductsProvider
    {
        public List<ProductCategoryLink> GetCategories(string path)
        {
            return null;
        }

        public List<ProductLink> GetProducts(string path)
        {
            var tree = new TreeProvider(MembershipContext.AuthenticatedUser);

            var pages = tree.SelectNodes()
                            .Path(path, PathTypeEnum.Children)
                            .WhereEquals("ClassName", "KDA.Product")
                            .OnSite(new SiteInfoIdentifier(SiteContext.CurrentSiteID))
                            .Culture(LocalizationContext.CurrentCulture.CultureCode)
                            .CheckPermissions()
                            .NestingLevel(1)
                            .OnCurrentSite();

            return pages.ToList().Select(p => new ProductLink
            {
                Id = p.DocumentID,
                Title = p.DocumentName,
                Url = p.DocumentUrlPath,
                ImageUrl = "/img.jpg",
                IsFavorite = false
            }
            ).ToList();
        }
    }
}
