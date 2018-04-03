using Kadena2.MicroserviceClients.Contracts;
using System;
using System.Threading.Tasks;

namespace Kadena2.MicroserviceClients.Clients
{
    public class OrderResubmitClient : IOrderResubmitClient
    {
        public Task Resubmit(string orderId)
        {
            throw new NotImplementedException();
        }
    }
}
