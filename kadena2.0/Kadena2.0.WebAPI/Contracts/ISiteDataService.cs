using Kadena.Models.Site;

namespace Kadena.WebAPI.Contracts
{
    public interface ISiteDataService
    {
        string GetOrderInfoRecepients(int siteId);
        ArtworkFtpSettings GetArtworkFtpSettings(int siteId);
    }
}
