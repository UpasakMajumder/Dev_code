using Kadena.BusinessLogic.Contracts;
using Kadena.Models.CustomerData;
using System.Linq;
using Kadena.WebAPI.KenticoProviders.Contracts;
using System.Collections.Generic;
using Kadena.Models;
using System;
using Kadena2.WebAPI.KenticoProviders.Contracts;

namespace Kadena.BusinessLogic.Services
{
    public class CustomerDataService : ICustomerDataService
    {
        private readonly IKenticoUserProvider kenticoUsers;
        private readonly IKenticoPermissionsProvider kenticoPermissions;
        private readonly IKenticoLocalizationProvider kenticoLocalization;
        private readonly IKenticoAddressBookProvider kenticoAddresses;

        public CustomerDataService(IKenticoUserProvider kenticoUsers, IKenticoPermissionsProvider kenticoPermissions, IKenticoLocalizationProvider kenticoLocalization, IKenticoAddressBookProvider kenticoAddresses)
        {
            if (kenticoUsers == null)
            {
                throw new ArgumentNullException(nameof(kenticoUsers));
            }
            if (kenticoPermissions == null)
            {
                throw new ArgumentNullException(nameof(kenticoPermissions));
            }
            if (kenticoLocalization == null)
            {
                throw new ArgumentNullException(nameof(kenticoLocalization));
            }
            if (kenticoAddresses == null)
            {
                throw new ArgumentNullException(nameof(kenticoAddresses));
            }

            this.kenticoUsers = kenticoUsers;
            this.kenticoPermissions = kenticoPermissions;
            this.kenticoLocalization = kenticoLocalization;
            this.kenticoAddresses = kenticoAddresses;
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

            var address = kenticoAddresses.GetCustomerAddresses(customerId, AddressType.Shipping).FirstOrDefault();
            if (address != null)
            {
                var country = kenticoLocalization.GetCountries().FirstOrDefault(c => c.Id == address.Country.Id);
                var state = kenticoLocalization.GetStates().FirstOrDefault(s => s.Id == address.State.Id);

                customerData.Address = new CustomerAddress
                {
                    Street = new List<string> { address.Address1, address.Address2 },
                    City = address.City,
                    Country = country.Name,
                    State = state?.StateDisplayName,
                    Zip = address.Zip
                };
            }

            return  customerData;
        }

        private Dictionary<string, string> GetCustomerClaims(int siteId, int userId)
        {
            var claims = new Dictionary<string, string>();

            bool canSeePrices = kenticoPermissions.UserCanSeePrices(siteId, userId);
            claims.Add("UserCanSeePrices", canSeePrices.ToString().ToLower());

            bool canDownloadHiresPdf = kenticoPermissions.UserCanDownloadHiresPdf(siteId, userId);
            claims.Add("UserCanDownloadHiResPdf", canDownloadHiresPdf.ToString().ToLower());

            return claims;
        }
    }
}