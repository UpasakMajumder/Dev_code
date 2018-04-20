using System.Threading.Tasks;

namespace Kadena.BusinessLogic.Contracts
{
    public interface IPdfService
    {
        Task<string> GetHiresPdfRedirectLink(string orderId, int line, string hash);
        string GetHiresPdfUrl(string orderId, int lineNumber);
    }
}
