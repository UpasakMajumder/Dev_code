using Kadena.Models.Site;

namespace Kadena.WebAPI.Contracts
{
    public interface ISiteDataService
    {
        KenticoSite GetKenticoSite(int siteId);
        ArtworkFtpSettings GetArtworkFtpSettings(int siteId);
    }
}
