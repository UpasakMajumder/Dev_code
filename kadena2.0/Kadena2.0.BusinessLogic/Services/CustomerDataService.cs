using Kadena.BusinessLogic.Contracts;
using Kadena.Models.CustomerData;
using System.Linq;
using Kadena.WebAPI.KenticoProviders.Contracts;
using System.Collections.Generic;
using Kadena.Models;
using System;

namespace Kadena.BusinessLogic.Services
{
    public class CustomerDataService : ICustomerDataService
    {
        private readonly IKenticoUserProvider kenticoUsers;
        private readonly IKenticoProviderService kenticoProvider;

        public CustomerDataService(IKenticoUserProvider kenticoUsers, IKenticoProviderService kenticoProvider)
        {
            if (kenticoUsers == null)
            {
                throw new ArgumentNullException(nameof(kenticoUsers));
            }
            if (kenticoProvider == null)
            {
                throw new ArgumentNullException(nameof(kenticoProvider));
            }

            this.kenticoUsers = kenticoUsers;
            this.kenticoProvider = kenticoProvider;
        }

        public CustomerData GetCustomerData(int siteId, int customerId)
        {
            var customer = kenticoUsers.GetCustomer(customerId);

            if (customer == null)
                return null;
                    
            var claims = GetCustomerClaims(siteId, customer.UserID);

            var customerData = new CustomerData()
            {
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Email = customer.Email,
                Phone = customer.Phone,
                PreferredLanguage = customer.PreferredLanguage,
                Address = null,
                Claims = claims,
            };

            var address = kenticoUsers.GetCustomerAddresses(customerId, AddressType.Shipping).FirstOrDefault();
            if (address != null)
            {
                var country = kenticoProvider.GetCountries().FirstOrDefault(c => c.Id == address.Country.Id);
                var state = kenticoProvider.GetStates().FirstOrDefault(s => s.Id == address.State.Id);

                customerData.Address = new CustomerAddress
                {
                    Street = new List<string> { address.Address1, address.Address2 },
                    City = address.City,
                    Country = country.Name,
                    State = state?.StateCode,
                    Zip = address.Zip
                };
            }

            return  customerData;
        }

        private Dictionary<string, string> GetCustomerClaims(int siteId, int userId)
        {
            var claims = new Dictionary<string, string>();

            bool canSeePrices = kenticoUsers.UserCanSeePrices(siteId, userId);
            claims.Add("UserCanSeePrices", canSeePrices.ToString().ToLower());

            bool canDownloadHiresPdf = kenticoUsers.UserCanDownloadHiresPdf(siteId, userId);
            claims.Add("UserCanDownloadHiResPdf", canDownloadHiresPdf.ToString().ToLower());

            return claims;
        }
    }
}