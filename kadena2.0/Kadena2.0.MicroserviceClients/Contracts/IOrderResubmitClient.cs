using System.Threading.Tasks;

namespace Kadena2.MicroserviceClients.Contracts
{
    public interface IOrderResubmitClient
    {
        Task Resubmit(string orderId);
    }
}
