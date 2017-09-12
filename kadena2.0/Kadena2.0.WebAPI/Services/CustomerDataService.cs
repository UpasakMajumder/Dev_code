using Kadena.WebAPI.Contracts;
using Kadena.Models.CustomerData;
using System.Linq;
using Kadena.WebAPI.KenticoProviders.Contracts;
using System.Collections.Generic;

namespace Kadena.WebAPI.Services
{
    public class CustomerDataService : ICustomerDataService
    {
        private readonly IKenticoUserProvider kenticoUsers;
        private readonly IKenticoProviderService kenticoProvider;
        private readonly IKenticoResourceService kenticoResource;

        public CustomerDataService(IKenticoUserProvider kenticoUsers, IKenticoProviderService kenticoProvider, IKenticoResourceService kenticoResource)
        {
            this.kenticoUsers = kenticoUsers;
            this.kenticoProvider = kenticoProvider;
            this.kenticoResource = kenticoResource;
        }

        public CustomerData GetCustomerData(int siteId, int customerId)
        {
            var customer = kenticoUsers.GetCustomer(customerId);

            if (customer == null)
                return null;

            var address = kenticoUsers.GetCustomerShippingAddresses(customerId).FirstOrDefault();

            if (address == null)
                return null;

            var claims = GetCustomerClaims(siteId, customer.UserID);

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
                Claims = claims,
                SiteDomain = customer.SiteDomain
            };
        }

        private Dictionary<string, string> GetCustomerClaims(int siteId, int userId)
        {
            var claims = new Dictionary<string, string>();

            bool canSeePrices = kenticoUsers.UserCanSeePrices(siteId, userId);
            claims.Add("UserCanSeePrices", canSeePrices.ToString().ToLower());

            return claims;
        }
    }
}