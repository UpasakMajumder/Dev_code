using CMS.Ecommerce;
using Kadena.Models;
using Kadena.WebAPI.KenticoProviders;

namespace Kadena2.WebAPI.KenticoProviders.Factories
{
    public static class CustomerFactory
    {
        public static Customer CreateCustomer(CustomerInfo customer)
        {
            return new Customer()
            {
                Id = customer.CustomerID,
                FirstName = customer.CustomerFirstName,
                LastName = customer.CustomerLastName,
                Email = customer.CustomerEmail,
                CustomerNumber = customer.CustomerGUID.ToString(),
                Phone = customer.CustomerPhone,
                UserID = customer.CustomerUserID,
                Company = customer.CustomerCompany,
                SiteId = customer.CustomerSiteID,
                PreferredLanguage = customer.CustomerUser?.PreferredCultureCode ?? string.Empty,
                DefaultShippingAddressId = customer.GetIntegerValue(KenticoUserProvider.CustomerDefaultShippingAddresIDFieldName, 0)
            };
        }
    }
}