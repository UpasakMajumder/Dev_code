using System.Threading.Tasks;

namespace Kadena.WebAPI.Contracts
{
    public interface IPdfService
    {
        Task<string> GetHiresPdfLink(string orderId, int line);
    }
}
