using Kadena.Models.CustomerData;

namespace Kadena.WebAPI.Contracts
{
    public interface ICustomerDataService
    {
        CustomerData GetCustomerData(int siteId, int customerId);
    }
}
