using Kadena.Models.CampaignData;

namespace Kadena.BusinessLogic.Contracts
{
    public interface IByteConverter
    {
        byte[] GetBytes(string contentHtml, string coverHtml);
    }
}
