using Kadena.Models.Site;

namespace Kadena.WebAPI.Contracts
{
    public interface ISiteDataService
    {
        string GetOrderInfoRecepients(string siteName);
        ArtworkFtpSettings GetArtworkFtpSettings(int siteId);
    }
}
