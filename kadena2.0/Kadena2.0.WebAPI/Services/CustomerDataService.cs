using Kadena.WebAPI.Contracts;
using Kadena.WebAPI.Models.CustomerData;
using System.Linq;

namespace Kadena.WebAPI.Services
{
    public class CustomerDataService : ICustomerDataService
    {
        IKenticoProviderService kenticoProvider;

        public CustomerDataService(IKenticoProviderService kenticoProvider)
        {
            this.kenticoProvider = kenticoProvider;
        }

        public CustomerData GetCustomerData(int customerId)
        {
            var customer = kenticoProvider.GetCustomer(customerId);

            if (customer == null)
                return null;
            
            var address = kenticoProvider.GetCustomerShippingAddresses(customerId).FirstOrDefault();

            if (address == null)
                return null;

            return new CustomerData()
            {
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Email = customer.Email,
                Phone = customer.Phone,
                Address = new CustomerAddress()
                {
                    Street = address.Street,
                    City = address.City,
                    Country = address.Country,
                    State = address.State,
                    Zip = address.Zip
                }
            };
        }
    }
}