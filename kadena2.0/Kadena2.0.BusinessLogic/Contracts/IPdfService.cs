using System.Threading.Tasks;

namespace Kadena.BusinessLogic.Contracts
{
    public interface IPdfService
    {
        Task<string> GetHiresPdfLink(string orderId, int line);
    }
}
