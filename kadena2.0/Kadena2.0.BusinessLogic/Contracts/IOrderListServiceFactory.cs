using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kadena.BusinessLogic.Contracts
{
    public interface IOrderListServiceFactory
    {
        IOrderListService GetDashboard();
        IOrderListService GetRecentOrders();
    }
}
