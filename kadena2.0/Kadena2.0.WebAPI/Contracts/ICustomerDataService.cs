using Kadena.WebAPI.Models.CustomerData;

namespace Kadena.WebAPI.Contracts
{
    public interface ICustomerDataService
    {
        string GetAdmingEmail(int customerId);
        CustomerData GetCustomerData(int customerId);
    }
}
