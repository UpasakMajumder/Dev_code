using Kadena.Models.CampaignData;

namespace Kadena.BusinessLogic.Contracts
{
    public interface ICatalogService
    {
        byte[] GetPdfBytes(string contentHtml, string coverHtml);
    }
}
