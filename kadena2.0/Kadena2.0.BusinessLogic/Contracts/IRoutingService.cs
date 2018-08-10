using System.Collections.Generic;
using System.Threading.Tasks;
using Kadena.Models.Routing;
using Kadena.Models.Routing.Request;

namespace Kadena.BusinessLogic.Contracts
{
    public interface IRoutingService
    {
        Task<IEnumerable<Routing>> GetRoutings(string orderId = null, string siteId = null);
        Task<Routing> SetRouting(SetRouting data);
        Task<bool> DeleteRouting(DeleteRouting data);
    }
}
