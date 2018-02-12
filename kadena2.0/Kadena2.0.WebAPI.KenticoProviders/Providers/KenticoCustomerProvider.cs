using CMS.Ecommerce;
using Kadena.WebAPI.KenticoProviders.Contracts;
using System.Linq;

namespace Kadena.WebAPI.KenticoProviders.Providers
{
    public class KenticoCustomerProvider : IKenticoCustomerProvider
    {
        public int GetUserIDByCustomerID(int customerID)
        {
            return CustomerInfoProvider.GetCustomers().WhereEquals("CustomerID", customerID).FirstOrDefault()?.CustomerUserID ?? 0;
        }

        public int GetCustomerIDByUserID(int userID)
        {
            return CustomerInfoProvider.GetCustomers().WhereEquals("UserID", userID).FirstOrDefault()?.CustomerID ?? 0;
        }
    }
}
