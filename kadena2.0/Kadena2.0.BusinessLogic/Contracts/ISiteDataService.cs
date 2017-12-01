using Kadena.Models.Site;

namespace Kadena.BusinessLogic.Contracts
{
    public interface ISiteDataService
    {
        KenticoSite GetKenticoSite(int? siteId, string siteName);
        ArtworkFtpSettings GetArtworkFtpSettings(int siteId);
    }
}
