using Kadena.WebAPI.Contracts;
using Kadena.Models.CustomerData;
using System.Linq;
using Kadena.WebAPI.KenticoProviders.Contracts;
using System.Collections.Generic;

namespace Kadena.WebAPI.Services
{
    public class CustomerDataService : ICustomerDataService
    {
        IKenticoProviderService kenticoProvider;
        IKenticoResourceService kenticoResource;

        public CustomerDataService(IKenticoProviderService kenticoProvider, IKenticoResourceService kenticoResource)
        {
            this.kenticoProvider = kenticoProvider;
            this.kenticoResource = kenticoResource;
        }

        public CustomerData GetCustomerData(int customerId)
        {
            var customer = kenticoProvider.GetCustomer(customerId);

            if (customer == null)
                return null;

            var address = kenticoProvider.GetCustomerShippingAddresses(customerId).FirstOrDefault();

            if (address == null)
                return null;

            var claims = GetCustomerClaims(customer.UserID);

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
                },
                Claims = claims
            };
        }

        private Dictionary<string, string> GetCustomerClaims(int userId)
        {
            var claims = new Dictionary<string, string>();

            bool canSeePrices = kenticoProvider.UserCanSeePrices(userId);
            claims.Add("UserCanSeePrices", canSeePrices.ToString().ToLower());

            return claims;
        }
    }
}