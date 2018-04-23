using System;
using System.Threading.Tasks;

namespace Kadena.BusinessLogic.Contracts
{
    public interface IPdfService
    {
        string GetHiresPdfUrl(string orderId, int lineNumber);
        string GetLowresPdfUrl(Guid templateId, Guid settingsId);

        Task<string> GetHiresPdfRedirectLink(string orderId, int line, string hash);
        Task<string> GetLowresPdfRedirectLink(Guid templateId, Guid settingsId, string hash);
    }
}
