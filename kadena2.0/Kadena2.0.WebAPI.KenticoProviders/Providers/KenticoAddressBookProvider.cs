using CMS.Ecommerce;
using CMS.SiteProvider;
using Kadena.WebAPI.KenticoProviders.Contracts;
using System.Collections.Generic;
using System.Linq;

namespace Kadena.WebAPI.KenticoProviders
{
    public class KenticoAddressBookProvider : IKenticoAddressBookProvider
    {
        public void DeleteAddress(int addressID)
        {
            var address = AddressInfoProvider.GetAddressInfo(addressID);
            if (address != null)
            {
                AddressInfoProvider.DeleteAddressInfo(addressID);
            }
        }

        public Dictionary<int, string> GetAddressNames()
        {
            List<int> customerIDs = CustomerInfoProvider.GetCustomers().Where(x => x.CustomerSiteID.Equals(SiteContext.CurrentSiteID)).Select(x => x.CustomerID).ToList();
            return AddressInfoProvider.GetAddresses().WhereIn("AddressCustomerID", customerIDs).ToDictionary(x => x.AddressID, x => x.AddressName);
        }
    }
}
