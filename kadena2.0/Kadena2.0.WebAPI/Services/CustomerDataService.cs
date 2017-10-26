using Kadena.WebAPI.Contracts;
using Kadena.Models.CustomerData;
using System.Linq;
using Kadena.WebAPI.KenticoProviders.Contracts;
using System.Collections.Generic;
using Kadena.Models;

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

            var address = kenticoUsers.GetCustomerAddresses(customerId, AddressType.Shipping).FirstOrDefault();

            if (address == null)
                return null;

            var country = kenticoProvider.GetCountries().FirstOrDefault(c => c.Id == address.Country.Id);
            var state = kenticoProvider.GetStates().FirstOrDefault(s => s.Id == address.State.Id);

            var claims = GetCustomerClaims(siteId, customer.UserID);

            return new CustomerData()
            {
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Email = customer.Email,
                Phone = customer.Phone,
                PreferredLanguage = customer.PreferredLanguage,
                Address = new CustomerAddress
                {
                    Street = new List<string> { address.Street1, address.Street2 },
                    City = address.City,
                    Country = country.Name,
                    State = state.StateCode,
                    Zip = address.Zip
                },
                Claims = claims,
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