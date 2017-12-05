using Kadena.Models.CustomerData;

namespace Kadena.BusinessLogic.Contracts
{
    public interface ICustomerDataService
    {
        CustomerData GetCustomerData(int siteId, int customerId);
    }
}
