using Kadena.WebAPI.Models;
using Kadena.WebAPI.Models.CustomerData;

namespace Kadena.WebAPI.Contracts
{
    public interface ICustomerDataService
    {
        CustomerData GetCustomerData(int customerId);
    }
}
